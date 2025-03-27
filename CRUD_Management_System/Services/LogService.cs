using Serilog;
using CRUD_Management_System.Models;
using System.Diagnostics;

namespace CRUD_Management_System.Services
{
    public class LogService
    {
        public void LogNewUserDetails(UserLoginModel newUserLogin, UserDetailsModel newUserDetails,string generatedPassword, string currentUser)
        {
            if (
                  !string.IsNullOrEmpty(currentUser) 
               && !string.IsNullOrEmpty(newUserLogin.AliasId)
               && !string.IsNullOrEmpty(generatedPassword))
            {
                Log.Information(
                @$" [NEW USER ACCOUNT by [{currentUser.ToUpper()}]]
                AliasId: {newUserLogin.AliasId} Password: {generatedPassword}
                ---
                {newUserDetails.Name} {newUserDetails.Surname}
                {newUserDetails.Address} {newUserDetails.ZIP} {newUserDetails.City}
                {newUserDetails.Email} {newUserDetails.Phonenumber}
                ---
            ");
            }
            else
            {
                Log.Error(
                @$" [ERROR CREATING NEW USER ACCOUNT]
                Created by: {currentUser.ToUpper()}
                AliasId: {newUserLogin.AliasId} Password: {generatedPassword}
                ---
                {newUserDetails.Name} {newUserDetails.Surname}
                {newUserDetails.Address} {newUserDetails.ZIP} {newUserDetails.City}
                {newUserDetails.Email} {newUserDetails.Phonenumber}
                ---
                ");
            }
        }

        public void LogEditUserDetails(UserDetailsModel updatedUser, string currentUser)
        {
            if (!string.IsNullOrEmpty(currentUser) && updatedUser != null)
            {
                Log.Information(
                @$" [EDIT USERDETAILS by {currentUser.ToUpper()}]
                ---
                {updatedUser.Alias}
                {updatedUser.Name} {updatedUser.Surname}
                {updatedUser.Address} {updatedUser.ZIP} {updatedUser.City}
                {updatedUser.Email} {updatedUser.Phonenumber}
                ---
                ");
            }
            else
            {
                Log.Error(
                @$" [ERROR EDIT USERDETAILS by {currentUser.ToUpper()}]
                ---
                {updatedUser!.Alias}
                {updatedUser.Name} {updatedUser.Surname}
                {updatedUser.Address} {updatedUser.ZIP} {updatedUser.City}
                {updatedUser.Email} {updatedUser.Phonenumber}
                ---
                ");
            }
        }

        public void LogDeleteUserAccount(string currentUser, string deletedUser)
        {
            if (!string.IsNullOrEmpty(currentUser) && !string.IsNullOrEmpty(deletedUser))
            {
            Log.Information(
            $@" [DELETE USER ACCOUNT by {currentUser.ToUpper()}]
            User Account [{deletedUser.ToUpper()}] is deleted by [{currentUser.ToUpper()}]
            ");
            }
            else
            {
            Log.Error(
            $@" [ERROR DELETE USER ACCOUNT]
            User Account [{deletedUser.ToUpper()}] is deleted by [{currentUser.ToUpper()}]
            ");
            }
        }

        public void LogLogin(string currentUser, string userRole)
        {
            Log.Information
            (@$" [LOGIN] 
            [{userRole.ToUpper()}] {currentUser.ToUpper()} logged in
            ");
        }

        public void LogLogout(string currentUser, string userRole)
        {
            Log.Information
            (@$" [LOGOUT] 
            [{userRole.ToUpper()}] [{currentUser.ToUpper()}] logged out
            ");
        }
    }
}
