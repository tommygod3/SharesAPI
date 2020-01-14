using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharesAPI.Models
{
    public class ApiResponse
    {
        public string symbols_requested { get; set; }
        public string symbols_returned { get; set; }
        public List<Stock> Data { get; set; }

    }
}
