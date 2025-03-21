using Serilog;
using CRUD_Management_System.Models;
using System.Diagnostics;

namespace CRUD_Management_System.Services
{
    public class LogService
    {
        public void LogNewUserDetails(UserLoginModel newUserLogin, UserDetailsModel newUserDetails,string generatedPassword, string currentUser)
        {
            Log.Information(
            @$"
            [NEW USER ACCOUNT]
            Created by: {currentUser.ToUpper()}
            AliasId: {newUserLogin.AliasId} Password: {generatedPassword}
            
            {newUserDetails.Name} {newUserDetails.Surname}
            {newUserDetails.Address} {newUserDetails.ZIP} {newUserDetails.City}
            {newUserDetails.Email} {newUserDetails.Phonenumber}
            {Environment.NewLine}
            ");
        }

        public void LogEditUserDetails(UserDetailsModel updatedUser, string currentUser)
        {
            if (!string.IsNullOrEmpty(currentUser))
            {
                Log.Information(
                @$"
                [EDIT USERDETAILS by {currentUser}]
                {updatedUser.Alias}
                {updatedUser.Name} {updatedUser.Surname}
                {updatedUser.Address} {updatedUser.ZIP} {updatedUser.City}
                {updatedUser.Email} {updatedUser.Phonenumber}
                ");
            }
            else
            {
                Log.Error(
                @$"
                [EDIT USERDETAILS by {currentUser}]
                {updatedUser.Alias}
                {updatedUser.Name} {updatedUser.Surname}
                {updatedUser.Address} {updatedUser.ZIP} {updatedUser.City}
                {updatedUser.Email} {updatedUser.Phonenumber}
                ");
            }
        }

        public void LogLogin(string currentUser)
        {Log.Information
            (@$"
            [LOGIN] User [{currentUser.ToUpper()}] logged in
            ");
        }
    }
}
