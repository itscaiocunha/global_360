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
using global.iugu;
using static global.iugu.Assinaturas;
using static global.iugu.token;
using w7startup.src.iugu;
using System.Web.UI.WebControls;

namespace global.lojista
{
    public partial class novavenda : System.Web.UI.Page
    {
        public static string BASEURRLTOKEN = @"https://api.iugu.com/v1/payment_token";
        public static string BASEURRL = @"https://api.iugu.com/v1/customers?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";
        public static string BASEURRLASSINATURA = @"https://api.iugu.com/v1/subscriptions?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";
        public static string BASEURLFATURA = @"https://api.iugu.com/v1/invoices?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D";
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
            "INSERT INTO pedido_produto (idpedido, idproduto, qtde, valor, lote, ean, ano_fabricacao, ano_modelo, chassi, cor, marca, modelo, placa, renavam) values (@idpedido, @idproduto, @qtde, @valor, @lote, @ean, @ano_fabricacao, @ano_modelo, @chassi, @cor, @marca, @modelo, @placa, @renavam)");
            db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
            db.AddInParameter(command, "@idproduto", DbType.Int16, Convert.ToInt16(ddlProduto.SelectedValue));
            db.AddInParameter(command, "@qtde", DbType.Int16, Convert.ToInt16(txtQtde.Text));
            db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.VerificaValor(ddlProduto.SelectedValue)));
            if (ddlLote.SelectedValue != "0")
                db.AddInParameter(command, "@lote", DbType.String, ddlLote.SelectedValue);
            else
                db.AddInParameter(command, "@lote", DbType.String, txtLote.Text);
            db.AddInParameter(command, "@ean", DbType.String, txtEAN.Text);
            db.AddInParameter(command, "@ano_fabricacao", DbType.String, txtAnoFabricacao.Text);
            db.AddInParameter(command, "@ano_modelo", DbType.String, txtAno.Text);
            db.AddInParameter(command, "@chassi", DbType.String, txtChassi.Text);
            db.AddInParameter(command, "@cor", DbType.String, txtCor.Text);
            db.AddInParameter(command, "@marca", DbType.String, txtMarca.Text);
            db.AddInParameter(command, "@modelo", DbType.String, txtModelo.Text);
            db.AddInParameter(command, "@placa", DbType.String, txtPlaca.Text);
            db.AddInParameter(command, "@renavam", DbType.String, txtRenavam.Text);
            try
            {
                db.ExecuteNonQuery(command);
                gdvDados.DataBind();
                RetornaValorTotal(lblNumeroPedido.Text);
                txtEAN.Text = "";
                txtQtde.Text = "1";
                lblMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            System.Threading.Thread.Sleep(200);

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string valorfinal = lblValorTotal.Text.Replace(",", "").Replace(".", "");
            valorfinal = valorfinal.PadRight(4, '0');
            try
            {
                var client = new RestClient($"{BASEURRLASSINATURA}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                Assinaturas dadosAssina = new Assinaturas();
                dadosAssina.plan_identifier = "premium_plan";
                dadosAssina.customer_id = lblMsgErro.Text;
                dadosAssina.expires_at = null;
                dadosAssina.only_on_charge_success = null;
                dadosAssina.ignore_due_email = null;
                dadosAssina.payable_with = "credit_card";
                dadosAssina.credits_based = false;
                dadosAssina.price_cents = Convert.ToInt32(valorfinal);
                dadosAssina.credits_cycle = null;
                dadosAssina.credits_min = 0;// Convert.ToInt32(lblValorTotal.Text.Replace(",", "").Replace(".", ""));
                List<subitem> item = new List<subitem>() { new subitem { description = "Mensalidade", price_cents = Convert.ToInt32(valorfinal), quantity = 1, recurrent = true } };
                dadosAssina.subitems = item;
                dadosAssina.two_step = false;
                dadosAssina.suspend_on_invoice_expired = false;
                dadosAssina.only_charge_on_due_date = false;

                var env = dadosAssina.toCreate();
                string json = JsonConvert.SerializeObject(env);
                request.AddParameter(
                    "application/json",
                    env,
                    ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                var dados = response.Content;
                string idiugu = dados.Substring(7, 32);

                //try
                //{
                //    var client3 = new RestClient($"{BASEURLFATURA}");
                //    var request3 = new RestRequest(Method.POST);
                //    request3.AddHeader("Accept", "application/json");
                //    fatura dadosFatura = new fatura();
                //    dadosFatura.email = "premium_plan";
                //    dadosFatura.cc_emails = lblMsgErro.Text;
                //    dadosFatura.due_date = null;
                //    dadosFatura.ensure_workday_due_date = null;
                //    dadosFatura.bank_slip_extra_due = "false";
                //    dadosFatura.return_url = "credit_card";
                //    dadosFatura.expired_url = false;
                //    dadosFatura.notification_url = 5990;
                //    dadosFatura.ignore_canceled_email = null;
                //    dadosFatura.fines = 0;
                //    fatura.items items = new fatura.items();
                //    items.description = "Mensalidade Global 360";
                //    items.price_cents = 5990;
                //    items.quantity = 1;
                //    dadosFatura.listItems = items;
                //    dadosFatura.late_payment_fine = false;
                //    dadosFatura.per_day_interest = true;
                //    dadosFatura.per_day_interest_value = true;
                //    dadosFatura.discount_cents = true;
                //    dadosFatura.customer_id = true;
                //    dadosFatura.ignore_due_email = true;
                //    dadosFatura.subscription_id = true;
                //    dadosFatura.payable_with = true;
                //    dadosFatura.credits = true;
                //    dadosFatura.early_payment_discount = true;
                //    dadosFatura.early_payment_discounts = true;
                //    dadosFatura.order_id = true;
                //    var env3 = dadosFatura.toCreate();
                //    request.AddParameter(
                //        "application/json",
                //        env3,
                //        ParameterType.RequestBody);

                //    IRestResponse response3 = client3.Execute(request3);
                //    var dados3 = response3.Content;
                //    string idfatura = dados3.Substring(7, 32);

                try
                {
                    DbCommand command = db.GetSqlStringCommand(
                    "UPDATE pedido SET desconto = @desconto, valor = @valor, idtaxa = @idtaxa, idlojista = @idlojista, idconsumidor = @idconsumidor, observacao = @observacao,rastreio = @rastreio, status = @status, notafiscal = @notafiscal, prazo_entrega = @prazo_entrega, data_entrega = @data_entrega, idcartao = @idcartao, datacadastro = getdate(), idassinatura = @idassinatura where id = @id");
                    db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
                    db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.RetornaTotalPedido(lblNumeroPedido.Text)));
                    db.AddInParameter(command, "@idtaxa", DbType.Int16, Convert.ToInt16(auth.RetornaTaxaComissao(Session["idcliente"].ToString())));
                    db.AddInParameter(command, "@idlojista", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                    db.AddInParameter(command, "@idconsumidor", DbType.Int16, Convert.ToInt16(lblIdCliente.Text));
                    db.AddInParameter(command, "@observacao", DbType.String, txtObservacoes.Text);
                    db.AddInParameter(command, "@rastreio", DbType.String, "");
                    db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(command, "@notafiscal", DbType.String, "");
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, 0);
                    db.AddInParameter(command, "@data_entrega", DbType.DateTime, DateTime.Now);
                    db.AddInParameter(command, "@idcartao", DbType.Int16, 0);
                    db.AddInParameter(command, "@idassinatura", DbType.String, idiugu);
                    db.AddInParameter(command, "@desconto", DbType.String, lblDesconto.Text);

                    db.ExecuteNonQuery(command);

                    try
                    {
                        auth.InserirStatus(lblNumeroPedido.Text, ddlStatus.SelectedValue);
                        auth.RotinaFaturas(lblNumeroPedido.Text);
                        auth.RotinaComissao(lblNumeroPedido.Text);

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

                        lblMensagemFinal.Text = "Pedido realizado com sucesso!";
                        lblNumeroPedidoFinal.Text = lblNumeroPedido.Text;
                        hplVerPedido.NavigateUrl = "../lojista/viewpedido.aspx?id=" + lblNumeroPedido.Text + "";
                        hplVerContrato.NavigateUrl = "../lojista/viewcontrato.aspx?id=" + lblNumeroPedido.Text + "";
                        pnlDadosFinais.Visible = false;
                        pnlFinal.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro ao tentar alterar dados de fatura. " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar salvar dados. " + ex.Message;
                }
                //}
                //catch (Exception ex)
                //{
                //    lblMensagem.Text = "Erro ao tentar gerar fatura do pedido. " + ex.Message;
                //}
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar conectar com pagamento. " + ex.Message;
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
                RetornaValorTotal(lblNumeroPedido.Text);
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

        public static void CriaComissao(string idpedido, string idcliente, string valor, string datavencimento)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select * from cliente_taxa where idcliente = '" + idcliente + "'"))
            {
                if (reader.Read())
                {
                    Database db = DatabaseFactory.CreateDatabase("ConnectionString");
                    double valortotal = Convert.ToDouble(valor);

                    if (reader["tipo"].ToString() == "P")//percentual 
                    {
                        double percentual = Convert.ToDouble(reader["valor"].ToString());
                        double vlr = percentual * valortotal;

                        DbCommand command = db.GetSqlStringCommand(
                                "INSERT INTO comissao (idpedido, idcliente, valor, datacadastro, datavencimento, status) values (@idpedido, @idcliente, @valor, getDate(), @datavencimento, @status)");
                        db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(idpedido));
                        db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(idcliente));
                        db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(vlr));
                        db.AddInParameter(command, "@datavencimento", DbType.DateTime, DateTime.Now.AddDays(30));
                        db.AddInParameter(command, "@status", DbType.String, "ATIVO");
                        try
                        {
                            db.ExecuteNonQuery(command);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if(reader["tipo"].ToString() == "V")//valor fixo uma vez 
                    {
                        double vlr = Convert.ToDouble(reader["valor"].ToString());

                        DbCommand command = db.GetSqlStringCommand(
                                "INSERT INTO comissao (idpedido, idcliente, valor, datacadastro, datavencimento, status) values (@idpedido, @idcliente, @valor, getDate(), @datavencimento, @status)");
                        db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(idpedido));
                        db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(idcliente));
                        db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(vlr));
                        db.AddInParameter(command, "@datavencimento", DbType.DateTime, DateTime.Now.AddDays(30));
                        db.AddInParameter(command, "@status", DbType.String, "ATIVO");
                        try
                        {
                            db.ExecuteNonQuery(command);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

        protected void rblCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblCliente.SelectedValue == "Novo Cliente")
            {
                pnlDadosCliente.Visible = true;
                txtNomeCliente.Text = "";
                txtCPFCNPJ.Text = "";
                txtEmail.Text = "";
                txtCEP.Text = "";
                txtEndereco.Text = "";
                txtBairro.Text = "";
                txtNum.Text = "";
                txtCidade.Text = "";
                txtRG.Text = "";
                txtComplemento.Text = "";
                txtCelular.Text = "";
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
                    txtRG.Text = reader["rg"].ToString();
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
                    lblMsgErro.Text = reader["idiugu"].ToString();
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
                "INSERT INTO cliente (token, cnpj_cpf, rg, email, celular, nomecompleto, cep, endereco, bairro, numero, cidade, estado, complemento, idtipocliente, status, contrato, cadastrado_por, datacadastro, idiugu) values (@token, @cnpj_cpf, @rg, @email, @celular, @nomecompleto, @cep, @endereco, @bairro, @numero, @cidade, @estado, @complemento, @idtipocliente, @status, @contrato, @cadastrado_por, getdate(), @idiugu)");
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
                            db.AddInParameter(command, "@idiugu", DbType.String, idiugu);


                            db.ExecuteNonQuery(command);

                            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                                  "SELECT * from cliente where cnpj_cpf = '" + txtCPFCNPJ.Text + "'"))
                            {
                                if (reader.Read())
                                {
                                    string pw = auth.GeraTokenAleatorio();
                                    lblIdCliente.Text = reader["id"].ToString();
                                    lblMsgErro.Text = reader["idiugu"].ToString();

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
                    lblMensagemCliente.Text = "Erro ao tentar atualizada informação. " + ex.Message;
                }
            }
        }

        protected void btnSalvarCarrinho_Click(object sender, EventArgs e)
        {
            //if (ddlCliente.SelectedValue != "0")
            //    ddlCartao.Visible = true;
            //else
            //    ddlCartao.Visible = true;

            pnlCarrinho.Visible = false;
            pnlDadosFinais.Visible = true;
            //pnlDadosPagamento.Visible = true;
            //txtNumeroCartao.Focus();
        }

        protected void btnSalvarCartao_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            //string idcartao = auth.VerificaCartao(txtNumeroCartao.Text, lblIdCliente.Text);
            if (ddlCartao.SelectedValue == "0")//novo cartão
            {
                try
                {
                    var client = new RestClient($"{BASEURRLTOKEN}");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/json");
                    token dadosAssina = new token();
                    dadosAssina.account_id = "2D31F42C262B4135A85420124CD71958";
                    dadosAssina.method = "credit_card";
                    dadosAssina.test = "false";
                    subitemstk ListaItems = new subitemstk();
                    ListaItems.number = txtNumeroCartao.Text;
                    ListaItems.verification_value = txtCodSegunraca.Text;
                    ListaItems.first_name = txtNomeCartao.Text;
                    ListaItems.last_name = txtUltimoNome.Text;
                    ListaItems.month = txtDataValidade.Text.Substring(0, 2);
                    ListaItems.year = "20" + txtDataValidade.Text.Substring(3, 2);
                    dadosAssina.data = ListaItems;
                    var env = dadosAssina.toCreate();
                    request.AddParameter(
                        "application/json",
                        env,
                        ParameterType.RequestBody);

                    IRestResponse response = client.Execute(request);
                    var dados = response.Content;
                    string idtoken = dados.Substring(7, 36);

                    var client2 = new RestClient($"https://api.iugu.com/v1/customers/59615B40438B42E4AC9DF0A5EA2E2634/payment_methods?api_token=A58C8CA308649C87AD34DC93E19C6E9EE3CCE1251DB456A4C0D61B2388401E0D");
                    var request2 = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/json");
                    formapagamento dadosForma = new formapagamento();
                    dadosForma.description = "Cartão de Pagamento";
                    dadosForma.token = idtoken;
                    dadosForma.set_as_default = "true";
                    var env2 = dadosForma.toCreate();
                    request2.AddParameter(
                        "application/json",
                        env2,
                        ParameterType.RequestBody);

                    IRestResponse response2 = client2.Execute(request2);
                    var dados2 = response2.Content;
                    string idpagamento = dados2.Substring(7, 32);

                    try
                    {
                        DbCommand command = db.GetSqlStringCommand(
            "INSERT INTO cartao (idcliente, nome, ultimonome, numero, vencimento, codigo, status, datacadastro, tokeiugu, idpagamento) values (@idcliente, @nome, @ultimonome, @numero, @vencimento, @codigo, @status, getdate(), @tokeiugu, @idpagamento)");
                        db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(lblIdCliente.Text));
                        db.AddInParameter(command, "@nome", DbType.String, txtNomeCartao.Text);
                        db.AddInParameter(command, "@ultimonome", DbType.String, txtUltimoNome.Text);
                        db.AddInParameter(command, "@numero", DbType.String, txtNumeroCartao.Text);
                        db.AddInParameter(command, "@vencimento", DbType.String, txtDataValidade.Text);
                        db.AddInParameter(command, "@codigo", DbType.String, txtCodSegunraca.Text);
                        db.AddInParameter(command, "@status", DbType.String, "ATIVO");
                        db.AddInParameter(command, "@tokeiugu", DbType.String, idtoken);
                        db.AddInParameter(command, "@idpagamento", DbType.String, idpagamento);

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
                    }
                    catch (Exception ex)
                    {
                        lblMensagemCartao.Text = "Erro ao tentar salvar os dados. Tente novamente! " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    lblMensagemCartao.Text = "Erro ao tentar conectar dados. Tente novamente! " + ex.Message;
                }
            }
            else
            {
                //utiliza o id retornado de cartão já cadastrado
                lblIdCartao.Text = ddlCartao.SelectedValue;
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

        protected void btnValidarCupom_Click(object sender, EventArgs e)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select valor from cupom where descricao = '" + txtCupom.Text + "' and status = 'ATIVO'"))
            {
                if (reader.Read())
                {
                    lblMsgCupom.Text = "Cupom adicionado com sucesso!";
                    lblDesconto.Text = reader["valor"].ToString();
                    RetornaValorTotal(lblNumeroPedido.Text);
                }
            }
        }

        public void RetornaValorTotal(string idpedido)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select sum(valor) as total from pedido_produto where idpedido = '" + idpedido + "'"))
            {
                if (reader.Read())
                {
                    lblSUbTotal.Text = reader["total"].ToString();
                    if (Convert.ToDouble(lblDesconto.Text) > 0)
                    {
                        lblValorTotal.Text = (Convert.ToDouble(lblSUbTotal.Text) - Convert.ToDouble(lblDesconto.Text)).ToString();
                    }
                    else
                    {
                        lblValorTotal.Text = lblSUbTotal.Text;
                    }
                }
            }
        }

        protected void ddlCartao_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select id, substring(numero,0,5)+'.****.****'+substring(numero,15,5) as dadoscartao, substring(nome,0,2)+'.****.****'+substring(nome,len(nome)-3,3) as nome from cartao where id = '" + ddlCartao.SelectedValue + "'"))
            {
                if (reader.Read())
                {
                    txtNumeroCartao.Text = reader["dadoscartao"].ToString();
                    txtCodSegunraca.Text = "***";
                    txtDataValidade.Text = "**/**";
                    txtNomeCartao.Text = reader["nome"].ToString();
                    txtUltimoNome.Text = "******";
                }
            }
        }


    }
}