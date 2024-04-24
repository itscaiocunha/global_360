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
    public partial class printpedido : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            hdfId.Value = Request.QueryString["id"].ToString();
            //traz o conteudo do contrato para salvar no cadastro final
            using (IDataReader reader = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                              "select p.id, p.valor, p.rastreio, p.prazo_entrega, p.data_entrega, FORMAT(p.DATACADASTRO,'dd/MM/yyyy') as datacadastro, p.[status], c1.nomecompleto as nomefantasia, 'CEP: '+ c1.cep+' - '+ c1.cidade+'/'+c1.estado as local_lojista, c1.celular as telefonelojista, c2.nomecompleto as nomecliente, 'CEP: '+ c2.CEP+' - '+ c2.endereco+' '+c2.bairro+' '+c2.numero+','+c2.cidade+'/'+c2.estado as enderecocliente from pedido p join cliente c1 on c1.id = p.idlojista join cliente c2 on c2.id = p.idconsumidor where p.id = '" + hdfId.Value + "'"))
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
                }
            }
        }        
    }
}