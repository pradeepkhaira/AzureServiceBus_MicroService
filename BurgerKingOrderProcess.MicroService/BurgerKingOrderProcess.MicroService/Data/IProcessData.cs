using BurgerKingOrderProcess.MicroService.Models;

namespace BurgerKingOrderProcess.MicroService.Data
{
    public interface IProcessData
    {
        Task Process(BurgerKingOrder model);
        Task<IEnumerable<BurgerKingOrder>> GetBurgerKingOrders();
    }
}
