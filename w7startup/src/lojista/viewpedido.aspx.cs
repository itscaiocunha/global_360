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
    public partial class viewpedido : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            hdfId.Value = Request.QueryString["id"].ToString();
            //traz o conteudo do contrato para salvar no cadastro final
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select p.id, p.valor, p.rastreio, p.prazo_entrega, p.data_entrega, FORMAT(p.DATACADASTRO,'dd/MM/yyyy') as datacadastro, p.[status], c1.nomecompleto as nomefantasia, c1.cidade+'/'+c1.estado as local_lojista, c1.celular as telefonelojista, c2.nomecompleto as nomecliente, c2.endereco+' '+c2.bairro+' '+c2.numero+','+c2.cidade+'/'+c2.estado as enderecocliente from pedido p join cliente c1 on c1.id = p.idlojista join cliente c2 on c2.id = p.idconsumidor where p.id = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    lblNumeroPedido.Text = reader["id"].ToString();
                    lblRazaoSocial.Text = reader["nomefantasia"].ToString();
                    lblCidadeUf.Text = reader["local_lojista"].ToString();
                    lblTelefone.Text = reader["telefonelojista"].ToString();
                    lblDataEntrega.Text = reader["data_entrega"].ToString();
                    lblDataPedido.Text = reader["datacadastro"].ToString();
                    lblStatusAtual.Text = reader["status"].ToString();
                    lblEndereco.Text = reader["enderecocliente"].ToString();
                    lblPrazoEntrega.Text = reader["prazo_entrega"].ToString();
                    lblRastreio.Text = reader["rastreio"].ToString();
                    lblValorTotal.Text = reader["valor"].ToString();
                    lblCliente.Text = reader["nomecliente"].ToString();
                    ddlStatus.SelectedValue = reader["status"].ToString();
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            if (txtDataEntrega.Text == "")
            {
                DbCommand command = db.GetSqlStringCommand(
                    "UPDATE pedido SET rastreio = @rastreio, status = @status, notafiscal = @notafiscal, prazo_entrega = @prazo_entrega where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                db.AddInParameter(command, "@rastreio", DbType.String, txtRastreio.Text);
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                db.AddInParameter(command, "@notafiscal", DbType.String, txtLinkNF.Text);
                if(txtPrazo.Text == "")
                db.AddInParameter(command, "@prazo_entrega", DbType.Int16, 0);
                else
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, Convert.ToInt16(txtPrazo.Text));
                try
                {
                    db.ExecuteNonQuery(command);

                    auth.InserirStatus(lblNumeroPedido.Text, ddlStatus.SelectedValue);

                    lblMensagem.Text = "Informação atualizada com sucesso!";
                    txtDataEntrega.Text = "";
                    txtLinkNF.Text = "";
                    txtPrazo.Text = "";
                    txtRastreio.Text = "";
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar atualizada informação. " + ex.Message;
                }
            }
            else
            {
                DbCommand command = db.GetSqlStringCommand(
                    "UPDATE pedido SET rastreio = @rastreio, status = @status, notafiscal = @notafiscal, prazo_entrega = @prazo_entrega, data_entrega = @data_entrega where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                db.AddInParameter(command, "@rastreio", DbType.String, txtRastreio.Text);
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                db.AddInParameter(command, "@notafiscal", DbType.String, txtLinkNF.Text);
                if (txtPrazo.Text == "")
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, 0);
                else
                    db.AddInParameter(command, "@prazo_entrega", DbType.Int16, Convert.ToInt16(txtPrazo.Text));
                db.AddInParameter(command, "@data_entrega", DbType.DateTime, Convert.ToDateTime(txtDataEntrega.Text));
                try
                {
                    db.ExecuteNonQuery(command);

                    auth.InserirStatus(lblNumeroPedido.Text, ddlStatus.SelectedValue);

                    lblMensagem.Text = "Informação atualizada com sucesso!";
                    txtDataEntrega.Text = "";
                    txtLinkNF.Text = "";
                    txtPrazo.Text = "";
                    txtRastreio.Text = "";
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar atualizada informação. " + ex.Message;
                }
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Response.Redirect("printpedido.aspx?id="+hdfId.Value+"", false);
        }
    }
}