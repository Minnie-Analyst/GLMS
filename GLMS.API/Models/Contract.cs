namespace GLMS.API.Models;

public class Contract
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; } //  nullable

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string? Status { get; set; } //  nullable
    public string? ServiceLevel { get; set; }

    public string? FilePath { get; set; }

    public List<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>(); //  initialize
}