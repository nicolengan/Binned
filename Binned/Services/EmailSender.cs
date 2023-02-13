using Binned.Model;
using Binned.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection.PortableExecutable;

namespace Binned.Services
{

    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _configuration;
        private readonly ILogger _logger;
        public EmailSender(ILogger<EmailSender> logger, EmailSettings configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async System.Threading.Tasks.Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (!Configuration.Default.ApiKey.ContainsKey("api-key"))
            {
                Configuration.Default.ApiKey.Add("api-key", _configuration.API);
            }

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Binned";
            string SenderEmail = "Binned@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

            string ToEmail = toEmail;
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);

            string Subject = subject;
            string HtmlContent = message;

            try
            {
                var sendSmtpEmail = new SendSmtpEmail(Email, To, null, null, HtmlContent, null, Subject, null, null, null, null, null, null, null);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Debug.WriteLine(result.ToJson());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }
    }
}
