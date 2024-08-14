﻿using JobFreela.Payments.API.Models.InputModels;
using JobFreela.Payments.API.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace JobFreela.Payments.API.Consumer;

public class ProcessPaymentConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _provider;
    private const string QUEUE_NAME = "payments";
    public ProcessPaymentConsumer(IServiceProvider provider)
    {
        _provider = provider;

        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, eventArgs) =>
        {
            var byteArray = eventArgs.Body.ToArray();
            var paymentInfoJson = Encoding.UTF8.GetString(byteArray);

            var paymentInfo = JsonSerializer.Deserialize<PaymentInfoInputModel>(paymentInfoJson);

            ProcessPayment(paymentInfo);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(QUEUE_NAME, false, consumer);

        return Task.CompletedTask;
    }

    public void ProcessPayment(PaymentInfoInputModel paymentInfo)
    {
        using (var scope = _provider.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<IPaymentService>();
            service.Process(paymentInfo);
        }
    }
}
