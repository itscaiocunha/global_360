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
using System.Web.UI.WebControls;

namespace global.admin
{
    public partial class lote : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            hdfIdUsuario.Value = Session["idcliente"].ToString();
            //traz o conteudo do contrato para salvar no cadastro final
            //using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
            //                  "SELECT * from contrato where idtipocliente = '2'"))
            //{
            //    if (reader.Read())
            //    {
            //        hdfContrato.Value = reader["conteudo"].ToString();
            //    }
            //}
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = true;
            txtLote.Text = "";
            txtIMEI.Text = "";
        }

        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            if (ddlLote.SelectedValue == "0")
            {
                try
                {
                    DbCommand command = db.GetSqlStringCommand("INSERT INTO lote (numlote, idproduto, status, data_criacao) values (@lote, @produto, @status, GETDATE())");
                    db.AddInParameter(command, "@lote", DbType.String, txtLote.Text);
                    db.AddInParameter(command, "@produto", DbType.String, Convert.ToInt16(ddlProduto.SelectedValue));
                    db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);

                    try
                    {
                        db.ExecuteNonQuery(command);

                        using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                 "SELECT top 1 idlote from lote order by data_criacao desc"))
                        {
                            if (reader.Read())
                            {
                                DbCommand command2 = db.GetSqlStringCommand("INSERT INTO lote_imei (idlote, IMEI) values (@idlote, @IMEI)");
                                db.AddInParameter(command2, "@idlote", DbType.Int16, Convert.ToInt16(reader["idlote"].ToString()));
                                db.AddInParameter(command2, "@IMEI", DbType.String, txtIMEI.Text);

                                try
                                {
                                    db.ExecuteNonQuery(command2);
                                    ddlLote.DataBind();
                                    lblMensagem.Text = "Informação salva com sucesso!";
                                }
                                catch (Exception ex)
                                {
                                    lblMensagem.Text = "Lote salvo com sucesso, mas erro ao salvar IMEI. " + ex.Message;
                                }                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Erro ao tentar salvar informação. " + ex.Message;
                    }

                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao entrar no banco. " + ex.Message;
                }
            }
            else
            {
                DbCommand command = db.GetSqlStringCommand("INSERT INTO lote_imei (idlote, IMEI) values (@idlote, @IMEI)");
                db.AddInParameter(command, "@idlote", DbType.Int16, Convert.ToInt16(ddlLote.SelectedValue));
                db.AddInParameter(command, "@IMEI", DbType.String, txtIMEI.Text);

                try
                {
                    db.ExecuteNonQuery(command);
                    ddlLote.DataBind();
                    lblMensagem.Text = "Informação salva com sucesso!";
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar salvar informação. " + ex.Message;
                }
            }

        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select * from lote where name_produto like '%" + txtBuscar.Text + "%'";
            gdvDados.DataBind();
        }

        protected void gdvDados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                HiddenField1.Value = e.CommandArgument.ToString();

                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "SELECT l.idlote, l.numlote as numlote, l.idproduto as idproduto, l.status as status, lm.imei as IMEI from lote l inner join lote_imei lm on l.idlote = lm.idlote where l.idlote = '" + HiddenField1.Value + "'"))
                {
                    if (reader.Read())
                    {
                        txtLote.Text = reader["numlote"].ToString();
                        ddlProduto.SelectedValue = reader["idproduto"].ToString();
                        ddlStatus.SelectedValue = reader["status"].ToString();
                        txtIMEI.Text = reader["IMEI"].ToString();

                        pnlModal.Visible = true;
                        lblMensagem.Text = "";
                    }
                }
            }
        }


        protected void ddlLote_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLote.SelectedValue == "0")
                txtLote.Visible = true;
            else
                txtLote.Visible = false;
        }
    }
}