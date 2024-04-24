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
    public partial class dashboard : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblNomeUsuario.Text = Session["nomeusuario"].ToString();
                }
                catch
                {
                    Response.Redirect("../sessao.aspx", false);
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from cliente where idtipocliente = 1"))
                {
                    if (reader.Read())
                    {
                        lblQtdeDistribuidor.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from cliente where idtipocliente = 2"))
                {
                    if (reader.Read())
                    {
                        lblQtdeLojista.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from cliente where idtipocliente = 3"))
                {
                    if (reader.Read())
                    {
                        lblQtdeCliente.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from produto"))
                {
                    if (reader.Read())
                    {
                        lblQtdeProdutos.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT count(*) as qtde from pedido where idlojista is not null"))
                {
                    if (reader.Read())
                    {
                        lblQtdeVendas.Text = reader["qtde"].ToString();
                    }
                }

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT sum(valor) as qtde from pedido where idlojista is not null"))
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