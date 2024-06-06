using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public class RoleRepo : IRoleRepo
    {
        private readonly ApplicationDbContext db;
        public RoleRepo(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Role> GetRoles()
        {
            var model = (from role in db.Roles
                         select role).ToList();
            return model;
        }
    }
}
