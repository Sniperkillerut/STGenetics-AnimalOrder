using AnimalOrder.Models.Data;
using Bogus;
using Bogus.DataSets;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using static Bogus.DataSets.Name;

namespace AnimalOrder.Tools
{
    public class AnimalFaker
    {
        public static List<Animal> Animals = new();

        private static readonly string[] BreedEnum = new[] {
            "Angus",
            "Hereford",
            "Simmental",
            "Red Angus",
            "Charolais",
            "Club Calf",
            "Main-Anjou",
            "Shorthorn",
            "Gray Brahman",
            "Red Brahman",
            "Brangus",
            "Santa Gertrudis",
            "Gelbvieh",
            "Limousin",
            "Wagyu",
            "Holstein",
            "Jersey",
            "Ayrshire",
            "Brown Swiss",
            "Guernsey",
            "Milking Shorthorn"
        };
    
        public static void Init(int count)
        {
            var animalFaker = new Faker<Animal>().UseSeed(8675309)//Set the seed if you wish to generate repeatable data sets.
               .RuleFor(b => b.Name, f => f.Name.FirstName())
               .RuleFor(b => b.Sex, f => f.PickRandom<AnimalSexEnum>())
               .RuleFor(b => b.BirthDate, f => f.Date.Between(DateTime.Now.AddYears(-7), DateTime.Now))
               .RuleFor(b => b.Status, f => f.PickRandom<StatusEnum>())
               .RuleFor(b => b.Price, f => f.Random.Number(500, 3000))
               .RuleFor(b => b.Breed, f => f.PickRandomParam<string>(BreedEnum));

            var animals = animalFaker.Generate(count);

            AnimalFaker.Animals.AddRange(animals);
        }
    }
}
