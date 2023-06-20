using AnimalOrder.Entities;
using AnimalOrder.Models.Data;

namespace AnimalOrder.Models.Repository
{
    public interface IAnimalRepository
    {
        Task<Animal> CreateAnimalAsync(Animal animal);
        Task<bool> UpdateAnimalBulkAsync(IEnumerable<Animal> animals);
        Task<bool> DeactivateAnimalsByIds(IEnumerable<Animal> AnimalsIds);
        Task<bool> DeleteAnimalAsync(Animal animal);
        Task<Animal?> GetAnimalById(int id);
        Task<IEnumerable<Animal>> GetAnimals();
        Task<bool> UpdateAnimalAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAdultAnimals();
        Task<object> AnimalsBySex();
        Task<IEnumerable<Animal>> GetAnimalsByIds(List<int> AnimalsIds);
        Task<IEnumerable<Animal>?> GetAnimalsByfilter(AnimalFilter filter);

    }
}
