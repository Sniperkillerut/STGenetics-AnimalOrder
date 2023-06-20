using System.ComponentModel.DataAnnotations;

namespace AnimalOrder.Models.Data
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int AnimalsBought { get; set; }
        public double OrderValue { get; set; }
        public double OrderFreight { get; set; }
        public double OrderDiscount { get; set; }
        public double OrderTotal { get; set; }

    }
}
