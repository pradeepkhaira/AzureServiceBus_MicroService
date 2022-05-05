using BurgerKingOrderProcess.MicroService.Models;
using Microsoft.EntityFrameworkCore;

namespace BurgerKingOrderProcess.MicroService.Data
{
    public class ProcessData : IProcessData
    {
        private readonly OrderProcessContext _orderProcessContext;

        public ProcessData(OrderProcessContext orderProcessContext)
        {
            _orderProcessContext = orderProcessContext;
        }

        public async Task<IEnumerable<BurgerKingOrder>> GetBurgerKingOrders()
        {
            return await _orderProcessContext.BurgerKingOrder.ToListAsync();
        }

        public async Task Process(BurgerKingOrder model)
        {

            await _orderProcessContext.AddAsync(new BurgerKingOrder
            {
                OderId = model.OderId,
                OrderName = model.OrderName,
                IsMeal = model.IsMeal,
                Quantity = model.Quantity,
            }) ;

                await _orderProcessContext.SaveChangesAsync();
            
        }

    }
}
