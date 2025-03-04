using CRUD_Management_System.Models;

namespace CRUD_Management_System.Services
{
    /// <summary>
    /// Defines methods for managing users, including creation and deletion.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Deletes a user from the database based on their alias.
        /// </summary>
        /// <param name="alias">The alias of the user to be deleted.</param>
        /// <returns>True if the user was successfully deleted; otherwise, false.</returns>
        bool DeleteUserByAlias(string alias);

        /// <summary>
        /// Creates a new user and adds them to the database.
        /// </summary>
        /// <param name="user">The user details to be created.</param>
        /// <returns>True if the user was successfully created; otherwise, false.</returns>
        Task<bool> CreateUser(UserDetailsModel user);
    }
}
