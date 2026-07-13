using Microsoft.Extensions.Options;
using myshop.BLL.Interfaces;
using myshop.Models.Settings;
using System.Net;
using System.Net.Mail;

namespace myshop.BLL.Services
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;

		public MailService(IOptions<MailSettings> mailSettings)
			=> _mailSettings = mailSettings.Value;

		public async Task SendMailAsync(string mailTo, string subject, string body)
		{
			using var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
			};

			using var mailMessage = new MailMessage(
					from: _mailSettings.Email,
					to: mailTo,
					subject,
					body: $"<html><body>{body}</body></html>"
				);
			mailMessage.IsBodyHtml = true;

			await client.SendMailAsync(mailMessage);
		}
	}
}
