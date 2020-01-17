using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharesAPI.Contracts
{
    public class CreateTransactionRequest
    {
        public int Quantity { get; set; }
        public string Symbol { get; set; }
    }
}
