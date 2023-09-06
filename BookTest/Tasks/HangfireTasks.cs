using BookTest.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

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
    }
}
