using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AnimalOrder.Models.Data
{
    public class OrderAnimal
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Order")]
        public required int OrderId { get; set; }
        [ForeignKey("Animal")]
        public required int AnimalId { get; set; }

        public OrderAnimal() { }

        [SetsRequiredMembers]
        public OrderAnimal(int orderId, int animalId) { 
            OrderId = orderId;
            AnimalId = animalId;
        }
    }
}

