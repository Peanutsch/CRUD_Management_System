using System.ComponentModel.DataAnnotations;

namespace CRUD_Management_System.Models;

public class UserDetailsModel
{
    [Key]
    [Required(ErrorMessage = "Alias is verplicht")]
    public string Alias { get; set; } = null!;

    [Required(ErrorMessage = "Naam is verplicht")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Achternaam is verplicht")]
    public string Surname { get; set; } = null!;

    public string Address { get; set; } = "Unknown"; 
    public string ZIP { get; set; } = "Unknown";
    public string City { get; set; } = "Unknown";

    [Required(ErrorMessage = "Email is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
    public string Email { get; set; } = null!;

    public string Phonenumber { get; set; } = "Unknown";
    public bool Online { get; set; } = false;
    public bool Sick { get; set; } = false;
}
