using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface IOrderService
    {
      
        IEnumerable<Orders> GetOrders(int userId);
        IEnumerable<Orders> GetAllOrders();
        int UpdateOrderStatus(int orderItemId, int orderStatusId);
    }
}
