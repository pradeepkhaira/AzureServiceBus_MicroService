using BurgerKingOrderProcess.MicroService.Models;
using Microsoft.EntityFrameworkCore;

namespace BurgerKingOrderProcess.MicroService.Data
{
    public class ProcessData : IProcessData
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        

        public ProcessData(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString= _configuration["ConnectionStrings:OrderProcessContext"];
        }

        public async Task<IEnumerable<BurgerKingOrder>> GetBurgerKingOrders()
        {
            using(var _orderProcessContext = new OrderProcessContext(_connectionString))
            {
                return await _orderProcessContext.BurgerKingOrder.ToListAsync();
            }
        }

        public async Task Process(BurgerOrder model)
        {
            using (var _orderProcessContext = new OrderProcessContext(_connectionString))
            {
                await _orderProcessContext.AddAsync(new BurgerKingOrder
                {
                    OderId = model.OderId,
                    OrderName = model.OrderName,
                    IsMeal = model.IsMeal,
                    Quantity = model.Quantity,
                });

                await _orderProcessContext.SaveChangesAsync();

            }
        }

    }
}
