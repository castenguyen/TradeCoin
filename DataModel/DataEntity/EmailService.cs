using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace DataModel.DataEntity
{
    public class EmailService : IIdentityMessageService
    {
        #region methods
      //  public static string PageName = "HQDN";
       // public static string EmailAdminSMTP = "smtp.gmail.com";
       //  public static string EmailAdmin = "botogiasi@gmail.com";
       // public static string EmailAdminPassword = "Botogiasi2016";
       // public static int Portmail = 587;

        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
        public async Task SendAsync(IdentityMessage message, string CredentialUserName,
            string SentFrom, string Password, string SMTPServer, int Port, bool SSLEnable)
        {
            // Plug in your email service here to send an email.
            var result = Task.FromResult(0);
            try
            {
                SmtpClient client = new SmtpClient(SMTPServer);
                client.Port = Port;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(CredentialUserName, Password);
                client.EnableSsl = SSLEnable;
                client.Credentials = credentials;
                var mail = new System.Net.Mail.MailMessage(SentFrom, message.Destination);
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true;
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }


    //public class GmailEmailService : IEmailService
    //{
    //    private readonly SmtpConfiguration _config;

    //    private const string GmailUserNameKey = "GmailUserName";
    //    private const string GmailPasswordKey = "GmailPassword";
    //    private const string GmailHostKey = "GmailHost";
    //    private const string GmailPortKey = "GmailPort";
    //    private const string GmailSslKey = "GmailSsl";

    //    public GmailEmailService()
    //    {
    //        _config = new SmtpConfiguration();
    //        var gmailUserName = ConfigurationManager.AppSettings[GmailUserNameKey];
    //        var gmailPassword = ConfigurationManager.AppSettings[GmailPasswordKey];
    //        var gmailHost = ConfigurationManager.AppSettings[GmailHostKey];
    //        var gmailPort = Int32.Parse(ConfigurationManager.AppSettings[GmailPortKey]);
    //        var gmailSsl = Boolean.Parse(ConfigurationManager.AppSettings[GmailSslKey]);
    //        _config.Username = gmailUserName;
    //        _config.Password = gmailPassword;
    //        _config.Host = gmailHost;
    //        _config.Port = gmailPort;
    //        _config.Ssl = gmailSsl;
    //    }

    //    public bool SendEmailMessage(EmailMessage message)
    //    {
    //        var success = false;
    //        try
    //        {
    //            var smtp = new SmtpClient
    //            {
    //                Host = _config.Host,
    //                Port = _config.Port,
    //                EnableSsl = _config.Ssl,
    //                DeliveryMethod = SmtpDeliveryMethod.Network,
    //                UseDefaultCredentials = false,
    //                Credentials = new NetworkCredential(_config.Username, _config.Password)
    //            };

    //            using (var smtpMessage = new MailMessage(_config.Username, message.ToEmail))
    //            {
    //                smtpMessage.Subject = message.Subject;
    //                smtpMessage.Body = message.Body;
    //                smtpMessage.IsBodyHtml = message.IsHtml;
    //                smtp.Send(smtpMessage);
    //            }

    //            success = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            //todo: add logging integration
    //            //throw;
    //        }

    //        return success;
    //    }
    //}
}
