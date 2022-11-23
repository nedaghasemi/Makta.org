using Common;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _emailSettings;
        private readonly CommonSetting _commonSettings;
        private const string BaseFullLogPath = "wwwroot\\Logs";

        public EmailSender(EmailSetting emailSettings, CommonSetting commonSettings)
        {
            _emailSettings = emailSettings;
            _commonSettings = commonSettings;

            if (!Directory.Exists(BaseFullLogPath))
            {
                Directory.CreateDirectory(BaseFullLogPath);
            }
        }

        public async Task SendEmail(EmailSetting email, EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(email, emailMessage);
            //await Log(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(message.From);
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Content,
                TextBody = ""
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task Send(EmailSetting email, MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.CheckCertificateRevocation = false;

                await client.ConnectAsync(email.SmtpServer, email.Port, SecureSocketOptions.Auto);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(email.UserName, email.Password);
                await client.SendAsync(mailMessage);

                var fromaddress = mailMessage.From[0] as MailboxAddress;

                foreach (var item in mailMessage.To)
                {
                    var toAddress = item as MailboxAddress;
                    LogEmail("Email Sent successfully from: " + fromaddress.Address + " to: " + toAddress.Address);

                }

            }
            catch (Exception ex)
            {
                LogEmail(ex.Message);

            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        //--------------------

        public async Task SendNewSubscribe(string email)
        {
            try
            {
                MailboxAddress from = new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail);

                string subject = "New Makta subscriber";

                MailboxAddress to1 = new MailboxAddress(_commonSettings.ProductName + " Community", _commonSettings.AdminEmail);

                List<MailboxAddress> toAddresses = new List<MailboxAddress>
                {
                    to1
                };

                string body = "";
                body += "Dear <b>Makta Admin</b>,";
                body += $"<br>You have a new subscriber enrolled in :{DateTime.Now.ToString("yyyy-MM-dd hh:mm tt")}";
                body += $"<br>Email: <b>{email}</b>";
                body += $"<br><br>{_commonSettings.ProductName} Auto e-Mailing System";

                EmailMessage message = new EmailMessage(from, toAddresses, subject, body);

                await SendEmail(_emailSettings, message);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LogSystem(string logDescription)
        {
            await File.AppendAllTextAsync(BaseFullLogPath + "\\systemlog.txt", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " - " + logDescription + Environment.NewLine);
        }

        private void LogEmail(string logMessage)
        {
            File.AppendAllText(BaseFullLogPath + "\\Emaillog.txt", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " - " + logMessage + Environment.NewLine);
        }

        public async Task SendEmailtoAdmin(string body)
        {
            try
            {
                MailboxAddress from = new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail);

                string subject = "New Makta Contributer";

                MailboxAddress to1 = new MailboxAddress(_commonSettings.ProductName + " Community", _commonSettings.AdminEmail);

                List<MailboxAddress> toAddresses = new List<MailboxAddress>
                {
                    to1
                };

                string bodyText = "";
                bodyText += "Dear <b>Makta Admin</b>,";
                bodyText += $"<br>You have a new community member request at :{DateTime.Now.ToString("yyyy-MM-dd hh:mm tt")}";
                bodyText += $"<br>Info: <b>{body}</b>";
                bodyText += $"<br><br>{_commonSettings.ProductName} Auto e-Mailing System";

                EmailMessage message = new EmailMessage(from, toAddresses, subject, bodyText);

                await SendEmail(_emailSettings, message);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
