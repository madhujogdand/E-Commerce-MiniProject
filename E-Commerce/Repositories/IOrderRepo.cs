using E_Commerce.Models;

namespace E_Commerce.Repositories
{
    public interface IOrderRepo
    {
     
        IEnumerable<Orders> GetOrders(int userId);
        IEnumerable<Orders> GetAllOrders();
    }
}
