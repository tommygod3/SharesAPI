using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharesAPI.Models;
using SharesAPI.DatabaseAccess;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [ProducesResponseType(typeof(List<Stock>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            bool outOfDate = false;
            IEnumerable<Stock> allStock = _stockRepository.GetStocks();
            foreach (Stock stock in allStock)
            {
                if ((DateTime.Now - stock.LastUpdated).TotalMinutes > 10)
                {
                    outOfDate = true;
                    break;
                }
            }
            if (outOfDate || allStock.Count() == 0) await _stockRepository.UpdateAllStockAsync();
            return Ok(_stockRepository.GetStocks());
        }

        [ProducesResponseType(typeof(Stock), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetAsync(string symbol)
        {
            Stock stock = _stockRepository.GetStock(symbol);
            if (stock == null) return NotFound($"No stock exists with symbol: {symbol}");
            if ((DateTime.Now - stock.LastUpdated).TotalMinutes > 1) await _stockRepository.UpdateAllStockAsync();
            return Ok(_stockRepository.GetStock(symbol));
        }

        [ProducesResponseType(typeof(Stock), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpDelete("{symbol}")]
        public IActionResult Delete(string symbol)
        {
            Stock deleted = _stockRepository.Delete(symbol);
            if (deleted == null) return NotFound($"No stock exists with symbol: {symbol}");
            else return Ok(deleted);
        }
    }
}
