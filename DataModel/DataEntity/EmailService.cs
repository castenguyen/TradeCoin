using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DataModel.Extension;
using DataModel.DataStore;


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

        public async Task SendMailRandom(IdentityMessage message,int stt, List<string> lstEmail)
        {
            Ctrl cms_db = new Ctrl();

            try
            {
                if (stt == 0)
                {
                    await this.SendServiceCom(message, lstEmail);
                   // cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendServiceCom", "NcoinAPIController", stt.ToString());
                }
                else if (stt == 1)
                {

                    await this.SendNotifyCom(message, lstEmail);
                    //cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNotifyCom", "NcoinAPIController", stt.ToString());
                }
                else if (stt == 2)
                {
                    await this.SendNotifyTop(message, lstEmail);
                  //  cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNotifyTop", "NcoinAPIController", stt.ToString());
                   
                }
                else if (stt == 3)
                {
                    await this.SendServiceTop(message, lstEmail);
                   // cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendServiceTop", "NcoinAPIController", stt.ToString());
                  
                }
                else if (stt == 4)
                {
                    await this.SendNotifyXYZ(message, lstEmail);
                    //cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNotifyXYZ", "NcoinAPIController", stt.ToString());
                  
                }
                else if (stt ==5)
                {
                    await this.SendServiceXYZ(message, lstEmail);
                    //cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendServiceXYZ", "NcoinAPIController", stt.ToString());

                }
                else if (stt == 6)
                {
                    await this.SendNcoinXYZ(message, lstEmail);
                   // cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNcoinXYZ", "NcoinAPIController", stt.ToString());

                }
                else if (stt == 7)
                {
                    await this.SendBaoKeo(message, lstEmail);
                   // cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendBaoKeo", "NcoinAPIController", stt.ToString());

                }
                else if (stt == 8)
                {
                    await this.SendNcoinGmail(message, lstEmail);
                   // cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNcoinGmail", "NcoinAPIController", stt.ToString());

                }
                else if (stt == 9)
                {
                    await this.SendNcoinDem(message, lstEmail);
                    //cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNcoinDem", "NcoinAPIController", stt.ToString());

                }
                else if (stt == 10)
                {
                    await this.SendNcoinKimCuong(message, lstEmail);
                    //cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNcoinKimCuong", "NcoinAPIController", stt.ToString());

                }
                else if (stt == 11)
                {
                    await this.SendNcoinThongBao(message, lstEmail);
                  //  cms_db.AddToExceptionLog("SendMailRandom----> " + stt.ToString() + "SendNcoinThongBao", "NcoinAPIController", stt.ToString());

                }

            }
            catch (Exception ex)
            {
              
                cms_db.AddToExceptionLog("SendMailRandom----> "+ stt.ToString(), "NcoinAPIController", ex.ToString());
         
            }
        }

        public async Task SendAsync(IdentityMessage message, bool SSLEnable, List<string> lstEmail,string SMTP,string Email,string Pass,int Port)
        {
            Ctrl cms_db = new Ctrl();
            // Plug in your email service here to send an email.
            var result = Task.FromResult(0);
            try
            {
                SmtpClient client = new SmtpClient(SMTP);
                client.Port = Port;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(Email, Pass);
                client.EnableSsl = SSLEnable;
                client.Credentials = credentials;
                var mail = new System.Net.Mail.MailMessage(Email, message.Destination);

                if (lstEmail != null)
                {
                    foreach (string email in lstEmail)
                    {
                        mail.To.Add(email);
                    }
                }
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true;
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                cms_db.AddToExceptionLog("SendMailRandom----> " + ex.ToString() + "SendServiceCom", "NcoinAPIController", ex.ToString());
            }
        }







        /// <summary>
        /// noreply@service-ncoinclub.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendServiceCom(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
               await this.SendAsync(message,false, lstEmail, ConstantSystem.ServiceSMTP, 
                   ConstantSystem.ServiceAdmin, ConstantSystem.ServicePassword, ConstantSystem.ServicePort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// /noreply@notify-ncoinclub.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNotifyCom(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, false, lstEmail, ConstantSystem.NotifySMTP,
                    ConstantSystem.NotifyAdmin, ConstantSystem.NotifyPassword, ConstantSystem.NotifyPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// noreply@notify-ncoinclub.top
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNotifyTop(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, false, lstEmail, ConstantSystem.NotifyTopSMTP,
                    ConstantSystem.NotifyTopAdmin, ConstantSystem.NotifyTopPassword, ConstantSystem.NotifyTopPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// noreply@service-ncoinclub.top
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendServiceTop(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, false, lstEmail, ConstantSystem.ServiceTopSMTP,
                    ConstantSystem.ServiceTopAdmin, ConstantSystem.ServiceTopPassword, ConstantSystem.ServiceTopPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// /noreply@notify-ncoinclub.xyz
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNotifyXYZ(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, false, lstEmail, ConstantSystem.NotifyXYZSMTP,
                    ConstantSystem.NotifyXYZAdmin, ConstantSystem.NotifyXYZPassword, ConstantSystem.NotifyXYZPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// noreply@service-ncoinclub.xyz
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendServiceXYZ(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, false, lstEmail, ConstantSystem.ServiceXYZSMTP,
                    ConstantSystem.ServiceXYZAdmin, ConstantSystem.ServiceXYZPassword, ConstantSystem.ServiceXYZPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// /noreply@ncoinclub.xyz
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNcoinXYZ(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, false, lstEmail, ConstantSystem.NcoinXYZSMTP,
                    ConstantSystem.NcoinXYZAdmin, ConstantSystem.NcoinXYZPassword, ConstantSystem.NcoinXYZPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ncoinclubbaokeo@gmail.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendBaoKeo(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, true, lstEmail, ConstantSystem.Email_SMTP_1,
                    ConstantSystem.Email_User_1, ConstantSystem.Email_Password_1, ConstantSystem.Email_Port_1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// ncoinclub@gmail.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNcoinGmail(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, true, lstEmail, ConstantSystem.EmailAdminSMTP,
                    ConstantSystem.EmailAdmin, ConstantSystem.EmailAdminPassword, ConstantSystem.Portmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ncoinclubdem@gmail.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>

        public async Task SendNcoinDem(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, true, lstEmail, ConstantSystem.Email_SMTP_2,
                    ConstantSystem.Email_User_2, ConstantSystem.Email_Password_2, ConstantSystem.Email_Port_2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// /ncoinclubkimcuong@gmail.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNcoinKimCuong(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, true, lstEmail, ConstantSystem.Email_SMTP_3,
                    ConstantSystem.Email_User_3, ConstantSystem.Email_Password_3, ConstantSystem.Email_Port_3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// ncoinclubthongbao@gmail.com
        /// </summary>
        /// <param name="message"></param>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public async Task SendNcoinThongBao(IdentityMessage message, List<string> lstEmail)
        {
            try
            {
                await this.SendAsync(message, true, lstEmail, ConstantSystem.Email_SMTP_4,
                    ConstantSystem.Email_User_4, ConstantSystem.Email_Password_4, ConstantSystem.Email_Port_4);
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
