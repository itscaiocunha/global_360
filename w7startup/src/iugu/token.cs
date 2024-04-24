using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static global.iugu.Assinaturas;

namespace global.iugu
{
    public class token
    {
        //const string apitoken = "AD99A0D399716033D451951780EEA5EEB2F3CA5990D109E97C7CDBDFBA88EFB4";
        const string apitoken = "A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";
        public string account_id { get; set; }
        public string method { get; set; }
        public string test { get; set; }
        
        public subitemstk data { get; set; }
        
        public class subitemstk
        {
            /// <summary>
            /// Nome do campo.
            /// </summary>
            public string number { get; set; }

            public string verification_value { get; set; }

            public string first_name { get; set; }
            public string last_name { get; set; }
            public string month { get; set; }
            public string year { get; set; }
        }

        internal string toCreate()
        {
            var obj = new
            {
                this.account_id,
                this.method,
                this.test,
                this.data
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}