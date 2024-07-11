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

namespace global
{
    public partial class profile : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select * from cliente where id = '" + Session["idcliente"].ToString() + "'"))
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
                        hplContrato.NavigateUrl = "../admin/viewcontrato.aspx?id=" + reader["id"].ToString() +"";
                    }
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
        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            DbCommand command = db.GetSqlStringCommand(
               "UPDATE cliente SET rg = @rg, celular = @celular, nomecompleto = @nomecompleto, cep = @cep, endereco = @endereco, bairro = @bairro, numero = @numero, cidade = @cidade, estado = @estado, complemento = @complemento where id = @id");
            db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
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

            try
            {
                db.ExecuteNonQuery(command);

                if (txtNovaSenha.Text != "")
                {
                    DbCommand command2 = db.GetSqlStringCommand(
               "UPDATE usuario SET senha = @senha where idcliente = @idcliente");
                    db.AddInParameter(command2, "@idcliente", DbType.Int16, Convert.ToInt16(Session["idcliente"].ToString()));
                    db.AddInParameter(command2, "@senha", DbType.String, Criptografia.Encrypt(txtNovaSenha.Text).Replace("+",""));

                    try
                    {
                        db.ExecuteNonQuery(command2);
                        lblMensagem.Text = "Dados atualizados com sucesso em nosso sistema.";
                    }
                    catch (Exception ex)
                    {
                        lblMensagem.Text = "Dados atualizado com sucesso, apenas a senha com erro. " + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao tentar atualizar os dados. " + ex.Message;
            }
        }
    }
}