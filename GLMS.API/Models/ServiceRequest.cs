using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.API.Models;

public class ServiceRequest
{
    public int Id { get; set; }

    [Required]
    public int ContractId { get; set; }

    public Contract? Contract { get; set; }

    public string? Description { get; set; }

    [Required]
    public decimal CostUSD { get; set; }

    [NotMapped]
    public decimal TempZAR { get; set; }

    public decimal CostZAR { get; set; }

    public string? Status { get; set; }
}