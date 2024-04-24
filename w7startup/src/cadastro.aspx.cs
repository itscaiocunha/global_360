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
using Newtonsoft.Json;
using System.Data.Common;
using global.iugu;

namespace global
{
    public partial class cadastro : System.Web.UI.Page
    {
        public static string BASEURRL = @"https://api.iugu.com/v1/customers?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {                 
                txtNomeFantasia.Focus();
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
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            if (ddlTipo.SelectedValue == "PJ")//pessoa juridica
            {
                if (!auth.VerificaCNPJ(txtCNPJ.Text))
                {
                    if (!auth.VerificaEmail(txtEmail.Text))
                    {
                        DbCommand command = db.GetSqlStringCommand(
                "INSERT INTO cliente (token, cnpj_cpf, rg, inscricao_estadual, razao_social, email, celular, nomecompleto, cep, endereco, bairro, numero, cidade, estado, complemento, idtipocliente, status, qtde_lojistas) values (@token, @cnpj_cpf, @rg, @inscricao_estadual, @razao_social, @email, @celular, @nomecompleto, @cep, @endereco, @bairro, @numero, @cidade, @estado, @complemento, @idtipocliente, @status, @qtde_lojistas)");
                        db.AddInParameter(command, "@token", DbType.String, Criptografia.Encrypt(auth.GeraTokenAleatorio()).Replace("+", "=").Replace("/", "="));
                        db.AddInParameter(command, "@cnpj_cpf", DbType.String, txtCNPJ.Text);
                        db.AddInParameter(command, "@rg", DbType.String, txtRG.Text);
                        db.AddInParameter(command, "@razao_social", DbType.String, txtRazaoSocial.Text);
                        db.AddInParameter(command, "@inscricao_estadual", DbType.String, txtIE.Text);
                        db.AddInParameter(command, "@email", DbType.String, txtEmail.Text);
                        db.AddInParameter(command, "@celular", DbType.String, txtCelular.Text);
                        db.AddInParameter(command, "@nomecompleto", DbType.String, txtNomeFantasia.Text);
                        db.AddInParameter(command, "@cep", DbType.String, txtCEP.Text);
                        db.AddInParameter(command, "@endereco", DbType.String, txtEndereco.Text);
                        db.AddInParameter(command, "@bairro", DbType.String, txtBairro.Text);
                        db.AddInParameter(command, "@numero", DbType.String, txtNum.Text);
                        db.AddInParameter(command, "@cidade", DbType.String, txtCidade.Text);
                        db.AddInParameter(command, "@estado", DbType.String, ddlUF.SelectedValue);
                        db.AddInParameter(command, "@complemento", DbType.String, txtComplemento.Text);
                        db.AddInParameter(command, "@idtipocliente", DbType.Int16, ddlTipoCliente.SelectedValue);
                        db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                        db.AddInParameter(command, "@qtde_lojistas", DbType.Int16, Convert.ToInt16(txtQtdeLojistas.Text));

                        try
                        {
                            db.ExecuteNonQuery(command);

                            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from cliente where cnpj_cpf = '" + txtCNPJ.Text + "'"))
                            {
                                if (reader.Read())
                                {
                                    string pw = txtSenha.Text;

                                    DbCommand command2 = db.GetSqlStringCommand(
                        "INSERT INTO usuario (idcliente, email, senha, status) values (@idcliente, @email, @senha, 'ATIVO')");
                                    db.AddInParameter(command2, "@idcliente", DbType.Int16, Convert.ToInt16(reader["id"].ToString()));
                                    db.AddInParameter(command2, "@email", DbType.String, txtEmail.Text);
                                    db.AddInParameter(command2, "@senha", DbType.String, Criptografia.Encrypt(pw).Replace("+", ""));
                                    try
                                    {
                                        db.ExecuteNonQuery(command2);

                                        using (IDataReader reader2 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from usuario where email = '" + txtEmail.Text + "'"))
                                        {
                                            if (reader2.Read())
                                            {
                                                DbCommand command3 = db.GetSqlStringCommand(
                        "INSERT INTO usuario_perfil (idusuario, idperfil) values (@idusuario, @idperfil)");
                                                db.AddInParameter(command3, "@idusuario", DbType.Int16, Convert.ToInt16(reader2["id"].ToString()));
                                                if(ddlTipoCliente.SelectedValue == "1")
                                                db.AddInParameter(command3, "@idperfil", DbType.Int16, 5);
                                                else if(ddlTipoCliente.SelectedValue == "2")
                                                        db.AddInParameter(command3, "@idperfil", DbType.Int16, 6);
                                                try
                                                {
                                                    db.ExecuteNonQuery(command3);

                                                    //aqui envia o email ao cliente cadastrado
                                                    // corpo do e-mail
                                                    string strHtml = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
                                                    strHtml = strHtml + "<title>Global 360 - Plataforma Digital</title></head><body><br>";
                                                    strHtml = strHtml + "<img src='https://global360.app.br/src/img/logo/logo_global.png' width='200' alt='Logo'>";
                                                    strHtml = strHtml + "<p><strong><font size='2' face='Verdana, Arial, Helvetica, sans-serif'>Novo Cadastro<br>Global 360 - Plataforma Digital</font></strong></p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Olá, tudo bem?</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Seu cadastro foi realizado com sucesso na plataforma.</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>CNPJ:</strong>" + txtCNPJ.Text + "</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>E-mail:</strong>" + txtEmail.Text + "</p><br><br>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Senha de acesso</strong><br>"+pw+"</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                                                    strHtml = strHtml + "</font><img src=''></body></html>";

                                                    //base teste
                                                    Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Novo Cadastro", strHtml, 1);
                                                    //base oficial
                                                    Email.emailTxt("contato@w7agencia.com.br", txtEmail.Text, "", "", "Global 360 - Novo Cadastro", strHtml, 1);

                                                    lblMensagem.Text = "Informação salva com sucesso!";
                                                    txtRazaoSocial.Text = "";
                                                    txtNomeCompleto.Text = "";
                                                    txtIE.Text = "";
                                                    txtCPF.Text = "";
                                                    txtEmail.Text = "";
                                                    txtCEP.Text = "";
                                                    txtEndereco.Text = "";
                                                    txtBairro.Text = "";
                                                    txtNum.Text = "";
                                                    txtCidade.Text = "";
                                                    txtRG.Text = "";
                                                    txtComplemento.Text = "";
                                                    txtCelular.Text = "";
                                                    txtQtdeLojistas.Text = "0";
                                                }
                                                catch (Exception ex)
                                                {
                                                    lblMensagem.Text = "Erro ao tentar salvar informação2. " + ex.Message;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lblMensagem.Text = "Erro ao tentar salvar informação3. " + ex.Message;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMensagem.Text = "Erro ao tentar salvar informação. " + ex.Message;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Não foi possível realizar o cadastro! Verifique o seu e-mail.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Não foi possível realizar o cadastro! Verifique o seu cnpj.";
                }
            }
            else
            {
                if (!auth.VerificaCNPJ(txtCPF.Text))
                {
                    if (!auth.VerificaEmail(txtEmail.Text))
                    {
                        //cria o cliente na iugu
                        Clientes dadoscliente = new Clientes();
                        dadoscliente.email = txtEmail.Text;
                        dadoscliente.name = txtNomeCompleto.Text;
                        dadoscliente.notes = "";
                        dadoscliente.phone = txtCelular.Text.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(" ", "").Substring(2, 9);
                        dadoscliente.phone_prefix = "0" + txtCelular.Text.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(" ", "").Substring(0, 2);
                        dadoscliente.cpf_cnpj = txtCPF.Text.Replace("-", "").Replace(".", "");
                        dadoscliente.cc_emails = txtEmail.Text;
                        dadoscliente.zip_code = txtCEP.Text.Replace("-", "");
                        dadoscliente.number = txtNum.Text;
                        dadoscliente.street = txtEndereco.Text;
                        dadoscliente.city = txtCidade.Text;
                        dadoscliente.state = "SP";
                        dadoscliente.district = txtBairro.Text;
                        dadoscliente.complement = "";

                        var client = new RestClient($"{BASEURRL}");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Accept", "application/json");
                        var env = dadoscliente.toCreate();
                        request.AddParameter(
                            "application/json",
                            env,
                            ParameterType.RequestBody);
                        try
                        {
                            IRestResponse response = client.Execute(request);
                            var dados = response.Content;
                            string idiugu = dados.Substring(7, 32);

                            DbCommand command = db.GetSqlStringCommand(
                   "INSERT INTO cliente (token, cnpj_cpf, inscricao_estadual, razao_social, email, celular, nomecompleto, cep, endereco, bairro, numero, cidade, estado, complemento, idtipocliente, status, idiugu, qtde_lojistas) values (@token, @cnpj_cpf, @inscricao_estadual, @razao_social, @email, @celular, @nomecompleto, @cep, @endereco, @bairro, @numero, @cidade, @estado, @complemento, @idtipocliente, @status, @idiugu, @qtde_lojistas)");
                            db.AddInParameter(command, "@token", DbType.String, Criptografia.Encrypt(auth.GeraTokenAleatorio()).Replace("+", "=").Replace("/", "="));
                            db.AddInParameter(command, "@cnpj_cpf", DbType.String, txtCPF.Text);
                            db.AddInParameter(command, "@razao_social", DbType.String, "");
                            db.AddInParameter(command, "@inscricao_estadual", DbType.String, "");
                            db.AddInParameter(command, "@email", DbType.String, txtEmail.Text);
                            db.AddInParameter(command, "@celular", DbType.String, txtCelular.Text);
                            db.AddInParameter(command, "@nomecompleto", DbType.String, txtNomeCompleto.Text);
                            db.AddInParameter(command, "@cep", DbType.String, txtCEP.Text);
                            db.AddInParameter(command, "@endereco", DbType.String, txtEndereco.Text);
                            db.AddInParameter(command, "@bairro", DbType.String, txtBairro.Text);
                            db.AddInParameter(command, "@numero", DbType.String, txtNum.Text);
                            db.AddInParameter(command, "@cidade", DbType.String, txtCidade.Text);
                            db.AddInParameter(command, "@estado", DbType.String, ddlUF.SelectedValue);
                            db.AddInParameter(command, "@complemento", DbType.String, txtComplemento.Text);
                            db.AddInParameter(command, "@idtipocliente", DbType.Int16, ddlTipoCliente.SelectedValue);
                            db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                            db.AddInParameter(command, "@idiugu", DbType.String, idiugu);
                            db.AddInParameter(command, "@qtde_lojistas", DbType.Int16, Convert.ToInt16(txtQtdeLojistas.Text));

                            db.ExecuteNonQuery(command);

                            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from cliente where cnpj_cpf = '" + txtCPF.Text + "'"))
                            {
                                if (reader.Read())
                                {
                                    string pw = txtSenha.Text;

                                    DbCommand command2 = db.GetSqlStringCommand(
                        "INSERT INTO usuario (idcliente, email, senha, status) values (@idcliente, @email, @senha, 'ATIVO')");
                                    db.AddInParameter(command2, "@idcliente", DbType.Int16, Convert.ToInt16(reader["id"].ToString()));
                                    db.AddInParameter(command2, "@email", DbType.String, txtEmail.Text);
                                    db.AddInParameter(command2, "@senha", DbType.String, Criptografia.Encrypt(pw));
                                    try
                                    {
                                        db.ExecuteNonQuery(command2);

                                        using (IDataReader reader2 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from usuario where email = '" + txtEmail.Text + "'"))
                                        {
                                            if (reader2.Read())
                                            {
                                                DbCommand command3 = db.GetSqlStringCommand(
                        "INSERT INTO usuario_perfil (idusuario, idperfil) values (@idusuario, @idperfil)");
                                                db.AddInParameter(command3, "@idusuario", DbType.Int16, Convert.ToInt16(reader2["id"].ToString()));
                                                db.AddInParameter(command3, "@idperfil", DbType.Int16, 7);
                                                try
                                                {
                                                    db.ExecuteNonQuery(command3);

                                                    //aqui envia o email ao cliente cadastrado
                                                    // corpo do e-mail
                                                    string strHtml = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
                                                    strHtml = strHtml + "<title>Global 360 - Plataforma Digital</title></head><body><br>";
                                                    strHtml = strHtml + "<img src='https://global360.app.br/src/img/logo/logo_global.png' width='200' alt='Logo'>";
                                                    strHtml = strHtml + "<p><strong><font size='2' face='Verdana, Arial, Helvetica, sans-serif'>Novo Cadastro<br>Global 360 - Plataforma Digital</font></strong></p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Olá, tudo bem?</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Seu cadastro foi realizado com sucesso na plataforma.</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>CPF:</strong>" + txtCPF.Text + "</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>E-mail:</strong>" + txtEmail.Text + "</p><br><br>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Senha de acesso</strong><br>" + pw + "</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                                                    strHtml = strHtml + "</font><img src=''></body></html>";

                                                    //base teste
                                                    Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Novo Cadastro", strHtml, 1);
                                                    //base oficial
                                                    Email.emailTxt("contato@w7agencia.com.br", txtEmail.Text, "", "", "Global 360 - Novo Cadastro", strHtml, 1);

                                                    lblMensagem.Text = "Seu cadastro foi realizado com sucesso!";
                                                    txtNomeCompleto.Text = "";
                                                    txtCPF.Text = "";
                                                    txtEmail.Text = "";
                                                    txtCEP.Text = "";
                                                    txtEndereco.Text = "";
                                                    txtRG.Text = "";
                                                    txtBairro.Text = "";
                                                    txtNum.Text = "";
                                                    txtCidade.Text = "";
                                                    txtComplemento.Text = "";
                                                    txtCelular.Text = "";
                                                    txtQtdeLojistas.Text = "0";
                                                }
                                                catch (Exception ex)
                                                {
                                                    lblMensagem.Text = "Erro ao tentar salvar informação2. " + ex.Message;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lblMensagem.Text = "Erro ao tentar salvar informação3. " + ex.Message;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMensagem.Text = "Erro ao tentar salvar informação. " + ex.Message;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Não foi possível realizar o cadastro! Verifique o seu e-mail.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Não foi possível realizar o cadastro! Verifique o seu cpf.";
                }
            }
        }

        protected void txtCEP_TextChanged(object sender, EventArgs e)
        {
            if (txtCEP.Text.Length > 8)
            {
                string cepnovo = txtCEP.Text.Replace("-", "");
                var info = cep.HttpPost("http://viacep.com.br/ws/" + cepnovo + "/json/");
                dynamic dados = JsonConvert.DeserializeObject<dynamic>(info);
                var end = dados["logradouro"];
                txtEndereco.Text = end.ToString();
                var bairro = dados["bairro"];
                txtBairro.Text = bairro.ToString();
                var cidade = dados["localidade"];
                txtCidade.Text = cidade.ToString();
                var uf = dados["uf"];
                ddlUF.SelectedValue = uf.ToString();
                txtNum.Focus();

            }
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipo.SelectedValue == "PJ")
            {
                pnlPF.Visible = false;
                pnlPJ.Visible = true;
                txtNomeFantasia.Focus();
            }
            else
            {
                pnlPF.Visible = true;
                pnlPJ.Visible = false;
                txtNomeCompleto.Focus();
            }
        }
        protected void lkbJatenhoconta_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx", false);
        }
    }
}