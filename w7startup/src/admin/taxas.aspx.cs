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

namespace global
{
    public partial class taxas : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {


        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            //aqui vai inserir um registro novo no sistema
            if (hdfId.Value == "")
            {
                DbCommand command = db.GetSqlStringCommand(
                "INSERT INTO cliente_taxa (idcliente, recorrente, valor, tipo, dia_pagamento, datacadastro) values (@idcliente, @recorrente, @valor, @tipo, @dia_pagamento, getDate())");
                db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(ddlDistribuidor.SelectedValue));
                db.AddInParameter(command, "@recorrente", DbType.String, ddlRecorrente.SelectedValue);
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(txtTaxa.Text));
                db.AddInParameter(command, "@tipo", DbType.String, ddlTipoTaxa.SelectedValue);
                db.AddInParameter(command, "@dia_pagamento", DbType.Int16, Convert.ToInt16(txtDiaPagamento.Text));
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação salva com sucesso!";
                    txtTaxa.Text = "";
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
               "UPDATE cliente_taxa SET recorrente = @recorrente, valor = @valor, tipo = @tipo, dia_pagamento = @dia_pagamento where id = @id");
                db.AddInParameter(command, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));                
                db.AddInParameter(command, "@recorrente", DbType.String, ddlRecorrente.SelectedValue);
                db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(txtTaxa.Text));
                db.AddInParameter(command, "@tipo", DbType.String, ddlTipoTaxa.SelectedValue);
                db.AddInParameter(command, "@dia_pagamento", DbType.Int16, Convert.ToInt16(txtDiaPagamento.Text));
                try
                {
                    db.ExecuteNonQuery(command);
                    lblMensagem.Text = "Informação atualizada com sucesso!";
                    txtTaxa.Text = "";
                    hdfId.Value = "";
                    ddlDistribuidor.Enabled = true;
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
            ddlDistribuidor.Enabled = true;
            txtTaxa.Text = "";
            lblMensagem.Text = "";
            txtTaxa.Focus();
        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select ct.id, cnpj_cpf, razao_social, nomecompleto, celular, cidade, estado as uf, valor, tipo, recorrente from cliente_taxa ct join cliente c on c.id= ct.idcliente where c.nomecompleto like '%" + txtBuscar.Text + "%'";
            gdvDados.DataBind();
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            hdfId.Value = e.CommandArgument.ToString();
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                          "SELECT * from cliente_taxa where id = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    txtTaxa.Text = reader["valor"].ToString();
                    ddlRecorrente.SelectedValue = reader["recorrente"].ToString();
                    ddlTipoTaxa.SelectedValue = reader["tipo"].ToString();
                    ddlDistribuidor.SelectedValue = reader["idcliente"].ToString();
                    txtDiaPagamento.Text = reader["dia_pagamento"].ToString();
                    ddlDistribuidor.Enabled = false;
                    txtTaxa.Focus();
                    pnlModal.Visible = true;
                    lblMensagem.Text = "";
                }
            }
        }
    }
}