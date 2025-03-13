﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Management_System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;

namespace CRUD_Management_System.Controllers
{
    public class DashboardAdminController : Controller
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardAdminController"/> class.
        /// </summary>
        /// <param name="context">Database context for accessing user data.</param>
        public DashboardAdminController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of users and displays them in the dashboard view.
        /// Retrieves the current user from TempData for display purposes.
        /// </summary>
        /// <returns>A view displaying the list of users and the current user.</returns>
        //[Route("Index")]
        public IActionResult Index()
        {
            Debug.WriteLine("<Running [LogginController] Index");
            var token = Request.Cookies["AuthToken"]; // Haal token uit de cookie

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                    if (jsonToken != null)
                    {
                        ViewData["CurrentUser"] = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                        ViewData["Role"] = jsonToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    }
                    else
                    {
                        Debug.WriteLine("[LoginController]\n Index > Invalid token format");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[LoginController]\n Index > Token error:\n{ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("[LoginController]\n Index > No token found in cookies");
            }

            return View();
        }
    }
}
