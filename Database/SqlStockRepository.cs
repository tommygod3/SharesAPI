using SharesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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


        public Stock Update(Stock newStockItem)
        {
            Stock stockItem = GetStock(newStockItem.Symbol);
            if (stockItem != null)
            {
                stockItem.Name = newStockItem.Name;
                stockItem.LastUpdated = newStockItem.LastUpdated;
                stockItem.Price = newStockItem.Price;
                stockItem.Currency = newStockItem.Currency;
                stockItem.NumberAvailable = newStockItem.NumberAvailable;
                
                Context.SaveChanges();
            }
            return stockItem;
        }

        public Stock AddOrUpdateStock(Stock newStockItem)
        {
            Stock oldStockItem = GetStock(newStockItem.Symbol);
            if (oldStockItem == null)
            {
                newStockItem.LastUpdated = DateTime.Now;
                newStockItem.NumberAvailable = 10000;
                return Add(newStockItem);
            }
            else
            {
                newStockItem.LastUpdated = DateTime.Now;
                newStockItem.NumberAvailable = oldStockItem.NumberAvailable;
                return Update(newStockItem);
            }
        }

        public async System.Threading.Tasks.Task UpdateAllStockAsync()
        {
            string url = "https://api.worldtradingdata.com/api/v1/stock?symbol=EA,NTDOY,ATVI,SNE,MSFT&api_token=f3ISp1fZG9VtcgfmXIVCJP5TDqeJKXRunDEfvq22tHOPL3EVfoKHDYLBCOTt";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        ApiResponse content = response.Content.ReadAsAsync<ApiResponse>().Result;
                        foreach (Stock stock in content.Data)
                        {
                            AddOrUpdateStock(stock);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
