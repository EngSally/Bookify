using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookTest.Tasks
{
	public class HangfireTasks
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IEmailBodyBuilder _emailBodyBuilder;
		private readonly IEmailSender _emailSender;
		private readonly IWebHostEnvironment _webHostEnvironment;
		// private readonly IWhatsAppClient _whatsAppClient;

		public HangfireTasks(ApplicationDbContext dbContext, IEmailBodyBuilder emailBodyBuilder, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
		{
			_dbContext = dbContext;
			_emailBodyBuilder = emailBodyBuilder;
			_emailSender = emailSender;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task PrepareExpirationAlert()
		{
			var  subscribers=_dbContext.Subscribers
				.Include(s=>s.RenewalSubscribtions)
				.Where
				  (        s=>!s.IsBlackListed &&  s.RenewalSubscribtions
							.OrderByDescending(r=>r.EndDate)
							.First()
							.EndDate==DateTime.Today.AddDays(5)
				)
				.ToList();

			string   endDate;
			foreach (var subscriber in subscribers)
			{
				endDate = subscriber.RenewalSubscribtions.Last().EndDate.ToString("d MMM, yyyy");
				var placeholders = new Dictionary<string, string>()
						{
								  { "imageUrl", "https://res.cloudinary.com/devcreed/image/upload/v1671062674/calendar_zfohjc.png" },
								  { "header", $"Hello {subscriber.FristName}," },
								  { "body", $"your subscription will be expired by {endDate} 🙁" }
				};

				var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Notification, placeholders);

				await _emailSender.SendEmailAsync(subscriber.Email, "Bookify Subscription Renewal", body);

			}
		}

		public async Task PrepareRentalEnd()
		{
			var tomorrow=DateTime.Today.AddDays(1);
			var rentals =_dbContext.Rentals
				.Include(r=>r.Subscriber)
				.Include(r=>r.RentalCopies)
				.ThenInclude(c=>c.BookCopy)
				.ThenInclude(b=>b.Book)
				.Where(r=>r.RentalCopies
							.Any( r=>!r.ReturnDate.HasValue
							&&r.EndDate==tomorrow))
				.ToList();
			foreach (var rental in rentals)
			{
				var expiredCopies = rental.RentalCopies.Where(c => c.EndDate.Date == tomorrow && !c.ReturnDate.HasValue).ToList();

				var message = $"your rental for the below book(s) will be expired by tomorrow {tomorrow.ToString("dd MMM, yyyy")} 💔:";
				message += "<ul>";

				foreach (var copy in expiredCopies)
				{
					message += $"<li>{copy.BookCopy!.Book!.Title}</li>";
				}

				message += "</ul>";

				var placeholders = new Dictionary<string, string>()
				{
					{ "imageUrl", "https://res.cloudinary.com/devcreed/image/upload/v1671062674/calendar_zfohjc.png" },
					{ "header", $"Hello {rental.Subscriber!.FristName}," },
					{ "body", message }
				};

				var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Notification, placeholders);

				await _emailSender.SendEmailAsync(
					rental.Subscriber!.Email,
					"Bookify Rental Expiration 🔔", body);
			}



		}
	}
}
