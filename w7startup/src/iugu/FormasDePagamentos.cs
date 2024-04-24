using RestSharp;
using System.Configuration;

namespace global.iugu
{
    public class FormasDePagamentos
    {
        public static readonly string apiToken = ConfigurationManager.AppSettings["apitoken"];
        public static string BASEURRL = @"https://api.iugu.com/v1/plans?api_token=" + apiToken + "";

        public void CriarFormaPagamento()
        {
            //var options = new RestClientOptions("https://api.iugu.com/v1/customers/customer_id/payment_methods");
            //var client = new RestClient(options);
            //var request = new RestRequest("");
            //request.AddHeader("accept", "application/json");
            //request.AddHeader("content-type", "application/json");
            //var response = await client.PostAsync(request);

            //Console.WriteLine("{0}", response.Content);
        }
    }
}
