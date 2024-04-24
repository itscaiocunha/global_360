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

namespace global.admin
{
    public partial class viewcontrato : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            hdfId.Value = Request.QueryString["id"].ToString();
            //traz o conteudo do contrato para salvar no cadastro final
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "SELECT * from cliente c join contrato t on t.idtipocliente = c.idtipocliente where c.token = '" + hdfId.Value + "'"))
            {
                if (reader.Read())
                {
                    lblContrato.Text = reader["conteudo"].ToString().Replace("{cnpj_cpf}", reader["cnpj_cpf"].ToString()).Replace("{nome_fantasia}", reader["nomecompleto"].ToString()).Replace("{email}", reader["email"].ToString()).Replace("{telefone}", reader["celular"].ToString()).Replace("{endereco}", reader["endereco"].ToString()).Replace("{bairro}", reader["bairro"].ToString()).Replace("{cep}", reader["cep"].ToString()).Replace("{numero}", reader["numero"].ToString()).Replace("{cidade}", reader["cidade"].ToString()).Replace("{estado}", reader["estado"].ToString()).Replace("{data_atual}", reader["datacadastro"].ToString()).Replace("{razao_social}", reader["razao_social"].ToString()).Replace("{inscricao_estadual}", reader["inscricao_estadual"].ToString().Replace("{rg}", reader["rg"].ToString()));
                }
            }
        }
   }
}