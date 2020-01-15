using System;
using System.Net.Http;

namespace SharesAPI.Currency
{
    public class CurrencyAPI
    {
        public double rate { get; set; }
        public static async System.Threading.Tasks.Task<double?> ConvertAsync(string currency, double value)
        {
            string url = $"http://127.0.0.1:8000/currency/{currency}/?format=json&source=USD";
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
                        System.Console.WriteLine(content.rate);
                        return content.rate * value;
                    }
                    else return null;
                }
                catch (System.Net.Http.HttpRequestException e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }
    }
}
