namespace CRUD_Management_System.Services
{
    public interface IUserService
    {
        // Methoden zoals CreateUser, UpdateUser, etc.
        bool DeleteUserByAlias(string alias);
        //bool CreateUser(string alias);
    }
}
