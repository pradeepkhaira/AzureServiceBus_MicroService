using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BurgerKingOrder.MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BKOrderController : ControllerBase
    {
        private readonly string connectionString;
        private readonly IConfiguration _configuration;
        private readonly string bkQueue;
        public BKOrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:BKServiceBus"];
            bkQueue = _configuration["ConnectionStrings:ServiceBusQueue"];
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(IEnumerable<BurgerOrder> orders)
        {
            await ProcessOrder(orders);
            return Ok();
        }

        private async Task ProcessOrder(IEnumerable<BurgerOrder> orders)
        {
            try
            {
                ServiceBusClient client = new ServiceBusClient(connectionString);
                ServiceBusSender sender = client.CreateSender(bkQueue);
                foreach (var order in orders)
                {
                    string jsonEntity = JsonSerializer.Serialize(order);
                    ServiceBusMessage serializedContents = new ServiceBusMessage(jsonEntity);
                    await sender.SendMessageAsync(serializedContents);
                }
            }
            catch (Exception ex)
            {
                
            }
            

        }
    }
}
