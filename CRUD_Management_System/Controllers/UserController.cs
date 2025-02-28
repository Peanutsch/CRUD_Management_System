﻿using CRUD_Management_System.Data;
using CRUD_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser([FromBody] UserDetailsModel update)
    {
        if (update == null || string.IsNullOrEmpty(update.Alias))
        {
            return BadRequest("Ongeldige gebruikersgegevens.");
        }

        var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == update.Alias);
        if (user == null)
        {
            return NotFound("Gebruiker niet gevonden.");
        }

        // Update velden
        user.Name = update.Name;
        user.Surname = update.Surname;
        user.Address = update.Address;
        user.ZIP = update.ZIP;
        user.City = update.City;
        user.Email = update.Email;
        user.Phonenumber = update.Phonenumber;
        user.Online = update.Online;
        user.Sick = update.Sick;

        // Opslaan in de database
        await _context.SaveChangesAsync();

        return Ok(user); // Stuur de bijgewerkte gebruiker terug als JSON
    }

    [HttpDelete]
    //[Route("delete")]
    public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Alias))
        {
            return BadRequest(new { message = "Ongeldige gebruikersalias." }); // Zorg ervoor dat je hier een object retourneert
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.AliasId == request.Alias);
        var userDetails = await _context.UserDetails.FirstOrDefaultAsync(u => u.Alias == request.Alias);
        if (user == null || userDetails == null)
        {
            return NotFound(new { message = "Gebruiker niet gevonden." }); // Retourneer een object met een message
        }

        // Verwijder de gebruiker
        _context.Users.Remove(user);
        _context.UserDetails.Remove(userDetails);
        await _context.SaveChangesAsync();

        await GetAllUsers();

        return Ok(new { message = "Gebruiker succesvol verwijderd." }); // Retourneer een object met een message
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _context.UserDetails.ToListAsync();
        return Ok(users); // Retourneer de lijst met gebruikers als JSON
    }
}
