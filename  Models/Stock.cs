using System;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class Stock
    {
        [Key]
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public double Price { get; set; }
        public int NumberAvailable { get; set; }
    }
}
