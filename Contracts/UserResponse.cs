using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SharesAPI.Models;

namespace SharesAPI.Contracts
{
    public class UserResponse
    {
        public string Username { get; set; }
        public double Wallet { get; set; }
        public List<StockOwnership> StockOwned{ get; set; }

        public UserResponse(User user)
        {
            this.Username = user.Username;
            this.Wallet = user.Wallet;
            this.StockOwned = user.StockOwned;
        }
    }
}
