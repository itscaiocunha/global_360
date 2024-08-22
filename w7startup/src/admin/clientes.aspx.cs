
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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Web.UI.WebControls;

namespace global
{
    public partial class clientes : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ConnectionString");

                if (!string.IsNullOrEmpty(hdfId.Value))
                {
                    // Atualizar registro existente
                    string updateQuery = "update cliente set nomecompleto = @nome, cnpj_cpf = @cpf, rg = @rg, celular = @celular, email = @email, cep = @cep, endereco = @endereco, numero = @numero, bairro = @bairro, complemento = @complemento, estado = @estado, cidade = @cidade, status = @status where id = @id";
                    DbCommand updateCommand = db.GetSqlStringCommand(updateQuery);

                    db.AddInParameter(updateCommand, "@nome", DbType.String, txtNomeCliente.Text);
                    db.AddInParameter(updateCommand, "@cpf", DbType.String, txtCPFCNPJ.Text);
                    db.AddInParameter(updateCommand, "@rg", DbType.String, txtRG.Text);
                    db.AddInParameter(updateCommand, "@celular", DbType.String, txtCelular.Text);
                    db.AddInParameter(updateCommand, "@email", DbType.String, txtEmail.Text);
                    db.AddInParameter(updateCommand, "@cep", DbType.String, txtCEP.Text);
                    db.AddInParameter(updateCommand, "@endereco", DbType.String, txtEndereco.Text);
                    db.AddInParameter(updateCommand, "@numero", DbType.String, txtNum.Text);
                    db.AddInParameter(updateCommand, "@bairro", DbType.String, txtBairro.Text);
                    db.AddInParameter(updateCommand, "@complemento", DbType.String, txtComplemento.Text);
                    db.AddInParameter(updateCommand, "@estado", DbType.String, ddlUF.SelectedValue);
                    db.AddInParameter(updateCommand, "@cidade", DbType.String, txtCidade.Text);
                    db.AddInParameter(updateCommand, "@status", DbType.String, ddlStatus.SelectedValue);
                    db.AddInParameter(updateCommand, "@id", DbType.Int32, Convert.ToInt32(hdfId.Value));

                    db.ExecuteNonQuery(updateCommand);
                    lblMensagem.Text = "Atualizado com sucesso!";
                }
                else
                {
                    // Adicionar novo registro
                    string insertQuery = "insert into cliente (cnpj_cpf, email, celular, nomecompleto, cep, endereco, bairro, numero, cidade, estado, complemento, idtipocliente, [status]) values (@cpf, @email, @celular, @nome, @cep, @endereco, @bairro, @numero, @cidade, @estado, @complemento, @tipo @status))";
                    DbCommand insertCommand = db.GetSqlStringCommand(insertQuery);

                    db.AddInParameter(insertCommand, "@nome", DbType.String, txtNomeCliente.Text);
                    db.AddInParameter(insertCommand, "@cpf", DbType.String, txtCPFCNPJ.Text);
                    db.AddInParameter(insertCommand, "@rg", DbType.String, txtRG.Text);
                    db.AddInParameter(insertCommand, "@celular", DbType.String, txtCelular.Text);
                    db.AddInParameter(insertCommand, "@email", DbType.String, txtEmail.Text);
                    db.AddInParameter(insertCommand, "@cep", DbType.String, txtCEP.Text);
                    db.AddInParameter(insertCommand, "@endereco", DbType.String, txtEndereco.Text);
                    db.AddInParameter(insertCommand, "@numero", DbType.String, txtNum.Text);
                    db.AddInParameter(insertCommand, "@bairro", DbType.String, txtBairro.Text);
                    db.AddInParameter(insertCommand, "@complemento", DbType.String, txtComplemento.Text);
                    db.AddInParameter(insertCommand, "@tipo", DbType.String, 3);
                    db.AddInParameter(insertCommand, "@estado", DbType.String, ddlUF.SelectedValue);
                    db.AddInParameter(insertCommand, "@cidade", DbType.String, txtCidade.Text);
                    db.AddInParameter(insertCommand, "@status", DbType.String, ddlStatus.SelectedValue);

                    db.ExecuteNonQuery(insertCommand);
                    lblMensagem.Text = "Adicionado com sucesso!";
                }

                LimparCampos();
                hdfId.Value = string.Empty;

                gdvDados.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "$('#" + pnlModal.ClientID + "').modal('hide');", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao salvar: " + ex.Message;
            }
        }

        private void LimparCampos()
        {
            txtNomeCliente.Text = string.Empty;
            txtCPFCNPJ.Text = string.Empty;
            txtRG.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtCidade.Text = string.Empty;
            ddlUF.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select id, cnpj_cpf as cpf, email, celular, nomecompleto, cidade, estado from cliente where idtipocliente = 3 and nomecompleto like '%" + txtBuscar.Text + "%' order by nomecompleto";
            BindData();
        }

        protected void gdvDados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                EditarRegistro(id);
            }
        }


        private void EditarRegistro(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            string query = "select id, nomecompleto, cnpj_cpf as cpf, rg, celular, email, cep, endereco, numero, bairro, complemento, estado, cidade, status from cliente where idtipocliente = 3 and id = @id";
            DbCommand cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet ds = db.ExecuteDataSet(cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                hdfId.Value = dr["id"].ToString();
                txtNomeCliente.Text = dr["nomecompleto"].ToString();
                txtCPFCNPJ.Text = dr["cpf"].ToString();
                txtRG.Text = dr["rg"].ToString();
                txtCelular.Text = dr["celular"].ToString();
                txtEmail.Text = dr["email"].ToString();
                txtCEP.Text = dr["cep"].ToString();
                txtEndereco.Text = dr["endereco"].ToString();
                txtNum.Text = dr["numero"].ToString();
                txtBairro.Text = dr["bairro"].ToString();
                txtComplemento.Text = dr["complemento"].ToString();
                txtCidade.Text = dr["cidade"].ToString();
                ddlUF.SelectedValue = dr["estado"].ToString();
                ddlStatus.SelectedValue = "Ativo";

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenModal", "$('#" + pnlModal.ClientID + "').modal('show');", true);
            }
        }

        private void BindData()
        {
            gdvDados.DataBind();
        }
    }
}