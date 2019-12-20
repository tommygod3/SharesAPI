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
            //return Context.Employees; This returns the Stock within the Employee class as null
            return Context.Stocks;
            //^Example of eager loading. This will ensure the Stock is populated
            //.Include() can be chained to eager load multiple objects.
        }

        public Stock GetStock(string symbol)
        {
            return Context.Stocks.FirstOrDefault(s => s.Symbol == symbol);
            //Include Stocks, Filter using LINQ on symbol, get first
        }


        public Stock Update(Stock StockChanges)
        {
            Context.Stocks.Update(StockChanges);
            Context.SaveChanges();
            return StockChanges;
        }
    }
}
