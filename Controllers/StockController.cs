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

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetAsync(string symbol)
        {
            Stock stock = _stockRepository.GetStock(symbol);
            if (stock == null) return NotFound($"No stock exists with symbol: {symbol}");
            if ((DateTime.Now - stock.LastUpdated).TotalMinutes > 1) await _stockRepository.UpdateAllStockAsync();
            return Ok(_stockRepository.GetStock(symbol));
        }

        [HttpDelete("{symbol}")]
        public IActionResult Delete(string symbol)
        {
            return Ok(_stockRepository.Delete(symbol));
        }
    }
}
