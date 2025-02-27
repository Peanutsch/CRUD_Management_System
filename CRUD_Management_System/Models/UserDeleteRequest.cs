using System.ComponentModel.DataAnnotations;

namespace CRUD_Management_System.Models
{
    public class UserDeleteRequest
    {
        [Required]
        public string Alias { get; set; } = null!;
    }
}
