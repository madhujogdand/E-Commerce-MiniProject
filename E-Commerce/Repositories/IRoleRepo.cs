using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public interface IRoleRepo
    {
        IEnumerable<Role> GetRoles();
    }
}
