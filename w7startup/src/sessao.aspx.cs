using Microsoft.Practices.EnterpriseLibrary.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Threading.Tasks;
using pix_dynamic_payload_generator.net;
using pix_dynamic_payload_generator.net.Requests.RequestServices;
using System.Runtime.InteropServices;

namespace global
{
    public partial class sessao : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["idcliente"] = "";
                Session["idusuario"] = "";
                Session["email"] = "";
                Session["nomeusuario"] = "";
                Session.Clear();
            }
        }

       
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx", false);
        }
    }
}