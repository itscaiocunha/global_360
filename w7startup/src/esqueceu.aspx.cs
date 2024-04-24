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

namespace global
{
    public partial class esqueceu : System.Web.UI.Page
    {
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

        /// <summary>
        /// aqui é verificado se email e senha são válidos e direciona para o perfil cadastrado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * from cliente where email = '" + txtEmail.Text + "'"))
            {
                if (reader.Read())
                {
                    string newpw = auth.GeraTokenAleatorio();
                    Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                    try
                    {
                        DbCommand command = db.GetSqlStringCommand(
                        "UPDATE usuario SET senha = @senha where id = @id");
                        db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(reader["id"].ToString()));
                        db.AddInParameter(command, "@senha", DbType.String, Criptografia.Encrypt(newpw).Replace("+", "="));

                        db.ExecuteNonQuery(command);

                        lblMensagem.Text = "Processo realizado com sucesso. Acesse sua caixa de e-mail para confirmar!";

                        //envia o email ao usuario gerando uma nova senha 
                        // corpo do e-mail
                        string strHtml = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
                        strHtml = strHtml + "<title>Global 360 - Plataforma Digital</title></head><body><br>";
                        strHtml = strHtml + "<img src='https://global360.app.br/src/img/logo/logo_global.png' width='200' alt='Logo'>";
                        strHtml = strHtml + "<p><strong><font size='2' face='Verdana, Arial, Helvetica, sans-serif'>Esqueceu a Senha<br>Global 360 - Plataforma Digital</font></strong></p>";
                        strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Olá, tudo bem?</p>";
                        strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Você solicitou alteração de senha. Acesse o sistema com esta senha gerada.</p>";
                        strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Nova senha</strong><br>"+newpw+"</p>";
                        strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                        strHtml = strHtml + "</font><img src=''></body></html>";

                        //base teste
                        //Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Esqueceu a senha", strHtml, 1);
                        //base oficial
                        Email.emailTxt("contato@w7agencia.com.br", reader["email"].ToString(), "", "", "Global 360 - Esqueceu a senha", strHtml, 1);

                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro ao tentar confirmar. " + ex.Message;
                    }                   
                }
                else
                    lblMensagem.Text = "E-mail não encontrado. Tente novamente!";
            }
        }

        protected void lkbJasou_Click(object sender, EventArgs e)
        {
            Response.Redirect("cadastro.aspx", false);
        }

        protected void lkbEsqueceuSenha_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx", false);
        }
    }
}