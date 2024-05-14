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
    public partial class novopedido : System.Web.UI.Page
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
                        hdfIdProduto.Value = reader["novopedido"].ToString();
                        lblNumeroPedido.Text = reader["novopedido"].ToString();
                    }
                    else
                    {
                        hdfIdProduto.Value = "1";
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
            "INSERT INTO pedido_produto (idpedido, idproduto, qtde, valor, lote, ean, marca, modelo, placa, ano_modelo, cor, chassi, renavam, ano_fabricacao) values (@idpedido, @idproduto, @qtde, @valor, @lote, @ean, @marca, @modelo, @placa, @ano_modelo, @cor, @chassi, @renavam, @ano_fabricacao)");
            db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
            db.AddInParameter(command, "@idproduto", DbType.Int16, Convert.ToInt16(ddlProduto.SelectedValue));
            db.AddInParameter(command, "@qtde", DbType.Int16, Convert.ToInt16(txtQtde.Text));
            db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.VerificaValor(ddlProduto.SelectedValue)));
            db.AddInParameter(command, "@lote", DbType.String, Convert.ToInt16(ddlLote.SelectedValue));
            //db.AddInParameter(command, "@ean", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@marca", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@modelo", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@placa", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@ano_modelo", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@cor", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@chassi", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@renavam", DbType.String, txtEAN.Text);
            //db.AddInParameter(command, "@ano_fabricacao", DbType.String, txtEAN.Text);
            try
            {
                db.ExecuteNonQuery(command);
                gdvDados.DataBind();
                //txtEAN.Text = "";
                txtQtde.Text = "1";
                //txtEAN.Text = "";
                lblMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
            }

            RetornaValorTotal(lblNumeroPedido.Text);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(1000);

            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand command = db.GetSqlStringCommand(
                "UPDATE pedido SET valor = @valor, idtaxa = @idtaxa, idlojista = @idlojista, idconsumidor = @idconsumidor, observacao = @observacao, rastreio = @rastreio, status = @status, notafiscal = @notafiscal, prazo_entrega = @prazo_entrega, datacadastro = getdate() where id = @id");
            db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(lblNumeroPedido.Text));
            db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(auth.RetornaTotalPedido(lblNumeroPedido.Text)));
            db.AddInParameter(command, "@idtaxa", DbType.Int16, Convert.ToInt16(auth.RetornaTaxaComissao(ddlVendedor.SelectedValue)));
            db.AddInParameter(command, "@idlojista", DbType.Int16, Convert.ToInt16(ddlVendedor.SelectedValue));
            db.AddInParameter(command, "@idconsumidor", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
            db.AddInParameter(command, "@observacao", DbType.String, "");
            db.AddInParameter(command, "@rastreio", DbType.String, "");
            db.AddInParameter(command, "@status", DbType.String, "");
            db.AddInParameter(command, "@notafiscal", DbType.String, "");
            db.AddInParameter(command, "@prazo_entrega", DbType.Int16, 0);
            try
            {
                db.ExecuteNonQuery(command);

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                         "select * from pedido p join cliente c on c.id = p.idconsumidor where p.id = '" + lblNumeroPedido.Text + "'"))
                {
                    if (reader.Read())
                    {
                        //// corpo do e-mail
                        //string strHtml = "<html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
                        //strHtml = strHtml + "<title>Global 360 - Plataforma Digital</title></head><body><br>";
                        //strHtml = strHtml + "<img src='https://global360.app.br/src/img/logo/logo_global.png' width='200' alt='Logo'>";
                        //strHtml = strHtml + "<p><strong><font size='2' face='Verdana, Arial, Helvetica, sans-serif'>Novo Pedido<br>Global 360 - Plataforma Digital</font></strong></p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Olá, tudo bem?</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p>Seu pedido foi realizado com sucesso na plataforma.</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>CNPJ:</strong>" + reader["cnpj_cpf"].ToString() + "</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Empresa:</strong>" + reader["nomecompleto"].ToString() + "</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Razão Social:</strong>" + reader["razao_social"].ToString() + "</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Número do Pedido:</strong>" + lblNumeroPedido.Text + "</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><strong>Valor total:</strong> R$ " + reader["valor"].ToString() + "</p>";
                        //strHtml = strHtml + "<font size='2' face='Verdana, Arial, Helvetica, sans-serif'><p><a href='https://global360.app.br/src/login.aspx'>Plataforma Global 360</a></p>";
                        //strHtml = strHtml + "</font><img src=''></body></html>";

                        ////base teste
                        //Email.emailTxt("contato@w7agencia.com.br", "contato@w7agencia.com.br", "", "", "Global 360 - Novo Pedido", strHtml, 1);
                        ////base oficial
                        //Email.emailTxt("contato@w7agencia.com.br", reader["email"].ToString(), "", "", "Global 360 - Novo Pedido", strHtml, 1);
                    }
                }

                lblSUbTotal.Visible = false;
                lblDesconto.Visible = false;
                lblValorTotal.Visible = false;

                lblMensagem.Text = "<span style='color: black; font-weight: bold; font-size: 16px;'>Pedido realizado com sucesso!</span>";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
            }

            //gera novo pedido
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

            DbCommand command2 = db.GetSqlStringCommand(
                    "INSERT INTO pedido (valor) values (0)");

            db.ExecuteNonQuery(command2);

            gdvDados.DataBind();
    }

        protected void CkbPagamento_CheckedChanged(object sender, EventArgs e)
        {
            if(CkbPagamento.Checked)
            {
                pnlPagamento.Visible = true;
            }
            else
            {
                pnlPagamento.Visible = false;
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
                //txtEAN.Text = "";
                txtQtde.Text = "1";
                //txtEAN.Text = "";
                hdfIdProduto.Value = "";
                lblMensagem.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar informação. " + ex.Message;
            }
        }

        protected void btnValidarCupom_Click(object sender, EventArgs e)
        {
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select valor from cupom where descricao = '"+txtCupom.Text+"' and status = 'ATIVO'"))
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
                    if(Convert.ToDouble(lblDesconto.Text) > 0)
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
    }
}