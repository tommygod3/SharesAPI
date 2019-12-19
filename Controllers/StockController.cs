using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace shares_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("/{id}")]
        public IEnumerable<Stock> Get(string id)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Stock
            {
                datetime = DateTime.Now.AddDays(index),
                price = rng.Next(0, 10000000),
                name = $"Some cool stock{id}",
                summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public IEnumerable<Stock> GetAll()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Stock
            {
                datetime = DateTime.Now.AddDays(index),
                price = rng.Next(0, 10000000),
                name = $"Some cool stock{rng.Next(0, 10000000)}",
                summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
