using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace global
{
    public class Email
    {
        //Adicionar a classe abaixo, senão o AutoDiscoverUrl retorna erro
        private static bool ValidateRedirectionUrlCallback(string url)
        {
            if (url.Substring(0, 8) == "https://")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private const string SmtpServer = "smtp.gmail.com";
        private const string MailserverLogin = "w7naoresponda@gmail.com";
        private const string MailServerPassword = "Sqlw7@22w7";
        private const string MailUserName = "Developer tips support";

        public static void emailTeste()
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("w7naoresponda@gmail.com", "qofy rbrd yriu qazd");

            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("w7naoresponda@gmail.com", "Plataforma Digital");
            mail.From = new MailAddress("w7naoresponda@gmail.com", "Plataforma Digital");
            mail.To.Add(new MailAddress("contato@w7agencia.com.br", "contato@w7agencia.com.br"));
            mail.Subject = "Test Support";
            mail.Body = "Mensagem teste";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                //trata erro
            }
            finally
            {
                mail = null;
            }
        }


        public static void emailTxt(string de, string para, string cc, string cco, string assunto, string mensagem, int prioridade)
        {
            Database db = DatabaseFactory.CreateDatabase("ConnectionString");

            DbCommand command = db.GetSqlStringCommand(
            "INSERT INTO Email_Disparo (Email_de,Email_Para,Email_Assunto,Email_Corpo,Email_data_gravacao,Email_data_envio,Email_Tentativa_Envio,Email_Enviado) " +
                    " Values (@Email_de,@Email_Para,@Email_Assunto,@Email_Corpo,@Email_data_gravacao,@Email_data_envio,1,0)");
            db.AddInParameter(command, "@Email_Para", DbType.String, para);
            db.AddInParameter(command, "@Email_De", DbType.String, "w7naoresponda@gmail.com");
            db.AddInParameter(command, "@Email_Assunto", DbType.String, assunto);
            db.AddInParameter(command, "@Email_Corpo", DbType.String, mensagem);
            db.AddInParameter(command, "@Email_data_gravacao", DbType.DateTime, DateTime.Now);
            db.AddInParameter(command, "@Email_data_envio", DbType.DateTime, DateTime.Now);
            //db.ExecuteNonQuery(command);

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("w7naoresponda@gmail.com", "qofy rbrd yriu qazd");

            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress("w7naoresponda@gmail.com", "Global 360");
            mail.From = new MailAddress("w7naoresponda@gmail.com", "Global 360");
            mail.To.Add(new MailAddress(para, para));
            mail.Subject = assunto;
            mail.Body = mensagem;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                //trata erro
            }
            finally
            {
                mail = null;
            }
        }
    }
}