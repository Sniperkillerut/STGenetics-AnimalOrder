using AnimalOrder.Entities;
using AnimalOrder.Models.Data;
using AnimalOrder.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalOrder.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : Controller
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        [HttpGet]
        [ActionName(nameof(GetAnimalsAsync))]
        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            return await _animalRepository.GetAnimals();
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetAnimalByIdAsync))]
        public async Task<ActionResult<Animal?>> GetAnimalByIdAsync(int id)
        {
            var animalByID = await _animalRepository.GetAnimalById(id);
            if (animalByID == null)
            {
                return NotFound();
            }
            return animalByID;
        }

        [HttpPost]
        [ActionName(nameof(CreateAnimalAsync))]
        public async Task<ActionResult<Animal>> CreateAnimalAsync(Animal animal)
        {
            await _animalRepository.CreateAnimalAsync(animal);
            return CreatedAtAction(nameof(GetAnimalByIdAsync), new { id = animal.AnimalId }, animal);
        }

        [HttpPut("{id}")]
        [ActionName(nameof(UpdateAnimal))]
        public async Task<ActionResult> UpdateAnimal(int id, Animal animal)
        {
            var animaldb = _animalRepository.GetAnimalById(id).Result;
            if (animaldb == null)
            {
                return NotFound();
            }
            if (animal is null)
            {
                return BadRequest();
            }
            animaldb.Sex = animal.Sex;
            animaldb.Status = animal.Status;
            animaldb.Name = animal.Name;
            animaldb.Price = animal.Price;
            animaldb.BirthDate = animal.BirthDate;
            animaldb.Breed = animal.Breed;

            if (await _animalRepository.UpdateAnimalAsync(animaldb))
            {
                return Ok();
            }
            else {
                return BadRequest();
            };
        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteAnimal))]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = _animalRepository.GetAnimalById(id).Result;
            if (animal == null)
            {
                return NotFound();
            }

            await _animalRepository.DeleteAnimalAsync(animal);

            return Ok();
        }

        [HttpGet("GetAdults")]
        [ActionName(nameof(GetAdultAnimalsAsync))]
        public async Task<IEnumerable<Animal>> GetAdultAnimalsAsync()
        {            
            return await _animalRepository.GetAdultAnimals();
        }
        [HttpGet("CountBySex")]
        [ActionName(nameof(AnimalsBySexAsync))]
        public async Task<object> AnimalsBySexAsync()
        {
            return await _animalRepository.AnimalsBySex();
        }
        [HttpGet("GetByList")]
        [ActionName(nameof(GetAnimalsByIdsAsync))]
        public async Task<object> GetAnimalsByIdsAsync(List<int> AnimalsIds)
        {
            return await _animalRepository.GetAnimalsByIds(AnimalsIds);
        }
        [HttpPost("GetWithFilter")]
        [ActionName(nameof(GetAnimalsWithFilterAsync))]
        public async Task<ActionResult<Animal?>> GetAnimalsWithFilterAsync(AnimalFilter filter)
        {
            if (filter is null
                || filter.Value is null)
            {
                return BadRequest();
            }
            var result = await _animalRepository.GetAnimalsByfilter(filter);
            if (!result.Any())
            {
                return NotFound("Could not find Animals with the given parameters, please check.");
            }
            return CreatedAtAction(nameof(GetAnimalByIdAsync), new { id = result.First().AnimalId }, result);
        }
    }
}
