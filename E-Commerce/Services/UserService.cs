using E_Commerce.Models;
using E_Commerce.Repositories;

namespace E_Commerce.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo repo;

        public UserService(IUserRepo repo)
        {
            this.repo = repo;
        }
        public int AddUser(Users user)
        {
           return repo.AddUser(user);
        }

        public int DeleteUser(int id)
        {
            return repo.DeleteUser(id);
        }

        public int EditUser(Users user)
        {
            return repo.EditUser(user);
        }

        public Users GetUserByEmail(string email)
        {
            return repo.GetUserByEmail(email);
        }

        public Users GetUserById(int id)
        {
            return repo.GetUserById(id);
        }
       
        public IEnumerable<Users> GetUsers()
        {
            return repo.GetUsers();
        }

        public Users UpdateUserStatus(int userId, int isActive)
        {
          return repo.UpdateUserStatus(userId, isActive);
        }
    }
}
