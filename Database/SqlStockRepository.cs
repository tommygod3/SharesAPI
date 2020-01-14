using SharesAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SharesAPI.DatabaseAccess
{
    public class SqlStockRepositroy : IStockRepository
    {
        private AppDbContext Context { get; }

        public SqlStockRepositroy(AppDbContext context)
        {
            Context = context;
        }

        public Stock Add(Stock Stock)
        {
            Context.Stocks.Add(Stock);
            Context.SaveChanges();
            return Stock;
        }

        public Stock Delete(string symbol)
        {
            Stock Stock = Context.Stocks.Find(symbol);
            if (Stock != null)
            {
                Context.Stocks.Remove(Stock);
                Context.SaveChanges();
            }
            return Stock;
        }

        public IEnumerable<Stock> GetStocks()
        {
            return Context.Stocks;
        }

        public Stock GetStock(string symbol)
        {
            return Context.Stocks.FirstOrDefault(s => s.Symbol == symbol);
        }


        public Stock Update(Stock StockChanges)
        {
            Context.Stocks.Update(StockChanges);
            Context.SaveChanges();
            return StockChanges;
        }
    }
}
