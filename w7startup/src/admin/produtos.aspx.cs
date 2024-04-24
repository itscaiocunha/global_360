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
using Azure.Storage.Blobs;

namespace global
{
    public partial class produtos : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {
            
        }

        public static async Task UploadFromFileAsync(
    BlobContainerClient containerClient,
    string localFilePath)
        {
            string fileName = Path.GetFileName(localFilePath);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(localFilePath, true);
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            hdfId.Value = e.CommandArgument.ToString();
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * from produto where id = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    txtNomeProduto.Text = reader["titulo"].ToString();
                    ddlCategoria.SelectedValue = reader["idcategoria"].ToString();
                    ddlStatus.SelectedValue = reader["status"].ToString();
                    txtDescricao.Text = reader["descricao"].ToString();
                    txtValor.Text = reader["valor"].ToString();
                    imgFoto.ImageUrl = reader["imagem"].ToString();
                    txtNomeProduto.Focus();
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
                "INSERT INTO produto (titulo, descricao, idcategoria, valor, imagem, status, datacadastro) values (@titulo, @descricao, @idcategoria, @valor, @imagem, @status, getDate())");
                db.AddInParameter(command, "@titulo", DbType.String, txtNomeProduto.Text);
                db.AddInParameter(command, "@descricao", DbType.String, txtDescricao.Text);
                db.AddInParameter(command, "@idcategoria", DbType.Int16, Convert.ToInt16(ddlCategoria.SelectedValue));
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(txtValor.Text.Replace(",",".")));
                if(fluImagem.FileName != "")
                    db.AddInParameter(command, "@imagem", DbType.String, fluImagem.FileName);
                else
                db.AddInParameter(command, "@imagem", DbType.String, "http://global360.app.br/" + imgFoto.ImageUrl.Replace("~/",""));
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação salva com sucesso!";
                    txtNomeProduto.Text = "";
                    txtDescricao.Text = "";
                    txtValor.Text = "";
                    gdvDados.DataBind();
                    pnlModal.Visible = false;
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = "Erro ao tentar salvar informação. " + ex.Message;
                }
            }
            //aqui vai editar um registro dentro do sistema
            else
            {
                DbCommand command = db.GetSqlStringCommand(
               "UPDATE produto SET titulo = @titulo, descricao = @descricao, idcategoria = @idcategoria, valor = @valor, imagem = @imagem, status = @status where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
                db.AddInParameter(command, "@titulo", DbType.String, txtNomeProduto.Text);
                db.AddInParameter(command, "@descricao", DbType.String, txtDescricao.Text);
                db.AddInParameter(command, "@idcategoria", DbType.Int16, Convert.ToInt16(ddlCategoria.SelectedValue));
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(txtValor.Text));
                if(fluImagem.FileName != "")
                db.AddInParameter(command, "@imagem", DbType.String, "");
                else
                    db.AddInParameter(command, "@imagem", DbType.String, "http://global360.app.br/" + imgFoto.ImageUrl.Replace("~/", ""));
                db.AddInParameter(command, "@status", DbType.String, ddlStatus.SelectedValue);
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação atualizada com sucesso!";
                    txtNomeProduto.Text = "";
                    txtDescricao.Text = "";
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
            txtNomeProduto.Text = "";
            txtDescricao.Text = "";
            txtValor.Text = "";
            lblMensagem.Text = "";
            txtNomeProduto.Focus();
        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select * from produto where titulo like '%" + txtBuscar.Text + "%'";
            gdvDados.DataBind();
        }

    }
}