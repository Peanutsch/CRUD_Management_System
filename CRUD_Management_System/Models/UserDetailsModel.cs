using System.ComponentModel.DataAnnotations;

namespace CRUD_Management_System.Models;

public class UserDetailsModel
{
    [Key]
    public string Alias { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Address { get; set; } = null!;
    public string ZIP { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phonenumber { get; set; } = null!;
    public bool Online { get; set; }
    public bool Sick { get; set; }

}