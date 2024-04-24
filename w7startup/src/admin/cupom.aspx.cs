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
using System.Web.Services.Description;
using System.Web.UI.WebControls.WebParts;

namespace global
{
    public partial class cupom : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {

            
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            hdfId.Value = e.CommandArgument.ToString();
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * from cupom where id = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    txtNomePerfil.Text = reader["descricao"].ToString();
                    ddlStatus.SelectedValue = reader["status"].ToString();
                    txtValor.Text = reader["valor"].ToString();
                    txtNomePerfil.Focus();
                    pnlModal.Visible = true;
                    lblMensagem.Text = "";
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            //aqui vai inserir um registro novo no sistema
            if (hdfId.Value == "")
            {
                DbCommand command = db.GetSqlStringCommand(
                "INSERT INTO cupom (descricao, valor, status) values (@descricao, @valor, @status)");
                db.AddInParameter(command, "@descricao", DbType.String, txtNomePerfil.Text);
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(txtValor.Text));
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação salva com sucesso!";
                    txtNomePerfil.Text = "";
                    txtValor.Text = "";
                    gdvDados.DataBind();
                    pnlModal.Visible = false;
                }
                catch(Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar salvar informação. " + ex.Message;
                }
            }
            //aqui vai editar um registro dentro do sistema
            else
            {
                DbCommand command = db.GetSqlStringCommand(
               "UPDATE cupom SET descricao = @descricao, valor = @valor, status = @status where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                db.AddInParameter(command, "@descricao", DbType.String, txtNomePerfil.Text);
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(txtValor.Text));
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação atualizada com sucesso!";
                    txtNomePerfil.Text = "";
                    txtValor.Text = "";
                    hdfId.Value = "";
                    gdvDados.DataBind();
                    pnlModal.Visible = false;
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar atualizada informação. " + ex.Message;
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = true;
            txtNomePerfil.Text = "";
            lblMensagem.Text = "";
            txtNomePerfil.Focus();
        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select * from cupom where descricao like '%" + txtBuscar.Text+"%'";
            gdvDados.DataBind();
        }
    }
}