using JobFreela.Payments.API.Models.InputModels;
using JobFreela.Payments.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobFreela.Payments.API.Controllers;

[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    public PaymentsController(IPaymentService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PaymentInfoInputModel paymentInfo)
    {
        var result = await _service.Process(paymentInfo);

        if (!result)
        {
            return BadRequest("PAYMENT_ERROR");
        }

        return NoContent();
    }
}
