using System.ComponentModel.DataAnnotations;

namespace CRUD_Management_System.Models
{
    public class LoginModel
    {
        public string? AliasId { get; set; }
        public string? Password { get; set; }
    }
}
