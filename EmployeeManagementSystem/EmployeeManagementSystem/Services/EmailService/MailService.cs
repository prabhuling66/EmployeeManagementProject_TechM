using EmployeeManagementSystem.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services.EmailService
{
    public class MailService : IMailService
    {
        private readonly SmtpModel _smtpmodel;

        public MailService(IOptions<SmtpModel> smtpModel)
        {
            _smtpmodel = smtpModel.Value;
        }
        public async Task SendEmailAsync(EmailModel emialModel)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_smtpmodel.From);
            email.To.Add(MailboxAddress.Parse(emialModel.ToEmail));
            email.Subject = emialModel.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = emialModel.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_smtpmodel.Host, _smtpmodel.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_smtpmodel.From, _smtpmodel.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
