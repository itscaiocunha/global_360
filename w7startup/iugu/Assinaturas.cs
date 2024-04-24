using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using RestSharp;
using System.Configuration;

namespace global.iugu
{
    public class Assinaturas
    {
        public static readonly string apiToken = ConfigurationManager.AppSettings["apitoken"];
        public static string BASEURRL = @"https://api.iugu.com/v1/subscriptions?api_token=" + apiToken + "";

        public static Assinaturas criar_Assinaturas(Clientes dadoscliente, Planos dadosplanos)
        {
            var client = new RestClient($"{BASEURRL}");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            Assinaturas dadosAssina = new Assinaturas();
            dadosAssina.plan_identifier = dadosplanos.name;
            dadosAssina.customer_id = dadoscliente.id;
            dadosAssina.expires_at = null;
            dadosAssina.only_on_charge_success = null;
            dadosAssina.ignore_due_email = null;
            dadosAssina.payable_with = "credit_card";
            dadosAssina.credits_based = false;
            dadosAssina.price_cents = dadosplanos.value_cents;
            dadosAssina.credits_cycle = null;
            dadosAssina.credits_min = 10;
            subitems ListaItems = new subitems();
            ListaItems.description = "Mensalidade Gerente Fácil";
            ListaItems.price_cents = dadosplanos.value_cents;
            ListaItems.quantity = 1;
            ListaItems.recurrent = true;            
            dadosAssina.listItems = ListaItems;            
            dadosAssina.two_step = false;
            dadosAssina.suspend_on_invoice_expired = true;
            var env = dadosAssina.toCreate();
            request.AddParameter(
                "application/json",
                env,
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            //if (response.StatusCode != HttpStatusCode.Created)
            //    throw new Exception($"{response.StatusCode} - {response.StatusDescription}");

            return JsonConvert.DeserializeObject<Assinaturas>(response.Content);
        }

        public string plan_identifier { get; set; }
        public string customer_id { get; set; }
        public string expires_at { get; set; }
        public string only_on_charge_success { get; set; }
        public string ignore_due_email { get; set; }
        public string payable_with { get; set; }
        public bool credits_based { get; set; }
        public int price_cents { get; set; }
        public string credits_cycle { get; set; }
        public subitems listItems { get; set; }
        public int credits_min { get; set; }
        public bool two_step { get; set; }
        public bool suspend_on_invoice_expired { get; set; }

        internal string toCreate()
        {
            var obj = new
            {
                this.plan_identifier,
                this.customer_id,
                this.expires_at,
                this.only_on_charge_success,
                this.ignore_due_email,
                this.payable_with,
                this.credits_based,
                this.price_cents,
                this.credits_cycle,
                this.listItems,
                this.credits_min,
                this.two_step,
                this.suspend_on_invoice_expired
            };
            return JsonConvert.SerializeObject(obj);
        }

        public class subitems
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public string description { get; set; }

            public bool recurrent { get; set; }

            /// <summary>
            /// Dados do campo.
            /// </summary>
            public int price_cents { get; set; }
            public int quantity { get; set; }
        }
    }
}
