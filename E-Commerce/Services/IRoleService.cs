using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoles();
    }
}
