using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharesAPI.Models;
using SharesAPI.DatabaseAccess;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet("{symbol}")]
        public IEnumerable<Stock> Get(string symbol)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Stock
            {
                Symbol = symbol,
                LastUpdated = DateTime.Now.AddDays(index),
                Price = rng.Next(0, 10000000),
                Name = $"Some cool stock {symbol}",
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public IEnumerable<Stock> GetAll()
        {
            return _stockRepository.GetStocks();
        }
    }
}
