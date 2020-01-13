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
        private readonly string url = "https://api.worldtradingdata.com/api/v1/stock?symbol=EA,NTDOY,ATVI,SNE,MSFT&api_token=f3ISp1fZG9VtcgfmXIVCJP5TDqeJKXRunDEfvq22tHOPL3EVfoKHDYLBCOTt";
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        [HttpGet("{symbol}")]
        public Stock Get(string symbol)
        {
            // Update symbol stock
            return _stockRepository.GetStock(symbol);
        }

        [HttpGet]
        public IEnumerable<Stock> GetAll()
        {
            // Update all stock
            return _stockRepository.GetStocks();
        }

        [HttpDelete("{symbol}")]
        public void Delete(string symbol)
        {
            _stockRepository.Delete(symbol);
        }
    }
}
