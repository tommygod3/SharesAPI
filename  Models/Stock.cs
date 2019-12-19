using System;

namespace SharesAPI.Models
{
    public class Stock
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Price { get; set; }
        public string Summary { get; set; }
    }
}
