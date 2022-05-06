using BurgerKingOrderProcess.MicroService.Models;

namespace BurgerKingOrderProcess.MicroService.Data
{
    public interface IProcessData
    {
        Task Process(BurgerOrder model);
        Task<IEnumerable<BurgerKingOrder>> GetBurgerKingOrders();
    }
}
