using JobFreela.Payments.API.Models.InputModels;
using JobFreela.Payments.API.Services.Interfaces;

namespace JobFreela.Payments.API.Services;

public class PaymentService : IPaymentService
{
    public Task<bool> Process(PaymentInfoInputModel paymentInfoInputModel)
    {
        return Task.FromResult(true);
    }
}
