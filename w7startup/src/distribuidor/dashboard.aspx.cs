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

namespace global.distribuidor
{
    public partial class dashboard : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblNomeUsuario.Text = Session["nomeusuario"].ToString();
                gdvDados2.DataBind();

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from cliente where idtipocliente = 2 and cadastrado_por = '" + Session["idcliente"].ToString() +"'"))
                {
                    if (reader.Read())
                    {
                        lblQtdeLojista.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from cliente where idtipocliente = 3 and cadastrado_por = '" + Session["idcliente"].ToString() +"'"))
                {
                    if (reader.Read())
                    {
                        lblQtdeCliente.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from pedido where idconsumidor = '" + Session["idcliente"].ToString() +"'"))
                {
                    if (reader.Read())
                    {
                        lblQtdeVendas.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT sum(valor) as qtde from pedido where idconsumidor = '" + Session["idcliente"].ToString() +"'"))
                {
                    if (reader.Read())
                    {
                        lblQtdeFaturamento.Text = reader["qtde"].ToString();
                    }
                }
            }
        } 
   }
}