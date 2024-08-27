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
using global.iugu;
using System.Data.Common;
using Newtonsoft.Json;
using static global.iugu.token;
using iugu.net;
using iugu.net.Entity;
using iugu.net.Lib;
using iugu.net.Request;
using iugu.net.Response;
using w7startup;
using w7startup.src.iugu;
using Azure.Core;
using static global.iugu.Assinaturas;

namespace global
{
    public partial class login : System.Web.UI.Page
    {
        public static string BASEURLFATURA = @"https://api.iugu.com/v1/invoices?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";
        public static string BASEURRLASSINATURA = @"https://api.iugu.com/v1/subscriptions?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";

        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtEmail.Focus();
                Session["idcliente"] = "";
                Session["idusuario"] = "";
                Session["email"] = "";
                Session["nomeusuario"] = "";
                Session.Clear();
            }
        }

        public string Json()
        {
            string dadosJson = "{    \"plan_identifier\": \"premium_plan\",    \"customer_id\": \"F287ED258915473FB41B50C6CBEEDCF5\",        \"expires_at\": null,    \"only_on_charge_success\": null,    \"ignore_due_email\": null,    \"payable_with\": \"all\",    \"credits_based\": false,    \"price_cents\": 7990,    \"credits_cycle\": null,    \"credits_min\": 0,    \"subitems\": [{        \"description\": \"Item um\",        \"price_cents\": 7990,        \"quantity\": 1,        \"recurrent\": true    }],    \"custom_variables\":[],    \"two_step\": false,    \"suspend_on_invoice_expired\": true,    \"only_charge_on_due_date\": false}";
            return dadosJson;
        }

        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            string senha = Criptografia.Encrypt(txtSenha.Text).Replace("+", "");

            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT u.id as idusuario, c.nomecompleto, u.email as emailusuario, u.idcliente, u.senha, u.nomeusuario, up.idperfil, c.idiugu from usuario u join usuario_perfil up on up.idusuario = u.id join cliente c on c.id = u.idcliente where c.status = 'ATIVO' and u.email = '" + txtEmail.Text +"' and senha = '" + senha + "'"))
            {
                if (reader.Read())
                {
                    Session["idcliente"] = reader["idcliente"].ToString();
                    Session["idusuario"] = reader["idusuario"].ToString();
                    Session["email"] = reader["emailusuario"].ToString();
                    Session["nomeusuario"] = reader["nomecompleto"].ToString();

                    if (reader["idperfil"].ToString() == "4")
                        Response.Redirect("admin/dashboard.aspx", false);
                    else if (reader["idperfil"].ToString() == "5")
                        Response.Redirect("distribuidor/dashboard.aspx", false);
                    else if (reader["idperfil"].ToString() == "6")
                        Response.Redirect("lojista/dashboard.aspx", false);
                    else if (reader["idperfil"].ToString() == "7")
                        Response.Redirect("cliente/vendas.aspx", false);
                    else
                        lblMensagem.Text = "Usuário não liberado. Tente novamente!";
                }
                else
                    lblMensagem.Text = "E-mail ou senha incorretos. Tente novamente!" + senha;
            }
        }

        protected void lkbJasou_Click(object sender, EventArgs e)
        {
            Response.Redirect("cadastro.aspx", false);
        }

        protected void lkbEsqueceuSenha_Click(object sender, EventArgs e)
        {
            Response.Redirect("esqueceu.aspx", false);
        }
    }
}