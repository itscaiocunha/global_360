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

namespace global.lojista
{
    public partial class contratos : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {

            
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            hdfId.Value = e.CommandArgument.ToString();
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * from contrato where id = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    txtTitulo.Text = reader["descricao"].ToString();
                    ddlTipoCliente.SelectedValue = reader["idtipocliente"].ToString();
                    txtConteudo.Text = reader["conteudo"].ToString();
                    ddlStatus.SelectedValue = reader["status"].ToString();
                    txtTitulo.Focus();
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
                "INSERT INTO contrato (descricao, conteudo, idtipocliente, status, datacadastro) values (@descricao, @conteudo, @idtipocliente, @status, getDate())");
                db.AddInParameter(command, "@descricao", DbType.String, txtTitulo.Text);
                db.AddInParameter(command, "@conteudo", DbType.String, txtConteudo.Text);
                db.AddInParameter(command, "@idtipocliente", DbType.Int16, Convert.ToInt16(ddlTipoCliente.SelectedValue));
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação salva com sucesso!";
                    txtTitulo.Text = "";
                    txtConteudo.Text = "";
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
               "UPDATE contrato SET descricao = @descricao, conteudo= @conteudo, idtipocliente = @idtipocliente, status = @status where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                db.AddInParameter(command, "@descricao", DbType.String, txtTitulo.Text);
                db.AddInParameter(command, "@conteudo", DbType.String, txtConteudo.Text);
                db.AddInParameter(command, "@idtipocliente", DbType.Int16, Convert.ToInt16(ddlTipoCliente.SelectedValue));
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação atualizada com sucesso!";
                    txtTitulo.Text = "";
                    txtConteudo.Text = "";
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
            txtTitulo.Text = "";
            txtConteudo.Text = "";
            lblMensagem.Text = "";
            txtTitulo.Focus();
        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select * from contrato where descricao like '%" + txtBuscar.Text+"%'";
            gdvDados.DataBind();
        }
    }
}