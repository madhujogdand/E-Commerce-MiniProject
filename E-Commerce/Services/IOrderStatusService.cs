using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface IOrderStatusService
    {
        IEnumerable<OrderStatus> GetAllOrderStatus();
        OrderStatus GetOrderStatusById(int id);
        int AddOrderStatus(OrderStatus orderStatus);
        int EditOrderStatus(OrderStatus orderStatus);
        int DeleteOrderStatus(int id);
    }
}
