using AnimalOrder.Models.Data;

namespace AnimalOrder.Models.Repository
{
    public interface IOrderAnimalRepository
    {
        Task<OrderAnimal> CreateOrderAnimalAsync(OrderAnimal orderAnimal);
        Task<bool> CreateOrderAnimalBulkAsync(int OrderId, List<int> AnimalsIds);
        Task<bool> DeleteOrderAnimalAsync(OrderAnimal orderAnimal);
        Task<OrderAnimal?> GetOrderAnimalById(int id);
        Task<IEnumerable<OrderAnimal>> GetOrderAnimals();
        Task<bool> UpdateOrderAnimalAsync(OrderAnimal orderAnimal);
    }
}
