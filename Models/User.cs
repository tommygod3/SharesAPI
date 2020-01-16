using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SharesAPI.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public double Wallet { get; set; }
        public IEnumerable<StockOwnership> StockOwned{ get; set; }
    }
}
