using DataTrackingSystem.Data.ViewModel.Input;
using DataTrackingSystem.Service.IService;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;

namespace DataTrackingSystem.Service
{
    public class EmailService : IEmailService

    {
        IConfiguration _config;
        public EmailService(IConfiguration config) { 
            _config = config;
        }
        public void SendEmail(MailRequest message)
        {
                Send(message);

        }
        private void Send(MailRequest message )
        {
            var FilePath = Directory.GetCurrentDirectory() + "\\Template\\Template.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[senderUserName]", message.senderUserName).Replace("[receiverUserName]", message.receiverUserName)
                .Replace("[date]", DateTime.UtcNow.ToString()).Replace("[time]", DateTime.Now.ToShortTimeString()).Replace("[receiverId]", message.ReceiverId);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_config.GetSection("EmailConfiguration:From").Value));
            emailMessage.To.Add(MailboxAddress.Parse(message.ToEmail));
            emailMessage.Subject = $"Welcome {message.receiverUserName}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            emailMessage.Body = builder.ToMessageBody();
            using var client = new SmtpClient();
                try
                {
                    client.Connect(_config.GetValue<string>("EmailConfiguration:SmtpServer"), _config.GetValue<int>("EmailConfiguration:Port"), SecureSocketOptions.StartTls);
                    client.Authenticate(_config.GetValue<string>("EmailConfiguration:Username"), _config.GetValue<string>("EmailConfiguration:Password"));
                    client.Send(emailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
               
            }
        }
}
