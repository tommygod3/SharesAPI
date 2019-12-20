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
        public int Price { get; set; }
        public string Summary { get; set; }
    }
}
