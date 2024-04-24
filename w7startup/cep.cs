using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace global
{
    public class cep
    {
        public static string HttpPost(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //byte[] data = Encoding.ASCII.GetBytes();
            //request.Headers["authorization"] = "basic: " + token;
            request.Method = "GET";
            //request.Accept = "application/json"; //?
            request.ContentType = "../json; charset=utf-8"; //?
                                                         //request.ContentLength = data.Length;

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }
    }
}