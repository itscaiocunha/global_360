using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace global.iugu
{
    public class Planos
    {
        public static readonly string apiToken = ConfigurationManager.AppSettings["apitoken"];
        public static string BASEURRL = @"https://api.iugu.com/v1/plans?api_token=" + apiToken + "";

        public static Planos criar_Planos(Planos dadosplanos)
        {
            var client = new RestClient($"{BASEURRL}");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            dadosplanos.interval = 1;
            dadosplanos.interval_type = "months";
            dadosplanos.payable_with = "credit_card";
            features featdados = new features();
            featdados.name = "";
            featdados.identifier = "";
            featdados.value = 0;
            //dadosplanos.Infofeatures = featdados;
            dadosplanos.billing_days = 10;
            var env = dadosplanos.toCreate();
            request.AddParameter(
                "application/json",
                env,
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //if (response.StatusCode != HttpStatusCode.Created)
            //    throw new Exception($"{response.StatusCode} - {response.StatusDescription}");

            return JsonConvert.DeserializeObject<Planos>(response.Content);
        }

        public string idplano { get; set; }
        public string name { get; set; }
        public string identifier { get; set; }
        public int interval { get; set; }
        public string interval_type { get; set; }
        public int value_cents { get; set; }
        public string payable_with { get; set; }
        public int billing_days { get; set; }
        public List<features> Infofeatures { get; set; }

        internal string toCreate()
        {
            var obj = new
            {
                this.name,
                this.identifier,
                this.interval,
                this.interval_type,
                this.value_cents,
                this.payable_with,
                this.Infofeatures,
                this.billing_days
            };
            return JsonConvert.SerializeObject(obj);
        }

        public class features
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public string name { get; set; }

            public string identifier { get; set; }

            /// <summary>
            /// Dados do campo.
            /// </summary>
            public int value { get; set; }

        }
    }
}
