using System;
using System.Net.Http;

namespace SharesAPI.Currency
{
    public class CurrencyAPI
    {
        public double rate { get; set; }
        public static async System.Threading.Tasks.Task<double?> ConvertAsync(string fromCurrency, string toCurrency, double value)
        {
            string url = $"http://127.0.0.1:8000/currency/{toCurrency}/?format=json&source={fromCurrency}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        CurrencyAPI content = response.Content.ReadAsAsync<CurrencyAPI>().Result;
                        return content.rate * value;
                    }
                    else return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
