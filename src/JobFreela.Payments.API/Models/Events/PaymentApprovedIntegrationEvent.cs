namespace JobFreela.Payments.API.Models.Events;

public class PaymentApprovedIntegrationEvent
{
    public PaymentApprovedIntegrationEvent(int idProject)
    {
        IdProject = idProject;
    }

    public int IdProject { get; set; }
}
