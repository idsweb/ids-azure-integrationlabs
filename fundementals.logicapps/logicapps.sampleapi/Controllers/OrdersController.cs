using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace logicapps.sampleapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("StockCheck")]
        public StockCount CheckStock(string productId)
        {
            
            string itemName = String.Format("Product {0}", productId);
            StockCount item = new StockCount(){ ItemId = Convert.ToInt32(productId), ItemName = itemName, Count = 0};
            
            if(Convert.ToInt32(productId) % 3 == 0){
                item.Count = 1;
            }

            return item;
        }
    }

    public class StockCount
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Count { get; set; }
    }
}
