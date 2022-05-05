#nullable disable
using BurgerKingOrderProcess.MicroService.Data;
using BurgerKingOrderProcess.MicroService.Models;
using Microsoft.AspNetCore.Mvc;

namespace BurgerKingOrderProcess.MicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BurgerKingOrdersController : ControllerBase
    {
        private readonly IProcessData _processData;

        public BurgerKingOrdersController(IProcessData processData)
        {
            _processData = processData;
        }

        /// <summary>
        /// BurgerKingOrders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BurgerKingOrder>>> GetBurgerKingOrder()
        {
            var result = await _processData.GetBurgerKingOrders();
            return Ok(result);
        }
       
    }
}
