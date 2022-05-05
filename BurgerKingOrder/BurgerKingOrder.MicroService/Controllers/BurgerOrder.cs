namespace BurgerKingOrder.MicroService.Controllers
{
    public class BurgerOrder
    {
        public int OderId { get; set; }
        public string? OrderName { get; set; }
        public bool IsMeal { get; set; }
        public int Quantity { get; set; }

    }
}