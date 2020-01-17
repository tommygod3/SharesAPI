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
using SharesAPI.ExternalAPI;
using SharesAPI.Contracts;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SellController : ControllerBase
    {
        
        private readonly IStockRepository _stockRepository;
        private readonly IUserRepository _userRepository;

        public SellController(IStockRepository stockRepository, IUserRepository userRepository)
        {
            _stockRepository = stockRepository;
            _userRepository = userRepository;
        }

        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpPost()]
        public IActionResult Create([FromBody] CreateTransactionRequest createSellRequest)
        {
            string username = Request.Headers["username"];
            string password = Request.Headers["password"];
            User user = _userRepository.GetUser(username);
            if (user == null) return NotFound($"No user exists with username: {username}");
            User authenticatedUser = _userRepository.AuthenticateUser(username, password);
            if (authenticatedUser == null) return Forbid("Username and password do not match");
            Stock stock = _stockRepository.GetStock(createSellRequest.Symbol);
            if (stock == null) return NotFound($"No stock exists with symbol: {createSellRequest.Symbol}");

            bool stockIsOwned = false;
            foreach (StockOwnership ownership in authenticatedUser.StockOwned)
            {
                if (createSellRequest.Symbol == ownership.Symbol)
                {
                    stockIsOwned = true;
                    if (ownership.AmountOwned < createSellRequest.Quantity) 
                        return BadRequest($"You cannot sell {createSellRequest.Quantity} of Stock {createSellRequest.Symbol} because you only own {ownership.AmountOwned}!");
                }
            }
            if (!stockIsOwned)
                return BadRequest($"You do not own any {createSellRequest.Symbol} Stocks");


            stock.NumberAvailable += createSellRequest.Quantity;
            Stock updatedStock = _stockRepository.Update(stock);

            User updatedUser = _userRepository.SellStock(user.Username, updatedStock, createSellRequest.Quantity);

            return Ok(updatedUser);
        }
    }
}
