using AnimalOrder.Entities;
using AnimalOrder.Models.Data;
using AnimalOrder.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace AnimalOrder.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderAnimalRepository _orderAnimalRepository;
        private readonly IAnimalRepository _animalRepository;

        public OrderController(IOrderRepository orderRepository, IAnimalRepository animalRepository, IOrderAnimalRepository orderAnimalRepository)
        {
            _orderRepository = orderRepository;
            _orderAnimalRepository = orderAnimalRepository;
            _animalRepository = animalRepository;
        }
        

        [HttpGet]
        [ActionName(nameof(GetOrdersAsync))]
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetOrders();
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetOrderByIdAsync))]
        public async Task<ActionResult<Order?>> GetOrderByIdAsync(int id)
        {
            var orderByID = await _orderRepository.GetOrderById(id);
            if (orderByID == null)
            {
                return NotFound();
            }
            return orderByID;
        }

        [HttpPost]
        [ActionName(nameof(CreateOrderAsync))]
        public async Task<ActionResult<Order>> CreateOrderAsync(WebOrder webOrder)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ","");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
            int userId = int.Parse(tokenS.Payload["UserId"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.


            if (webOrder is null
                || !webOrder.AnimalsIdToBuy.Any())
            {
                //empty array
                return BadRequest();
            }
            //check for duplicates
            List<int>? dupeFinder = webOrder.AnimalsIdToBuy.Distinct().ToList();
            if (dupeFinder.Count != webOrder.AnimalsIdToBuy.Count) {
                return BadRequest("Duplicate AnimalId found.");
            }
            //delete no longer needed var
            dupeFinder.Clear();
            dupeFinder.TrimExcess();
            dupeFinder = null;
            //delete no longer needed var

            IEnumerable<Animal> animalsDBlist = await _animalRepository.GetAnimalsByIds(webOrder.AnimalsIdToBuy);
            if (animalsDBlist.Count() != webOrder.AnimalsIdToBuy.Count
                || animalsDBlist.Where(a => a.Status == StatusEnum.Inactive).Any())
            {
                //if the db retuns a different number that the intended to buy
                //OR if there are Inactive animals
                return BadRequest("Some AnimalIds are not Avaliable, please check.");
            }

            double discountpercent = animalsDBlist.Count() > 200 ? 0.03 : 0;
            double OrderFreight = animalsDBlist.Count() > 300 ? 0 : 1000;
            double OrderValue = animalsDBlist.Sum(a => a.Price);
            discountpercent += animalsDBlist.GroupBy(a => a.Breed)
                                            .Select(g => new { breed = g.Key, counts = g.Count() })
                                            .OrderByDescending(x => x.counts)
                                            .First().counts > 50 ? 0.05 : 0;
            double OrderDiscount = OrderValue * discountpercent;

            Order newOrder = new Order() {
                UserId= userId,
                OrderDate=DateTime.Now,
                AnimalsBought=webOrder.AnimalsIdToBuy.Count(),
                OrderValue= OrderValue,
                OrderFreight= OrderFreight,
                OrderDiscount= OrderDiscount,
                OrderTotal= OrderValue+OrderFreight-OrderDiscount
            };
            var order = await _orderRepository.CreateOrderAsync(newOrder);
            await _orderAnimalRepository.CreateOrderAnimalBulkAsync(order.OrderId, webOrder.AnimalsIdToBuy);
            await _animalRepository.DeactivateAnimalsByIds(animalsDBlist);
            return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        [ActionName(nameof(UpdateOrder))]
        public async Task<ActionResult> UpdateOrder(int id, Order order)
        {//TODO: should not work this way
            var orderdb = _orderRepository.GetOrderById(id).Result;
            if (orderdb == null)
            {
                return NotFound();
            }
            if (order is null)
            {
                return BadRequest();
            }
            orderdb.OrderValue = order.OrderValue;
            orderdb.OrderDiscount = order.OrderDiscount;
            orderdb.OrderDate = order.OrderDate;
            orderdb.OrderFreight = order.OrderFreight;
            orderdb.AnimalsBought = order.AnimalsBought;
            orderdb.OrderTotal = order.OrderTotal;
            orderdb.UserId = order.UserId;

            if (await _orderRepository.UpdateOrderAsync(orderdb))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            };
        }

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteOrder))]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = _orderRepository.GetOrderById(id).Result;
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderAsync(order);

            return Ok();
        }

    }
}
