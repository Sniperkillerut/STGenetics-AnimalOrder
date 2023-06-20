using AnimalOrder.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace AnimalOrder.Models.Repository
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly OrderContext _context;
        public OrderRepository(OrderContext context) => _context = context;

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Set<Order>().AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrderAsync(Order order)
        {
            if (order is null)
            {
                return false;
            }
            _context.Set<Order>().Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
