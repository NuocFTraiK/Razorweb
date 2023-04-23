using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CS54.Services
{
	public class EmailService : IEmailSender
	{
		private readonly EmailConfiguration _emailConfiguration;
		private readonly ILogger<EmailService> _logger;
		public EmailService(EmailConfiguration emailConfiguration, ILogger<EmailService> logger)
		{
			_emailConfiguration = emailConfiguration;
			_logger = logger;
			_logger.LogInformation("Create SendMailService");
		}


		

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var message = new MimeMessage();
			message.Sender = new MailboxAddress(_emailConfiguration.Username, _emailConfiguration.From);
			message.From.Add(new MailboxAddress(_emailConfiguration.Username, _emailConfiguration.From));
			message.To.Add(MailboxAddress.Parse(email));
			message.Subject = subject;

			var builder = new BodyBuilder();
			builder.HtmlBody = htmlMessage;
			message.Body = builder.ToMessageBody();

			// dùng SmtpClient của MailKit
			using var smtp = new MailKit.Net.Smtp.SmtpClient();

			try
			{
				smtp.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.StartTls);
				smtp.Authenticate(_emailConfiguration.From, _emailConfiguration.Password);
				await smtp.SendAsync(message);
			}
			catch (Exception ex)
			{
				// Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
				System.IO.Directory.CreateDirectory("mailssave");
				var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
				await message.WriteToAsync(emailsavefile);

				_logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
				_logger.LogError(ex.Message);
			}

			smtp.Disconnect(true);

			_logger.LogInformation("send mail to: " + email);

		}
	

		
	}
}
