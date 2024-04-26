using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using RestSharp;

namespace global
{
    public class auth
    {
        public static bool VerificaCNPJ(string cnpj)
        {
            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select * from cliente where cnpj_cpf = '" + cnpj + "'"))
            {
                if (reader1.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool VerificaEmail(string email)
        {
            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select * from cliente where email = '" + email + "'"))
            {
                if (reader1.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string VerificaValor(string id)
        {
            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select valor from produto where id = '" + id + "'"))
            {
                if (reader1.Read())
                {
                    return reader1["valor"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        public static string VerificaCartao(string numero, string idcliente)
        {
            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select id from cartao where numero = '" + numero + "' and idcliente = '"+idcliente+"'"))
            {
                if (reader1.Read())
                {
                    return reader1["id"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        public static string RetornaTotalPedido(string id)
        {
            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select isnull(sum(valor),0) as total from pedido_produto where idpedido = '" + id + "'"))
            {
                if (reader1.Read())
                {
                    return reader1["total"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        public static string RetornaTaxaComissao(string id)
        {
            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select id from cliente_taxa where idcliente = '" + id + "'"))
            {
                if (reader1.Read())
                {
                    return reader1["id"].ToString();
                }
                else
                {
                    return "0";
                }
            }
        }

        public static void RotinaFaturas(string idpedido)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select * from pedido where id = '" + idpedido + "'"))
            {
                if (reader1.Read())
                {
                    //12 parcelas de fatura ao cliente 
                    for (int i = 0; i < 11; i++)
                    {  
                        DbCommand command = db.GetSqlStringCommand(
                                "INSERT INTO fatura (idpedido, status, datacadastro, valor, datavencimento) values (@idpedido, @status, getdate(), @valor, @datavencimento)");
                        db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(idpedido));
                        db.AddInParameter(command, "@status", DbType.String, "Aguardando Pagamento");
                        db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(reader1["valor"].ToString()));
                        db.AddInParameter(command, "@datavencimento", DbType.DateTime, DateTime.Now.AddMonths(i).Date);
                        db.ExecuteNonQuery(command);
                    }
                }
            }
        }

        public static void RotinaComissao(string idpedido)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            using (IDataReader reader1 = DatabaseFactory.CreateDatabase("ConnectionString").ExecuteReader(CommandType.Text,
                       "select ct.valor, ct.recorrente, c.cadastrado_por from pedido p join cliente c on c.id = p.idlojista join cliente_taxa ct on ct.idcliente = c.cadastrado_por where p.id = '" + idpedido + "' "))
            {
                if (reader1.Read())
                {
                    if (reader1["recorrente"].ToString() == "Não")
                    {
                        //12 parcelas de fatura ao cliente 
                        for (int i = 0; i < 11; i++)
                        {
                            DbCommand command = db.GetSqlStringCommand(
                                    "INSERT INTO comissao (idcliente, idpedido, status, datacadastro, valor, datavencimento) values (@idcliente, @idpedido, @status, getdate(), @valor, @datavencimento)");
                            db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(reader1["cadastrado_por"].ToString()));
                            db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(idpedido));
                            db.AddInParameter(command, "@status", DbType.String, "Aguardando Pagamento");
                            db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(reader1["valor"].ToString()));
                            db.AddInParameter(command, "@datavencimento", DbType.String, DateTime.Now.AddMonths(i).Date);
                            db.ExecuteNonQuery(command);
                        }
                    }
                    else
                    {
                        DbCommand command = db.GetSqlStringCommand(
                                    "INSERT INTO comissao (idcliente, idpedido, status, datacadastro, valor, datavencimento) values (@idcliente, @idpedido, @status, getdate(), @valor, @datavencimento)");
                        db.AddInParameter(command, "@idcliente", DbType.Int16, Convert.ToInt16(reader1["cadastrado_por"].ToString()));
                        db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(idpedido));
                        db.AddInParameter(command, "@status", DbType.String, "Aguardando Pagamento");
                        db.AddInParameter(command, "@valor", DbType.Double, Convert.ToDouble(reader1["valor"].ToString()));
                        db.AddInParameter(command, "@datavencimento", DbType.String, DateTime.Now.AddMonths(1).Date);
                        db.ExecuteNonQuery(command);
                    }
                }
            }
        }

        public static void InserirStatus(string id, string status)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand command = db.GetSqlStringCommand(
                    "INSERT INTO pedido_historico (idpedido, status, datacadastro) values (@idpedido, @status, getDate())");
            db.AddInParameter(command, "@idpedido", DbType.Int16, Convert.ToInt16(id));
            db.AddInParameter(command, "@status", DbType.String, status);
            db.ExecuteNonQuery(command);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumero(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GeraTokenAleatorio()
        {
            return RandomString(6);
        }
    }
}