using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharesAPI.Models;

namespace SharesAPI.ExternalAPI
{
    public class StockAPI
    {
        public string symbols_requested { get; set; }
        public string symbols_returned { get; set; }
        public List<Stock> Data { get; set; }

    }
}
