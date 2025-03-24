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
            Log.Error(
            @$" [TEST LogNewUserDetails]
            test Log.Error
            "); 
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
            Log.Error(
            @$" [TEST LogEditUserDetails]
            test Log.Error
            ");
        }

        public void LogDeleteUserAccount(string currentUser, string deletedUser)
        {
            if (!string.IsNullOrEmpty(currentUser) && !string.IsNullOrEmpty(deletedUser))
            {
            Log.Information(
            $@" [DELETE USER ACCOUNT by [{currentUser.ToUpper()}]]
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
            Log.Error(
            @$" [TEST LogDeleteUserAccount]
            test Log.Error
            ");
        }

        public void LogLogin(string currentUser, string userRole)
        {
            Log.Information
            (@$" [LOGIN] 
            [{userRole.ToUpper()}] {currentUser.ToUpper()} logged in
            ");

            Log.Error(
            @$" [TEST LogLogin]
            test Log.Error
            ");
        }

        public void LogLogout(string currentUser, string userRole)
        {
            Log.Information
            (@$" [LOGOUT] 
            {userRole} [{currentUser.ToUpper()}] logged out
            ");

            Log.Error(
            @$" [TEST LogLout]
            test Log.Error
            ");
        }
    }
}
