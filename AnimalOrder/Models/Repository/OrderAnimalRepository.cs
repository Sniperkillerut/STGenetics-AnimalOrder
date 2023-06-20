using AnimalOrder.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace AnimalOrder.Models.Repository
{
    public class OrderAnimalRepository : IOrderAnimalRepository
    {
        protected readonly OrderContext _context;
        public OrderAnimalRepository(OrderContext context) => _context = context;

        public async Task<OrderAnimal> CreateOrderAnimalAsync(OrderAnimal orderAnimal)
        {
            await _context.Set<OrderAnimal>().AddAsync(orderAnimal);
            await _context.SaveChangesAsync();
            return orderAnimal;
        }
        public async Task<bool> CreateOrderAnimalBulkAsync(int OrderId, List<int> AnimalsIds)
        {
            List<OrderAnimal> orderAnimals = new();
            foreach (var Animal in AnimalsIds)
            {
                orderAnimals.Add(new OrderAnimal(OrderId, Animal));
            }
            await _context.Set<OrderAnimal>().AddRangeAsync(orderAnimals);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAnimalAsync(OrderAnimal orderAnimal)
        {
            if (orderAnimal is null)
            {
                return false;
            }
            _context.Set<OrderAnimal>().Remove(orderAnimal);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<OrderAnimal?> GetOrderAnimalById(int id)
        {
            return await _context.OrderAnimals.FindAsync(id);
        }

        public async Task<IEnumerable<OrderAnimal>> GetOrderAnimals()
        {
            return await _context.OrderAnimals.ToListAsync();
        }

        public async Task<bool> UpdateOrderAnimalAsync(OrderAnimal orderAnimal)
        {
            _context.Entry(orderAnimal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
