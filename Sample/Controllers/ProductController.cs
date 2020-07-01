using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Sample.Controllers
{
    [Route("api/product")]
    public class ProductController : BaseController
    {
        [HttpPost, Route("price_convert")]
        [ProducesResponseType(typeof(JObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PriceConvert([FromBody]string currency, [FromBody] int price)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = new JObject();

            if (currency.ToLower() == "dollar")
            {
                data["rate"] = 6.80;
                data["price"] = price * 6.80;
            }
            
            if (currency.ToLower() == "euro")
            {
                data["rate"] = 7.80;
                data["price"] = price * 7.80;
            }

            return Ok(data);
        }
    }
}