using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharesAPI.Contracts
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

