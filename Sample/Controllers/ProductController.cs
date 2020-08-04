﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PriceConvert([FromBody]ProductPrice product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);

            var usd = double.Parse(xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='USD'] / BanknoteSelling").InnerXml?.Replace('.', ','));
            var euro = Convert.ToDouble(xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='EUR'] / BanknoteSelling").InnerXml?.Replace('.', ','));

            var data = new ProductPrice();

            if (product.Currency.ToLower() == "dollar (usa)")
                data.RateOfExchange = product.RateOfExchange <= 0 ? usd : product.RateOfExchange;


            if (product.Currency.ToLower() == "euro")
                data.RateOfExchange = product.RateOfExchange <= 0 ? euro : product.RateOfExchange;

            data.Price = product.Price * data.RateOfExchange;
            //data.Currency = "Turkish Liras";

            return Ok(data.Price.ToString().Replace(',', '.'));
        }
    }
}