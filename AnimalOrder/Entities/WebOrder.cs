
namespace AnimalOrder.Entities
{
    public class WebOrder
    {
        public required int UserId { get; set; }
        public required List<int> AnimalsIdToBuy { get; set; }
    }
}
