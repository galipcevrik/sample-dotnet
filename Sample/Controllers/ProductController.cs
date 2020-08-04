using System;
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
        [ProducesResponseType(typeof(ProductPrice), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PriceConvert([FromBody]ProductPrice product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Merkez banka gunluk kur degerlerini cekiyoruz.
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);

            //Merkez bankasi uzerindeki guncel dolar ve euro kur degerlerini aliyoruz.
            var usd = Convert.ToDouble(xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='USD'] / BanknoteSelling").InnerXml?.Replace('.', ','));
            var euro = Convert.ToDouble(xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='EUR'] / BanknoteSelling").InnerXml?.Replace('.', ','));

            var data = new ProductPrice();

            //Parametre olarak gelen para tipini kontrol ediyoruz ve sabit kur gonderilmemis ise merkez bankasi kurlarini setliyoruz.
            if (product.Currency.ToLower() == "dollar (usa)")
                data.RateOfExchange = product.RateOfExchange <= 0 ? usd : product.RateOfExchange;

            if (product.Currency.ToLower() == "euro")
                data.RateOfExchange = product.RateOfExchange <= 0 ? euro : product.RateOfExchange;

            //Kur degisim islemini yaparak donecegimiz degere setliyoruz.
            data.Price = (Convert.ToDouble(product.Price.Replace('.', ',')) * data.RateOfExchange).ToString();
            data.Currency = "Turkish Liras";

            return Ok(data);
        }
    }
}