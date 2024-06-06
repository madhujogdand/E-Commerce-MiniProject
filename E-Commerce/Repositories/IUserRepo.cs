using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public interface IUserRepo
    {
        IEnumerable<Users> GetUsers();
        Users GetUserById(int id);
        Users GetUserByEmail(string Email);
        int AddUser(Users user);
        int EditUser(Users user);
        int DeleteUser(int id);
    }
}
