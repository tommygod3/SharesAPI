using SharesAPI.Models;
using System.Collections.Generic;

namespace SharesAPI.DatabaseAccess
{
    public interface IStockRepository
    {
        Stock GetStock(string symbol);
        IEnumerable<Stock> GetStocks();
        Stock Add(Stock stock);
        Stock Update(Stock stockChanges);
        Stock Delete(string symbol);
        Stock AddOrUpdateStock(Stock newStockItem);
        System.Threading.Tasks.Task UpdateAllStockAsync();
    }
}
