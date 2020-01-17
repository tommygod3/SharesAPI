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
    public class PurchaseController : ControllerBase
    {
        
        private readonly IStockRepository _stockRepository;
        private readonly IUserRepository _userRepository;

        public PurchaseController(IStockRepository stockRepository, IUserRepository userRepository)
        {
            _stockRepository = stockRepository;
            _userRepository = userRepository;
        }

        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpPost()]
        public IActionResult Create([FromBody] CreateTransactionRequest createPurchaseRequest)
        {
            string username = Request.Headers["username"];
            string password = Request.Headers["password"];
            User user = _userRepository.GetUser(username);
            if (user == null) return NotFound($"No user exists with username: {username}");
            User authenticatedUser = _userRepository.AuthenticateUser(username, password);
            if (authenticatedUser == null) return Forbid("Username and password do not match");
            Stock stock = _stockRepository.GetStock(createPurchaseRequest.Symbol);
            if (stock == null) return NotFound($"No stock exists with symbol: {createPurchaseRequest.Symbol}");

            if (stock.NumberAvailable - createPurchaseRequest.Quantity < 0) 
                return BadRequest($"We only have {stock.NumberAvailable} remaining of Stock {createPurchaseRequest.Symbol}!");

            stock.NumberAvailable -= createPurchaseRequest.Quantity;
            Stock updatedStock = _stockRepository.Update(stock);

            User updatedUser = _userRepository.PurchaseStock(user.Username, updatedStock, createPurchaseRequest.Quantity);
            if (updatedStock == null) 
                return BadRequest($"You do not have enough funds to complete this purchase");

            return Ok(updatedUser);
        }
    }
}
