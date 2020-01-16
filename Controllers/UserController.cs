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
using SharesAPI.Requests;

namespace SharesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [HttpGet()]
        public IActionResult Get()
        {
            string username = Request.Headers["username"];
            string password = Request.Headers["password"];
            User user = _userRepository.GetUser(username);
            if (user == null) return NotFound($"No user exists with username: {username}");
            User authenticatedUser = _userRepository.AuthenticateUser(username, password);
            if (authenticatedUser == null) return Forbid("Username and password do not match");
            return Ok(authenticatedUser);
        }

        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost()]
        public IActionResult Create([FromBody] CreateUserRequest user)
        {
            User createdUser = _userRepository.Add(user);
            if (createdUser == null) return BadRequest("Bad request");
            return Ok(createdUser);
        }
    }
}
