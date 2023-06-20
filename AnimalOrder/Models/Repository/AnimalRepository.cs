using AnimalOrder.Entities;
using AnimalOrder.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace AnimalOrder.Models.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        protected readonly OrderContext _context;
        public AnimalRepository(OrderContext context) => _context = context;

        public async Task<Animal> CreateAnimalAsync(Animal animal)
        {
            await _context.Set<Animal>().AddAsync(animal);
            await _context.SaveChangesAsync();
            return animal;
        }
        public async Task<bool> UpdateAnimalBulkAsync(IEnumerable<Animal> animals)
        {
            foreach (var item in animals)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
                
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeactivateAnimalsByIds(IEnumerable<Animal> animalList)
        {
            foreach (var animal in animalList)
            {
                animal.Status = StatusEnum.Inactive;
            }
            _ = await UpdateAnimalBulkAsync(animalList);
            return true;
        }
        public async Task<bool> DeleteAnimalAsync(Animal animal)
        {
            if (animal is null)
            {
                return false;
            }
            _context.Set<Animal>().Remove(animal);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Animal?> GetAnimalById(int id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public async Task<IEnumerable<Animal>> GetAnimals()
        {
            return await _context.Animals.ToListAsync();
        }

        public async Task<bool> UpdateAnimalAsync(Animal animal)
        {
            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Animal>> GetAdultAnimals()
        {
            var adults=  await _context.Animals.Where(anim=> anim.BirthDate > DateTime.Now.AddYears(-2)&&anim.Sex==AnimalSexEnum.Female).OrderBy(anim => anim.Name).ToListAsync();
            return adults;
        }
        public async Task<object> AnimalsBySex()
        {
            var males = await _context.Animals.Where(anim => anim.Sex == AnimalSexEnum.Male).ToListAsync();
            var females = await _context.Animals.Where(anim => anim.Sex == AnimalSexEnum.Female).ToListAsync();
            var json = new
            {
                Males = males.Count,
                Females = females.Count,
                Total= males.Count + females.Count
            };
            return json;
        }

        public async Task<IEnumerable<Animal>> GetAnimalsByIds(List<int> AnimalsIds)
        {
            var animals = await _context.Animals.Where(t => AnimalsIds.Contains(t.AnimalId)).ToListAsync();
            return animals;
        }
        public async Task<IEnumerable<Animal>?> GetAnimalsByfilter(AnimalFilter filter) {
            List<Animal> animals = new();
            try
            {
                switch (filter.Filter)
                {
                    case AnimalFilterEnum.AnimalId:
                        animals = await _context.Animals.Where(t => t.AnimalId == int.Parse(filter.Value)).ToListAsync();
                        break;
                    case AnimalFilterEnum.Name:
                        animals = await _context.Animals.Where(t => t.Name == filter.Value).ToListAsync();
                        break;
                    case AnimalFilterEnum.Sex:
                        AnimalSexEnum sexEnum;
                        Enum.TryParse(filter.Value, out sexEnum);
                        animals = await _context.Animals.Where(t => t.Sex == sexEnum).ToListAsync();
                        break;
                    case AnimalFilterEnum.Status:
                        StatusEnum status;
                        Enum.TryParse(filter.Value, out status);
                        animals = await _context.Animals.Where(t => t.Status == status).ToListAsync();
                        break;
                    case AnimalFilterEnum.Breed:
                        animals = await _context.Animals.Where(t => t.Breed == filter.Value).ToListAsync();
                        break;
                    default:
                        animals = await _context.Animals.ToListAsync();
                        break;
                }
            }
            catch (Exception ex)
            {
                return animals;
            }

            return animals;
        }
    }
}
