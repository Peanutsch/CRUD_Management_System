using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;

namespace CRUD_Management_System.Controllers
{
    public class DashboardAdminController : Controller
    {
        public IActionResult Index()
        {
            var user = new UserLoginModel
            {
                AliasId = User.Identity?.Name ?? "Gast"
            };

            return View(user);
        }
    }
}
