using JobFreela.Payments.API.Models.InputModels;

namespace JobFreela.Payments.API.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> Process(PaymentInfoInputModel paymentInfoInputModel);
}
