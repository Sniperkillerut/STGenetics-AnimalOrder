using AnimalOrder.Models.Data;

namespace AnimalOrder.Models.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Order order);
        Task<Order?> GetOrderById(int id);
        Task<IEnumerable<Order>> GetOrders();
        Task<bool> UpdateOrderAsync(Order order);
    }
}
