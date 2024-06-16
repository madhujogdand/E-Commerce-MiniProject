using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface IUserService
    {
        IEnumerable<Users> GetUsers();
        Users GetUserById(int id);
        Users GetUserByEmail(string Email);
        int AddUser(Users user);
        int EditUser(Users user);
        int DeleteUser(int id);
        Users UpdateUserStatus(int userId, int isActive);
    }
}
