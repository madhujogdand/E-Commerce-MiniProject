using E_Commerce.Models;
using E_Commerce.Repositories;

namespace E_Commerce.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo repo;

        public RoleService(IRoleRepo repo)
        {
            this.repo = repo;
        }
        public IEnumerable<Role> GetRoles()
        {
            return repo.GetRoles();
        }
    }
}
