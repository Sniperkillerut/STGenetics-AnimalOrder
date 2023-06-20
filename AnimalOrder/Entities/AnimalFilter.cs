using AnimalOrder.Models.Data;
using System.Text.Json.Serialization;


namespace AnimalOrder.Entities
{
    public class AnimalFilter
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required AnimalFilterEnum Filter { get; set; }

        public required string Value { get; set; }
    }

    public enum AnimalFilterEnum : int
    {
        AnimalId = 1,
        Name = 2,
        Sex = 3,
        Status=4,
        Breed=5
    }
}
