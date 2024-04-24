using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace w7startup.src.iugu
{
    internal class fatura
    {
        public string email { get; set; }
        public string cc_emails { get; set; }
        public string due_date { get; set; }
        public bool ensure_workday_due_date { get; set; }
        public string bank_slip_extra_due { get; set; }
        public string return_url { get; set; }
        public string expired_url { get; set; }
        public string notification_url { get; set; }
        public bool ignore_canceled_email { get; set; }
        public List<items> listItems { get; set; }
        public bool fines { get; set; }
        public string late_payment_fine { get; set; }
        public string suspend_on_invoice_expired { get; set; }
        public bool per_day_interest { get; set; }
        public string per_day_interest_value { get; set; }
        public string discount_cents { get; set; }
        public string customer_id { get; set; }
        public bool ignore_due_email { get; set; }
        public string subscription_id { get; set; }
        public string payable_with { get; set; }
        public int credits { get; set; }
        public bool early_payment_discount { get; set; }
        public string early_payment_discounts { get; set; }
        public string order_id { get; set; }
        public splits Listsplits { get; set; }
        public custom_variables Listcustom_variables { get; set; }
        public payer Listpayer { get; set; }
        public payer.address ListAddress { get; set; }
       
        internal string toCreate()
        {
            var obj = new
            {
                this.email,
                this.cc_emails,
                this.due_date,
                this.ensure_workday_due_date,
                this.bank_slip_extra_due,
                this.listItems,
                this.return_url,
                this.expired_url,
                this.notification_url,                
                this.ignore_canceled_email,
                this.fines,
                this.late_payment_fine,
                this.per_day_interest,
                this.per_day_interest_value,
                this.discount_cents,
                this.customer_id,
                this.ignore_due_email,
                this.subscription_id,
                this.payable_with,
                this.credits,
                this.Listcustom_variables,
                this.early_payment_discount,                
                this.Listpayer,
                this.ListAddress,
                this.order_id,
                this.Listsplits
            };
            return JsonConvert.SerializeObject(obj);
        }

        public class items
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public string description { get; set; }
            public int quantity { get; set; }
            public int price_cents { get; set; }
        }

        public class custom_variables
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public string name { get; set; }
            public int value { get; set; }
        }

        public class splits
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public int cents { get; set; }
            public int percent { get; set; }
            public int credit_card_cents { get; set; }
            public int credit_card_percent { get; set; }
            public int bank_slip_cents { get; set; }
            public int bank_slip_percent { get; set; }
        }

        public class payer
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public string cpf_cnpj { get; set; }
            public string name { get; set; }
            public string phone_prefix { get; set; }
            public string phone { get; set; }
            public string email { get; set; }

            public class address
            {
                public string zip_code { get; set; }
                public string street { get; set; }
                public string number { get; set; }
                public string district { get; set; }
                public string city { get; set; }
                public string state { get; set; }
                public string country { get; set; }
                public string complement { get; set; }

            }
        }
    }
}