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
using Newtonsoft.Json;
using System.Runtime.ConstrainedExecution;
using global.iugu;

namespace global.lojista
{
    partial class clientes : System.Web.UI.Page
    {
        public static string BASEURRL = @"https://api.iugu.com/v1/customers?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";

        public void Page_Load(object sender, EventArgs e)
        {
            hdfIdUsuario.Value = Session["idcliente"].ToString();
            //traz o conteudo do contrato para salvar no cadastro final
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "SELECT * from contrato where idtipocliente = '3'"))
            {
                if (reader.Read())
                {
                    hdfContrato.Value = reader["conteudo"].ToString();
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = true;
            txtNomeCliente.Text = "";
            txtCPFCNPJ.Text = "";
            txtEmail.Text = "";
            txtCEP.Text = "";
            txtEndereco.Text = "";
            txtBairro.Text = "";
            txtNum.Text = "";
            txtCidade.Text = "";
            txtComplemento.Text = "";
            txtCelular.Text = "";
            lblMensagem.Text = "";
            txtNomeCliente.Focus();
        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select * from cliente where nomecompleto like '%" + txtBuscar.Text + "%' and idtipo_cliente = 3";
            gdvDados.DataBind();
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

        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            //aqui vai inserir um registro novo no sistema
            if (hdfId.Value == "")
            {
                if (!auth.VerificaCNPJ(txtCPFCNPJ.Text))
                {
                    if (!auth.VerificaEmail(txtEmail.Text))
                    {
                        //cria o cliente na iugu
                        Clientes dadoscliente = new Clientes();
                        dadoscliente.email = txtEmail.Text;
                        dadoscliente.name = txtNomeCliente.Text;
                        dadoscliente.notes = "";
                        dadoscliente.phone = txtCelular.Text.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(" ", "").Substring(2, 9);
                        dadoscliente.phone_prefix = "0" + txtCelular.Text.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(" ", "").Substring(0, 2);
                        dadoscliente.cpf_cnpj = txtCPFCNPJ.Text.Replace("-", "").Replace(".", "");
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
               "INSERT INTO cliente (token, cnpj_cpf, rg, inscricao_estadual, razao_social, email, celular, nomecompleto, cep, endereco, bairro, numero, cidade, estado, complemento, idtipocliente, status, cadastrado_por, idiugu) values (@token, @cnpj_cpf, @rg, @inscricao_estadual, @razao_social, @email, @celular, @nomecompleto, @cep, @endereco, @bairro, @numero, @cidade, @estado, @complemento, @idtipocliente, @status, @cadastrado_por, @idiugu)");
                        db.AddInParameter(command, "@token", DbType.String, Criptografia.Encrypt(auth.GeraTokenAleatorio()).Replace("+", "=").Replace("/", "="));
                        db.AddInParameter(command, "@cnpj_cpf", DbType.String, txtCPFCNPJ.Text);
                        db.AddInParameter(command, "@rg", DbType.String, txtRG.Text);
                        db.AddInParameter(command, "@razao_social", DbType.String, "");
                        db.AddInParameter(command, "@inscricao_estadual", DbType.String, "");
                        db.AddInParameter(command, "@email", DbType.String, txtEmail.Text);
                        db.AddInParameter(command, "@celular", DbType.String, txtCelular.Text);
                        db.AddInParameter(command, "@nomecompleto", DbType.String, txtNomeCliente.Text);
                        db.AddInParameter(command, "@cep", DbType.String, txtCEP.Text);
                        db.AddInParameter(command, "@endereco", DbType.String, txtEndereco.Text);
                        db.AddInParameter(command, "@bairro", DbType.String, txtBairro.Text);
                        db.AddInParameter(command, "@numero", DbType.String, txtNum.Text);
                        db.AddInParameter(command, "@cidade", DbType.String, txtCidade.Text);
                        db.AddInParameter(command, "@estado", DbType.String, ddlUF.SelectedValue);
                        db.AddInParameter(command, "@complemento", DbType.String, txtComplemento.Text);
                        db.AddInParameter(command, "@idtipocliente", DbType.Int16, 3);//id do cliente final
                        db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                        db.AddInParameter(command, "@cadastrado_por", DbType.Int32, Convert.ToInt32(hdfIdUsuario.Value));
                        db.AddInParameter(command, "@idiugu", DbType.String, idiugu);

                            db.ExecuteNonQuery(command);

                            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from cliente where cnpj_cpf = '" + txtCPFCNPJ.Text + "'"))
                            {
                                if (reader.Read())
                                {
                                    string pw = auth.GeraTokenAleatorio();

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
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>CPF:</strong>" + txtCPFCNPJ.Text + "</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>E-mail:</strong>" + txtEmail.Text + "</p><br><br>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Senha de acesso</strong><br>" + pw + "</p>";
                                                    strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                                                    strHtml = strHtml + "</font><img src=''></body></html>";
                                                    
                                                    //base teste
                                                    Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Cliente", strHtml, 1);
                                                    //base oficial
                                                    Email.emailTxt("contato@w7agencia.com.br", txtEmail.Text, "", "", "Global 360 - Cliente", strHtml, 1);

                                                    lblMensagem.Text = "Informação salva com sucesso!";
                                                    txtNomeCliente.Text = "";
                                                    txtCPFCNPJ.Text = "";
                                                    txtEmail.Text = "";
                                                    txtCEP.Text = "";
                                                    txtEndereco.Text = "";
                                                    txtBairro.Text = "";
                                                    txtNum.Text = "";
                                                    txtCidade.Text = "";
                                                    txtComplemento.Text = "";
                                                    txtCelular.Text = "";
                                                    gdvDados.DataBind();
                                                    pnlModal.Visible = false;
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
            //aqui vai editar um registro dentro do sistema
            else
            {
                if (!auth.VerificaCNPJ(txtCPFCNPJ.Text))
                {
                    if (!auth.VerificaEmail(txtEmail.Text))
                    {
                        DbCommand command = db.GetSqlStringCommand(
               "UPDATE cliente SET rg = @rg, razao_social = @razao_social, inscricao_estadual = @inscricao_estadual, celular = @celular, nomecompleto = @nomecompleto, cep = @cep, endereco = @endereco, bairro = @bairro, numero = @numero, cidade = @cidade, estado = @estado, complemento = @complemento, status = @status where id = @id");
                        db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                        db.AddInParameter(command, "@rg", DbType.String, txtRG.Text);
                        db.AddInParameter(command, "@razao_social", DbType.String, "");
                        db.AddInParameter(command, "@inscricao_estadual", DbType.String, "");
                        db.AddInParameter(command, "@celular", DbType.String, txtCelular.Text);
                        db.AddInParameter(command, "@nomecompleto", DbType.String, txtNomeCliente.Text);
                        db.AddInParameter(command, "@cep", DbType.String, txtCEP.Text);
                        db.AddInParameter(command, "@endereco", DbType.String, txtEndereco.Text);
                        db.AddInParameter(command, "@bairro", DbType.String, txtBairro.Text);
                        db.AddInParameter(command, "@numero", DbType.String, txtNum.Text);
                        db.AddInParameter(command, "@cidade", DbType.String, txtCidade.Text);
                        db.AddInParameter(command, "@estado", DbType.String, ddlUF.SelectedValue);
                        db.AddInParameter(command, "@complemento", DbType.String, txtComplemento.Text);
                        db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);

                        try
                        {
                            db.ExecuteNonQuery(command);
                            lblMensagem.Text = "Informação atualizada com sucesso!";
                            txtNomeCliente.Text = "";
                            txtCPFCNPJ.Text = "";
                            txtEmail.Text = "";
                            txtCEP.Text = "";
                            txtEndereco.Text = "";
                            txtBairro.Text = "";
                            txtNum.Text = "";
                            txtCidade.Text = "";
                            txtComplemento.Text = "";
                            txtCelular.Text = "";
                            hdfId.Value = "";
                            gdvDados.DataBind();
                            pnlModal.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            lblMensagem.Text = "Erro ao tentar atualizada informação. " + ex.Message;
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
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            hdfId.Value = e.CommandArgument.ToString();
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * from cliente where id = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    txtNomeCliente.Text = reader["nomecompleto"].ToString();
                    txtCPFCNPJ.Text = reader["cnpj_cpf"].ToString();
                    txtRG.Text = reader["rg"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                    txtCEP.Text = reader["cep"].ToString();
                    txtEndereco.Text = reader["endereco"].ToString();
                    txtBairro.Text = reader["bairro"].ToString();
                    txtNum.Text = reader["numero"].ToString();
                    txtCidade.Text = reader["cidade"].ToString();
                    txtComplemento.Text = reader["complemento"].ToString();
                    ddlUF.SelectedValue = reader["estado"].ToString();
                    txtCelular.Text = reader["celular"].ToString();
                    ddlStatus.SelectedValue = reader["status"].ToString();
                    txtNomeCliente.Focus();
                    pnlModal.Visible = true;
                    lblMensagem.Text = "";
                }
            }
        }
    }
}