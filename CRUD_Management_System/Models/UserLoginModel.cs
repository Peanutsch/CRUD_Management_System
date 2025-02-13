
namespace CRUD_Management_System.Models 
    
{
    public class ModelUserLogin 
    {
        public string Alias { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Admin { get; set; }
        public bool OnlineStatus { get; set; }
        public bool TheOne { get; set; }
    }
}
