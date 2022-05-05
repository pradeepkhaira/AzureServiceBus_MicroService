namespace BurgerKingOrderProcess.MicroService.Models
{
    public class BurgerKingOrder
    {
        public int Id { get; set; }
        public int OderId { get; set; }
        public string? OrderName { get; set; }
        public bool IsMeal { get; set; }
        public int Quantity { get; set; }
    }
    
}
