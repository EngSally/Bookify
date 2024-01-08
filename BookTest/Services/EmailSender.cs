using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BookTest.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly  MailSetting _mailSetting;
		public EmailSender(IWebHostEnvironment webHostEnvironment, IOptions<MailSetting> mailSetting)
		{
			_webHostEnvironment = webHostEnvironment;
			_mailSetting = mailSetting.Value;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			MailMessage message = new ()
			{
				From=new MailAddress (_mailSetting.Email!,_mailSetting.DisplayName),
				Subject=subject,
				Body = htmlMessage,
				IsBodyHtml=true
			};
			message.To.Add(_webHostEnvironment.IsDevelopment() ? "eng.sally.atalla@outlook.com" : email);
			SmtpClient smtpClient = new (_mailSetting.Host,_mailSetting.Port)
			{
				Credentials=new  NetworkCredential(_mailSetting.Email,_mailSetting.Password),
				EnableSsl=true

			};

			await smtpClient.SendMailAsync(message);
			smtpClient.Dispose();
		}
	}
}
