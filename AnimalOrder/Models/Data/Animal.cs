using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnimalOrder.Models.Data
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }
        public required string Name { get; set; }
        public required string Breed { get; set; }
        public required DateTime BirthDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required AnimalSexEnum Sex { get; set; }
        public required double Price { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required StatusEnum Status { get; set; }
    }

    public enum AnimalSexEnum : int
    {
        Male = 1,
        Female = 2
    }
    public enum StatusEnum : int
    {
        Active = 1,
        Inactive = 2
    }
}
