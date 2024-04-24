using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace w7startup.src.iugu
{
    internal class formapagamento
    {
        public string description { get; set; }
        public string token { get; set; }
        public string set_as_default { get; set; }

        internal string toCreate()
        {
            var obj = new
            {
                this.description,
                this.token,
                this.set_as_default
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}