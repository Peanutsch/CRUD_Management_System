using Microsoft.AspNetCore.Mvc;
using CRUD_Management_System.Models;

namespace CRUD_Management_System.Controllers
{
    public class DashboardAdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentUser"] = TempData["CurrentUser"]; // Haal de waarde uit TempData
            return View();
        }
    }
}
