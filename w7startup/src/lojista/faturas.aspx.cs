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
using System.Data.Common;

namespace global.lojista
{
    public partial class faturas : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            //sdsDados.SelectCommand = "select * from pedido p join cliente c on c.id = p.idcliente where idlojista = '" + Session["idcliente"].ToString() +"' and (nomecompleto like '%" + txtBuscar.Text + "%' or nome_fantasia like '%" + txtBuscar.Text + "%' or cnpj_cpf like '%" + txtBuscar.Text + "%' or p.id like '" + txtBuscar.Text +"')";
            //gdvDados.DataBind();
        }
    }
}