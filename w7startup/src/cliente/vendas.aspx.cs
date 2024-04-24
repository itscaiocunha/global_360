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

namespace global.cliente
{
    public partial class vendas : System.Web.UI.Page
    {
        public static void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("novavenda.aspx", false);
        }

        protected void lkbFiltro_Click(object sender, EventArgs e)
        {
            sdsDados.SelectCommand = "select * from pedido p join cliente c on c.id = p.idcliente where nomecompleto like '%" + txtBuscar.Text + "%' or nome_fantasia like '%" + txtBuscar.Text + "%' or cnpj_cpf like '%" + txtBuscar.Text + "%' or p.id like '" + txtBuscar.Text +"'";
            gdvDados.DataBind();
        }

        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");
            DbCommand command3 = db.GetSqlStringCommand(
                        "update pedido set status = @status where id = @id");
            db.AddInParameter(command3, "@id", DbType.Int16, Convert.ToInt16(hdfId.Value));
            db.AddInParameter(command3, "@status", DbType.String, "Cancelado");
            try
            {
                db.ExecuteNonQuery(command3);

                auth.InserirStatus(hdfId.Value, "Cancelado");

                //aqui faz o cancelamento da assinatura na Iugu

                //envia o email ao cliente notificando


                lblConfirma.Text = "Aparelho/Pedido cancelado com sucesso. Seu cliente será notificado por e-mail.";
            }
            catch (Exception ex)
            {
                lblConfirma.Text = "Erro ao tentar cancelar. " + ex.Message;
            }
        }

        protected void gdvDados_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            pnlModal.Visible = true;
            lblMensagem.Text = "Deseja confirmar o cancelamento do pedido/aparelho?";
        }

        protected void lkbFechar_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
        }
    }
}