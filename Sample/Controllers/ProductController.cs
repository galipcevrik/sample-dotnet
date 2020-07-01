using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Sample.Models;

namespace Sample.Controllers
{
    [Route("api/product")]
    public class ProductController : BaseController
    {
        [HttpPost, Route("price_convert")]
        [ProducesResponseType(typeof(ProductPrice), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PriceConvert([FromBody]ProductPrice product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = new ProductPrice();

            if (product.Currency.ToLower() == "dollar (usa)")
                data.RateOfExchange = product.RateOfExchange <= 0 ? 6.80 : product.RateOfExchange;


            if (product.Currency.ToLower() == "euro")
                data.RateOfExchange = product.RateOfExchange <= 0 ? 6.80 : product.RateOfExchange;

            data.Price = product.Price * data.RateOfExchange;
            product.Currency = "Turkish Liras";

            return Ok(data);
        }
    }
}