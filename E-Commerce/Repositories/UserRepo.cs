using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace E_Commerce.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext db;

        public UserRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddUser(Users user)
        {
            int result = 0;
            user.RoleId = 2;
            db.Users.Add(user);
            result=db.SaveChanges();
            return result;
        }

        public int DeleteUser(int id)
        {
            int result = 0;
            var model = db.Users.Where(user => user.Id == id).FirstOrDefault();
            if (model != null)
            {
                db.Users.Remove(model);
                result = db.SaveChanges();
            }
            return result;
        }

        public int EditUser(Users user)
        {
            int result = 0;
            var model = db.Users.Where(bk => bk.Id == user.Id).FirstOrDefault();
            if (model != null)
            {
                model.Name = user.Name;
                model.Email = user.Email;
                model.Password = user.Password;
                result = db.SaveChanges();
            }
            return result;
        }

        public Users GetUserById(int id)
        {
            return db.Users.Where(x => x.Id == id).SingleOrDefault();
        }
        public Users GetUserByEmail(string email)
        {
            return db.Users.Where(x => x.Email == email).SingleOrDefault();
        }

        public IEnumerable<Users> GetUsers()
        {
            var model = (from user in db.Users
                         select user).ToList();
            return model;
        }

      
    }
}
