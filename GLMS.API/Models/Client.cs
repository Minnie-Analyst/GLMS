using System.ComponentModel.DataAnnotations;

namespace GLMS.API.Models;

public class Client
{
    public int Id { get; set; }

    // FULL NAME
    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(100)]
    [RegularExpression(
        @"^[a-zA-Z\s]+$",
        ErrorMessage = "Name cannot contain numbers or special characters")]
    public string? Name { get; set; }

    // SOUTH AFRICAN ID NUMBER
    [Required(ErrorMessage = "ID Number is required")]
    [RegularExpression(
        @"^\d{13}$",
        ErrorMessage = "ID Number must be exactly 13 digits")]
    public string? IdNumber { get; set; }

    // EMAIL
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string? ContactDetails { get; set; }

    // PHONE NUMBER
    [Required(ErrorMessage = "Phone Number is required")]
    [RegularExpression(
        @"^\d{10}$",
        ErrorMessage = "Phone number must be 10 digits")]
    public string? PhoneNumber { get; set; }

    // BUSINESS NAME
    [StringLength(100)]
    [RegularExpression(
        @"^[a-zA-Z0-9\s]*$",
        ErrorMessage = "Business Name cannot contain special characters")]
    public string? BusinessName { get; set; }

    // REGION
    [Required(ErrorMessage = "Region is required")]
    [StringLength(50)]
    public string? Region { get; set; }

    public List<Contract> Contracts { get; set; }
        = new List<Contract>();
}