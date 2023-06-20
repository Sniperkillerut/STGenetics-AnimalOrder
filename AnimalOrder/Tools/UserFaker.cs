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
    public class UserFaker
    {
        public static List<User> Users = new();
    
        public static void Init(int count)
        {
            var userFaker = new Faker<User>().UseSeed(8675309)//Set the seed if you wish to generate repeatable data sets.
               .RuleFor(b => b.UserName, f => f.Person.UserName)
               .RuleFor(b => b.Password, f => f.Internet.Password())
               .RuleFor(b => b.Email, f => f.Person.Email)
               .RuleFor(b => b.FirstName, f => f.Person.FirstName)
               .RuleFor(b => b.LastName, f => f.Person.LastName)
               .RuleFor(b => b.PhoneNumber, f => f.Person.Phone);

            var users = userFaker.Generate(count);

            UserFaker.Users.AddRange(users);
        }
    }
}
