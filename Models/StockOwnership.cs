using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharesAPI.Models
{
    public class StockOwnership
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Symbol")]
        public string StockName{ get; set; }
        public string AmountOwned{ get; set; }
        public StockOwnership()
        {
            Id = Guid.NewGuid();
        }
    }
}
