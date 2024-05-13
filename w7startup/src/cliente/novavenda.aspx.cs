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

namespace global.cliente
{
    public partial class novavenda : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select top 1 id+1 as novopedido from pedido order by id desc"))
                {
                    if (reader.Read())
                    {
                        hdfId.Value = reader["novopedido"].ToString();
                        lblNumeroPedido.Text = reader["novopedido"].ToString();
                    }
                    else
                    {
                        hdfId.Value = "1";
                        lblNumeroPedido.Text = "1";
                    }
                }

                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                DbCommand command = db.GetSqlStringCommand(
                        "INSERT INTO pedido (valor) values (0)");

                db.ExecuteNonQuery(command);
            }
        }

        protected void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand command = db.GetSqlStringCommand(
            "INSERT INTO pedido_produto (idpedido, idproduto, qtde, valor, lote, ean) values (@idpedido, @idproduto, @qtde, @valor, @lote, @ean)");
            db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
            db.AddInParameter(command, "@idproduto", DbType.Int16, Convert.ToInt16(ddlProduto.SelectedValue));
            db.AddInParameter(command, "@qtde", DbType.Int16, Convert.ToInt16(txtQtde.Text));
            db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.VerificaValor(ddlProduto.SelectedValue)));
            db.AddInParameter(command, "@lote", DbType.String, ddlLote.SelectedValue);
            db.AddInParameter(command, "@ean", DbType.String, txtEAN.Text);
            try
            {
                db.ExecuteNonQuery(command);
                gdvDados.DataBind();
                txtEAN.Text = "";
                txtQtde.Text = "1";
                txtEAN.Text = "";
                lblMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
            }
        }
    

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            if (txtDataEntrega.Text == "")
            {
                try
                {
                    DbCommand command = db.GetSqlStringCommand(
                    "UPDATE pedido SET valor = @valor, idtaxa = @idtaxa, idlojista = @idlojista, idconsumidor = @idconsumidor, observacao = @observacao, rastreio = @rastreio, status = @status, notafiscal = @notafiscal, prazo_entrega = @prazo_entrega, idcartao = @idcartao, datacadastro = getdate() where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.RetornaTotalPedido(lblNumeroPedido.Text)));
                db.AddInParameter(command, "@idtaxa", DbType.Int16, Convert.ToInt16(auth.RetornaTaxaComissao(Session["idcliente"].ToString())));
                db.AddInParameter(command, "@idlojista", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                db.AddInParameter(command, "@idconsumidor", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                db.AddInParameter(command, "@observacao", DbType.String, txtObservacoes.Text);
                db.AddInParameter(command, "@rastreio", DbType.String, txtRastreio.Text);
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                db.AddInParameter(command, "@notafiscal", DbType.String, txtLinkNF.Text);
                if (txtPrazo.Text == "")
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, 0);
                else
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, Convert.ToInt16(txtPrazo.Text));
                db.AddInParameter(command, "@idcartao", DbType.Int16, Convert.ToInt16(lblIdCartao.Text));

                
                    db.ExecuteNonQuery(command);

                    auth.InserirStatus(lblNumeroPedido.Text, ddlStatus.SelectedValue);
                    auth.RotinaFaturas(lblNumeroPedido.Text);

                    using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                             "select * from pedido p join cliente c on c.id = p.idconsumidor where p.id = '" + lblNumeroPedido.Text + "'"))
                    {
                        if (reader.Read())
                        {
                            // corpo do e-mail
                            string strHtml = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
                            strHtml = strHtml + "<title>Global 360 - Plataforma Digital</title></head><body><br>";
                            strHtml = strHtml + "<img src='https://global360.app.br/src/img/logo/logo_global.png' width='200' alt='Logo'>";
                            strHtml = strHtml + "<p><strong><font size='2' face='Verdana, Arial, Helvetica, sans-serif'>Nova Venda<br>Global 360 - Plataforma Digital</font></strong></p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Olá, tudo bem?</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Sua venda foi realizada com sucesso na plataforma.</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>CPF:</strong>" + reader["cnpj_cpf"].ToString() + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Cliente:</strong>" + reader["nomecompleto"].ToString() + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Número do Pedido:</strong>" + lblNumeroPedido.Text + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Valor total:</strong> R$ " + reader["valor"].ToString() + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                            strHtml = strHtml + "</font><img src=''></body></html>";

                            //base teste
                            Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Nova Venda", strHtml, 1);
                            //base oficial
                            Email.emailTxt("contato@w7agencia.com.br", reader["email"].ToString(), "", "", "Global 360 - Nova Compra", strHtml, 1);
                        }
                    }

                    txtDataEntrega.Text = "";
                    txtLinkNF.Text = "";
                    txtPrazo.Text = "";
                    txtRastreio.Text = "";
                    lblMensagemFinal.Text = "Pedido realizado com sucesso!";
                    lblNumeroPedidoFinal.Text = lblNumeroPedido.Text;
                    hplVerPedido.NavigateUrl = "../lojista/viewpedido.aspx?id=" + lblNumeroPedido.Text+"";
                    hplVerContrato.NavigateUrl = "../lojista/viewcontrato.aspx?id=" + lblNumeroPedido.Text + "";
                    pnlDadosFinais.Visible = false;
                    pnlFinal.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
                }
            }
            else
            {
                try
                {
                    DbCommand command = db.GetSqlStringCommand(
                    "UPDATE pedido SET valor = @valor, idtaxa = @idtaxa, idlojista = @idlojista, idconsumidor = @idconsumidor, observacao = @observacao,rastreio = @rastreio, status = @status, notafiscal = @notafiscal, prazo_entrega = @prazo_entrega, data_entrega = @data_entrega, idcartao = @idcartao, datacadastro = getdate() where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.RetornaTotalPedido(lblNumeroPedido.Text)));
                db.AddInParameter(command, "@idtaxa", DbType.Int16, Convert.ToInt16(auth.RetornaTaxaComissao(Session["idcliente"].ToString())));
                db.AddInParameter(command, "@idlojista", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                db.AddInParameter(command, "@idconsumidor", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                db.AddInParameter(command, "@observacao", DbType.String, txtObservacoes.Text);
                db.AddInParameter(command, "@rastreio", DbType.String, txtRastreio.Text);
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                db.AddInParameter(command, "@notafiscal", DbType.String, txtLinkNF.Text);
                if (txtPrazo.Text == "")
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, 0);
                else
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, Convert.ToInt16(txtPrazo.Text));
                db.AddInParameter(command, "@data_entrega", DbType.DateTime, Convert.ToDateTime(txtDataEntrega.Text));
                db.AddInParameter(command, "@idcartao", DbType.Int16, Convert.ToInt16(lblIdCartao.Text));
               
                    db.ExecuteNonQuery(command);

                    auth.InserirStatus(lblNumeroPedido.Text, ddlStatus.SelectedValue);
                    auth.RotinaFaturas(lblNumeroPedido.Text);

                    using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                             "select * from pedido p join cliente c on c.id = p.idconsumidor where p.id = '" + lblNumeroPedido.Text + "'"))
                    {
                        if (reader.Read())
                        {
                            // corpo do e-mail
                            string strHtml = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
                            strHtml = strHtml + "<title>Global 360 - Plataforma Digital</title></head><body><br>";
                            strHtml = strHtml + "<img src='https://global360.app.br/src/img/logo/logo_global.png' width='200' alt='Logo'>";
                            strHtml = strHtml + "<p><strong><font size='2' face='Verdana, Arial, Helvetica, sans-serif'>Nova Venda<br>Global 360 - Plataforma Digital</font></strong></p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Olá, tudo bem?</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Sua venda foi realizada com sucesso na plataforma.</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>CPF:</strong>" + reader["cnpj_cpf"].ToString() + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Cliente:</strong>" + reader["nomecompleto"].ToString() + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Número do Pedido:</strong>" + lblNumeroPedido.Text + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Valor total:</strong> R$ " + reader["valor"].ToString() + "</p>";
                            strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                            strHtml = strHtml + "</font><img src=''></body></html>";

                            //base teste
                            Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Nova Venda", strHtml, 1);
                            //base oficial
                            Email.emailTxt("contato@w7agencia.com.br", reader["email"].ToString(), "", "", "Global 360 - Nova Compra", strHtml, 1);
                        }
                    }

                    txtDataEntrega.Text = "";
                    txtLinkNF.Text = "";
                    txtPrazo.Text = "";
                    txtRastreio.Text = "";
                    lblMensagemFinal.Text = "Pedido realizado com sucesso!";
                    lblNumeroPedidoFinal.Text = lblNumeroPedido.Text;
                    hplVerPedido.NavigateUrl = "../lojista/viewpedido.aspx?id=" + lblNumeroPedido.Text + "";
                    hplVerContrato.NavigateUrl = "../lojista/viewcontrato.aspx?id=" + lblNumeroPedido.Text + "";
                    pnlDadosFinais.Visible = false;
                    pnlFinal.Visible = true;
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
                }
            }
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            hdfIdProduto.Value = e.CommandArgument.ToString();
            DbCommand command = db.GetSqlStringCommand(
                    "DELETE FROM pedido_produto  where idpedido = @idpedido and idproduto = @idproduto");
            db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
            db.AddInParameter(command, "@idproduto", DbType.Int16, Convert.ToInt16(hdfIdProduto.Value));
            try
            {
                db.ExecuteNonQuery(command);
                gdvDados.DataBind();
                txtEAN.Text = "";
                txtQtde.Text = "1";
                txtEAN.Text = "";
                hdfIdProduto.Value = "";
                lblMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
            }
        }

        protected void rblCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblCliente.SelectedValue == "Novo Cliente")
            {
                pnlDadosCliente.Visible = true;
                ddlCliente.Visible = false;
            }
            else
            {
                ddlCliente.Visible = true;
                pnlDadosCliente.Visible = false;
            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select * from cliente where id = '" + ddlCliente.SelectedValue + "'"))
            {
                if (reader.Read())
                {
                    txtNomeCliente.Text = reader["nomecompleto"].ToString();
                    txtCPFCNPJ.Text = reader["cnpj_cpf"].ToString();
                    txtCPFCNPJ.Enabled = false;
                    txtEmail.Text = reader["email"].ToString();                    
                    txtCelular.Text = reader["celular"].ToString();
                    txtCEP.Text = reader["cep"].ToString();
                    txtEndereco.Text = reader["endereco"].ToString();
                    txtNum.Text = reader["numero"].ToString();
                    txtBairro.Text = reader["bairro"].ToString();
                    txtCidade.Text = reader["cidade"].ToString();
                    ddlUF.SelectedValue = reader["estado"].ToString();
                    txtComplemento.Text = reader["complemento"].ToString();
                    txtRG.Text = reader["rg"].ToString();
                    lblIdCliente.Text = reader["id"].ToString();
                    pnlDadosCliente.Visible = true;
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

        protected void btnSalvarCliente_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            if (lblIdCliente.Text == "")
            {
                if (!auth.VerificaCNPJ(txtCPFCNPJ.Text))
                {
                    if (!auth.VerificaEmail(txtEmail.Text))
                    {
                        DbCommand command = db.GetSqlStringCommand(
                "INSERT INTO cliente (token, cnpj_cpf, rg, email, celular, nomecompleto, cep, endereco, bairro, numero, cidade, estado, complemento, idtipocliente, status, contrato, cadastrado_por, datacadastro) values (@token, @cnpj_cpf, @rg, @email, @celular, @nomecompleto, @cep, @endereco, @bairro, @numero, @cidade, @estado, @complemento, @idtipocliente, @status, @contrato, @cadastrado_por, getdate())");
                        db.AddInParameter(command, "@token", DbType.String, Criptografia.Encrypt(auth.GeraTokenAleatorio()).Replace("+", "=").Replace("/", "="));
                        db.AddInParameter(command, "@cnpj_cpf", DbType.String, txtCPFCNPJ.Text);
                        db.AddInParameter(command, "@rg", DbType.String, txtRG.Text);
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
                        db.AddInParameter(command, "@idtipocliente", DbType.Int16, 3);//id do tipo de cliente = cliente final
                        db.AddInParameter(command, "@status", DbType.String, "Ativo");
                        db.AddInParameter(command, "@contrato", DbType.String, "");
                        db.AddInParameter(command, "@cadastrado_por", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                        try
                        {
                            db.ExecuteNonQuery(command);

                            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from cliente where cnpj_cpf = '" + txtCPFCNPJ.Text + "'"))
                            {
                                if (reader.Read())
                                {
                                    string pw = auth.GeraTokenAleatorio();
                                    lblIdCliente.Text = reader["id"].ToString();

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

                                                    ddlCliente.Visible = false;
                                                    txtCPFCNPJ.Enabled = true;
                                                    txtNomeCliente.Text = "";
                                                    txtCPFCNPJ.Text = "";
                                                    txtEmail.Text = "";
                                                    txtCEP.Text = "";
                                                    txtEndereco.Text = "";
                                                    txtRG.Text = "";
                                                    txtBairro.Text = "";
                                                    txtNum.Text = "";
                                                    txtCidade.Text = "";
                                                    txtComplemento.Text = "";
                                                    txtCelular.Text = "";
                                                    pnlDadosCliente.Visible = false;
                                                    pnlCarrinho.Visible = true;
                                                    ddlProduto.Focus();
                                                }
                                                catch (Exception ex)
                                                {
                                                    lblMensagemCliente.Text = "Erro ao tentar salvar informação2. " + ex.Message;
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        lblMensagemCliente.Text = "Erro ao tentar salvar informação3. " + ex.Message;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            lblMensagemCliente.Text = "Erro ao tentar salvar informação. " + ex.Message;
                        }
                    }
                    else
                    {
                        lblMensagemCliente.Text = "Não foi possível realizar o cadastro! Verifique o seu e-mail.";
                    }
                }
                else
                {
                    lblMensagemCliente.Text = "Não foi possível realizar o cadastro! Verifique o seu cpf.";
                }
            }
            else
            {
                DbCommand command = db.GetSqlStringCommand(
               "UPDATE cliente SET rg = @rg, celular = @celular, nomecompleto = @nomecompleto, cep = @cep, endereco = @endereco, bairro = @bairro, numero = @numero, cidade = @cidade, estado = @estado, complemento = @complemento, status = @status, contrato = @contrato where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                db.AddInParameter(command, "@rg", DbType.String, txtRG.Text);
                db.AddInParameter(command, "@celular", DbType.String, txtCelular.Text);
                db.AddInParameter(command, "@nomecompleto", DbType.String, txtNomeCliente.Text);
                db.AddInParameter(command, "@cep", DbType.String, txtCEP.Text);
                db.AddInParameter(command, "@endereco", DbType.String, txtEndereco.Text);
                db.AddInParameter(command, "@bairro", DbType.String, txtBairro.Text);
                db.AddInParameter(command, "@numero", DbType.String, txtNum.Text);
                db.AddInParameter(command, "@cidade", DbType.String, txtCidade.Text);
                db.AddInParameter(command, "@estado", DbType.String, ddlUF.SelectedValue);
                db.AddInParameter(command, "@complemento", DbType.String, txtComplemento.Text);
                db.AddInParameter(command, "@status", DbType.String, "Ativo");
                db.AddInParameter(command, "@contrato", DbType.String, "");
                try
                {
                    db.ExecuteNonQuery(command);

                    lblIdCliente.Text = ddlCliente.SelectedValue;
                    ddlCliente.Visible = false;
                    txtCPFCNPJ.Enabled = true;
                    txtNomeCliente.Text = "";
                    txtCPFCNPJ.Text = "";
                    txtEmail.Text = "";
                    txtCEP.Text = "";
                    txtEndereco.Text = "";
                    txtRG.Text = "";
                    txtBairro.Text = "";
                    txtNum.Text = "";
                    txtCidade.Text = "";
                    txtComplemento.Text = "";
                    txtCelular.Text = "";
                    pnlDadosCliente.Visible = false;
                    pnlCarrinho.Visible = true;
                    ddlProduto.Focus();
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar atualizada informação. " + ex.Message;
                }
            }
        }

        protected void btnSalvarCarrinho_Click(object sender, EventArgs e)
        {
            pnlCarrinho.Visible = false;
            pnlDadosPagamento.Visible = true;
            txtNumeroCartao.Focus();
        }

        protected void btnSalvarCartao_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string idcartao = auth.VerificaCartao(txtNumeroCartao.Text, lblIdCliente.Text);
            if (idcartao == "")
            {
                try
                {
                    DbCommand command = db.GetSqlStringCommand(
        "INSERT INTO cartao (idcliente, nome, numero, vencimento, codigo, status, datacadastro) values (@idcliente, @nome, @numero, @vencimento, @codigo, @status, getdate())");
                    db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(lblIdCliente.Text));
                    db.AddInParameter(command, "@nome", DbType.String, txtNomeCartao.Text);
                    db.AddInParameter(command, "@numero", DbType.String, txtNumeroCartao.Text);
                    db.AddInParameter(command, "@vencimento", DbType.String, txtDataValidade.Text);
                    db.AddInParameter(command, "@codigo", DbType.String, txtCodSegunraca.Text);
                    db.AddInParameter(command, "@status", DbType.String, "ATIVO");

                    db.ExecuteNonQuery(command);

                    using (IDataReader reader2 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "SELECT id from cartao where numero = '" + txtNumeroCartao.Text + "' and idcliente = '" + lblIdCliente.Text + "'"))
                    {
                        if (reader2.Read())
                        {
                            lblIdCartao.Text = reader2["id"].ToString();
                        }
                    }

                    pnlDadosPagamento.Visible = false;
                    pnlDadosFinais.Visible = true;
                    txtLinkNF.Focus();
                }
                catch (Exception ex)
                {
                    lblMensagemCartao.Text = "Erro ao tentar salvar os dados. Tente novamente! " + ex.Message;
                }
            }
            else
            {
                //utiliza o id retornado de cartão já cadastrado
                lblIdCartao.Text = idcartao;
                pnlDadosPagamento.Visible = false;
                pnlDadosFinais.Visible = true;
            }
        }

            protected void btnNovoPedido_Click(object sender, EventArgs e)
        {
            pnlFinal.Visible = false;
            rblCliente.Focus();
            lblIdCartao.Text = "";
            lblIdCliente.Text = "";
            hdfIdProduto.Value = "";
            
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select top 1 id+1 as novopedido from pedido order by id desc"))
            {
                if (reader.Read())
                {
                    hdfId.Value = reader["novopedido"].ToString();
                    lblNumeroPedido.Text = reader["novopedido"].ToString();
                }
                else
                {
                    hdfId.Value = "1";
                    lblNumeroPedido.Text = "1";
                }
            }

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand command = db.GetSqlStringCommand(
                    "INSERT INTO pedido (valor) values (0)");

            db.ExecuteNonQuery(command);
        }
    }
}