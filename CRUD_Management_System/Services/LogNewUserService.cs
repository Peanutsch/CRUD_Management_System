﻿using Serilog;
using CRUD_Management_System.Models;
using System.Diagnostics;

namespace CRUD_Management_System.Services
{
    public class LogNewUserService
    {
        public void LogUserDetails(UserLoginModel newUserLogin, string generatedPassword, string currentUser)
        {
            // Log de details van de nieuwe gebruiker
            Log.Information("[ NEW USER ACCOUNT ]");
            Log.Information($"Date: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");  // Datum en tijd
            Log.Information($"Created by: {currentUser}");  // De gebruiker die de actie uitvoert
            Log.Information($"AliasId: {newUserLogin.AliasId}");  // Alias van de nieuwe gebruiker
            Log.Information($"Password: {generatedPassword}");  // Het gegenereerde wachtwoord
            Log.Information($"Admin: {newUserLogin.Admin}");  // Admin status
            Log.Information($"OnlineStatus: {newUserLogin.OnlineStatus}");  // Online status
            Log.Information($"TheOne: {newUserLogin.TheOne}");  // TheOne status

            Debug.WriteLine($"[ NEW USER ACCOUNT ]");
            Debug.WriteLine($"Date: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");  // Datum en tijd
            Debug.WriteLine($"Created by: {currentUser}");  // De gebruiker die de actie uitvoert
            Debug.WriteLine($"AliasId: {newUserLogin.AliasId}");  // Alias van de nieuwe gebruiker
            Debug.WriteLine($"Password: {generatedPassword}");  // Het gegenereerde wachtwoord
            Debug.WriteLine($"Admin: {newUserLogin.Admin}");  // Admin status
            Debug.WriteLine($"OnlineStatus: {newUserLogin.OnlineStatus}");  // Online status
            Debug.WriteLine($"TheOne: {newUserLogin.TheOne}");  // TheOne status
        }

        public void LogUserCreationError(string errorMessage, string currentUser)
        {
            Log.Error("[ USER CREATION ERROR ]");
            Log.Error($"Error occurred while creating user: {errorMessage}");
            Log.Error($"Occurred by: {currentUser}");

            Debug.WriteLine($"[ USER CREATION ERROR ]");
            Debug.WriteLine($"Error occurred while creating user: {{errorMessage");
            Debug.WriteLine($"Occurred by: {currentUser}");
        }
    }
}
