using BookTest.Core.Models;
using BookTest.Core.ViewModels.Subscribers;
using BookTest.Services;
using Hangfire;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BookTest.Controllers
{
    [Authorize(Roles = AppRole.Reception)]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDataProtector _dataProtector;
        private readonly IImageService _imageService;
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public SubscribersController(ApplicationDbContext dbContext, IDataProtectionProvider dataProtector, IImageService imageService, IMapper mapper, IEmailSender emailSender, IEmailBodyBuilder emailBodyBuilder)
        {
            _dbContext = dbContext;
            _dataProtector = dataProtector.CreateProtector("MySecureKey");
            _imageService = imageService;
            _emailSender = emailSender;
            _mapper = mapper;
            _emailBodyBuilder = emailBodyBuilder;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model= new SubscriberFormViewModel();

            return View("Form", PopulateViewModel(model));
        }

        [HttpPost]
                         
        public async Task<IActionResult> Create(SubscriberFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", PopulateViewModel(model));
            }
            if (model.Image is null)
            {
                ModelState.AddModelError("Image", Errors.Required);
                return View("Form", PopulateViewModel(model));
            }
            Subscriber   subscriber=_mapper.Map<Subscriber>(model);
            string extension=Path.GetExtension(model.Image.FileName.ToLower());
            string imageName=$"{Guid.NewGuid()}{extension}";
            string imagPath="/images/subscribers";
            var (isUploded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagPath, hasThumbnail: true);
            if (!isUploded)
            {
                ModelState.AddModelError(nameof(model.Image), errorMessage!);
                return View("Form", PopulateViewModel(model));
            }
            subscriber.ImageUrl = $"{imagPath}/{imageName}";
            subscriber.ImageUrlThumbnail = $"{imagPath}/thumb/{imageName}";
            subscriber.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            RenewalSubscribtion renewal=new()
            {
                 CreatedById=subscriber.CreatedById,
                 CreatedOn=subscriber.CreatedOn,
                 StartDate=DateTime.Today,
                 EndDate=DateTime.Today.AddYears(1)
            };
            subscriber.RenewalSubscribtions.Add(renewal);
            _dbContext.Subscribers.Add(subscriber);
            _dbContext.SaveChanges();
            string key=_dataProtector.Protect(subscriber.Id.ToString());
            //Send welcome email
            var placeholders = new Dictionary<string, string>()
            {
                { "imageUrl", "https://res.cloudinary.com/devcreed/image/upload/v1668739431/icon-positive-vote-2_jcxdww.svg" },
                { "header", $"Welcome {model.FristName}," },
                { "body", "thanks for joining Bookify 🤩" }
            };

            var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Notification, placeholders);

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                model.Email,
                "Welcome to Bookify", body));

            
            //send  whatsapp massage

            return RedirectToAction(nameof(Details), new { id = key });
		}




        [HttpGet]
        public IActionResult Edit(string id)
        {
            var subscriberId=_dataProtector.Unprotect(id);
            if (subscriberId.Length == 0)
                return NotFound();
            var subscriber=_dbContext.Subscribers.Find(int.Parse(subscriberId));
            if (subscriber is null) return NotFound();
            SubscriberFormViewModel model= _mapper.Map<SubscriberFormViewModel>(subscriber);
			model.Key= id;
            			model.Governorates = _dbContext.Governorates.Select(g => new SelectListItem(g.Name, g.Id.ToString())).ToList();
            return View("Form", PopulateViewModel(model));
        }

        [HttpPost]
                         
      
        public async Task<IActionResult> Edit(SubscriberFormViewModel model)
        {
            var subscriberId=_dataProtector.Unprotect(model.Key!);
            if (subscriberId.Length == 0)
                return NotFound();
          
			var subscriber = _dbContext.Subscribers.Find(int.Parse(subscriberId));
            if (subscriber is null) return NotFound();
            if (model.Image != null)
            {
                _imageService.Delete(subscriber.ImageUrl!, subscriber.ImageUrlThumbnail);
                string extension=Path.GetExtension(model.Image.FileName.ToLower());
                string imageName=$"{Guid.NewGuid()}{extension}";
                string imagPath="/images/subscribers";
                var (isUploded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagPath, hasThumbnail: true);
                if (!isUploded)
                {
                    ModelState.AddModelError(nameof(model.Image), errorMessage!);
                    return View("Form", PopulateViewModel(model));
                }
                model.ImageUrl = $"{imagPath}/{imageName}";
                model.ImageUrlThumbnail = $"{imagPath}/thumb/{imageName}";
            }

            else if (!string.IsNullOrEmpty(subscriber.ImageUrl))
            {
                model.ImageUrl = subscriber.ImageUrl;
                model.ImageUrlThumbnail = subscriber.ImageUrlThumbnail;
            }
            subscriber = _mapper.Map(model, subscriber);
            subscriber.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            subscriber.LastUpdateOn = DateTime.Now;

            _dbContext.SaveChanges();
			return RedirectToAction(nameof(Details), new { id = model.Key });
		}



        [HttpPost]
                         
        public IActionResult RenewalSubscribtion(string key)
        {
            var subscriberId=_dataProtector.Unprotect(key);
            if (subscriberId.Length == 0)
                return NotFound();
       
            var subscriber=_dbContext.Subscribers
                .Include(s=>s.RenewalSubscribtions)
                .FirstOrDefault(s=>s.Id==int.Parse(subscriberId));
            if (subscriber is null) return NotFound();
            if (subscriber.IsBlackListed) return BadRequest();
            DateTime start;
            if (!subscriber.RenewalSubscribtions.Any())
            {
                start = DateTime.Now;
            }
            else
            {
                start =
                               subscriber.RenewalSubscribtions.Last().EndDate < DateTime.Today ?
                               DateTime.Today :
                               subscriber.RenewalSubscribtions.Last().EndDate.AddDays(1);

            }
            RenewalSubscribtion renewal=new()
            {
                CreatedOn= DateTime.Now,
                CreatedById=User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                StartDate= start,
                EndDate= start.AddYears(1)
            };
            subscriber.RenewalSubscribtions.Add(renewal);
            _dbContext.SaveChanges();
            //TODO Send Email And WhatsApp Message
            //Send email and WhatsApp Message
            var placeholders = new Dictionary<string, string>()
            {
                { "imageUrl", "https://res.cloudinary.com/devcreed/image/upload/v1668739431/icon-positive-vote-2_jcxdww.svg" },
                { "header", $"Hello {subscriber.FristName}," },
                { "body", $"your subscription has been renewed through {renewal.EndDate.ToString("d MMM, yyyy")} 🎉🎉" }
            };

            var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Notification, placeholders);

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                subscriber.Email,
                "Bookify Subscription Renewal", body));
          //whats app message
            
            var renewalSubscribtionViewModel=_mapper.Map<RenewalSubscribtionViewModel>(renewal);
            return PartialView("_RenewalSubscribtionRow", renewalSubscribtionViewModel);

        }



       


        [AjaxOnly]
        [HttpPost]
        public IActionResult LoadAreas(int governorateId)
        {
            var areas=  _dbContext.Areas.Where(a=>a.GovernorateId==governorateId).ToList();
            return Json(areas);
        }

        [HttpPost]
                         
        public  IActionResult Search(SearchFormViewModel model)
        {
			var subscriber = _dbContext.Subscribers
							.SingleOrDefault(s =>
									s.Email == model.Value
								|| s.NationalId == model.Value
								|| s.MobilNum == model.Value);
			var viewModel=_mapper.Map<SubscriberSearchResultViewModel>(subscriber);
           if(subscriber is not null) 
            viewModel.key = _dataProtector.Protect(subscriber.Id.ToString());
            return PartialView("_Result", viewModel);
        }

        public IActionResult Details(string   id)
        {
            int subscriberId=int.Parse(_dataProtector.Unprotect(id));
       	var subscriber = _dbContext.Subscribers
                .Include(s=>s.Area)
                .Include(s=>s.Governorate)
                .Include(s=> s.RenewalSubscribtions)
                .Include(s=>s.Rentals)
                .ThenInclude(r=>r.RentalCopies)
                .FirstOrDefault(s=>s.Id==subscriberId )
                
                ;
            if (subscriber is null) return NotFound();
            var viewModel=_mapper.Map<SubscriberViewModel>(subscriber);
            viewModel.Key = id;
            return View("Details",viewModel);
        }

        public IActionResult AllowMobilNum(SubscriberFormViewModel model)
        {
            int id=0;
            if(!string.IsNullOrEmpty(model.Key))
            {
                id = int.Parse(_dataProtector.Unprotect(model.Key));
            }
            var sub=_dbContext.Subscribers.SingleOrDefault(s=>s.MobilNum==  model.MobilNum);
            var allow=sub is null || sub.Id==id;
            return Json(allow);

        }


     
        public IActionResult AllowNationalId(SubscriberFormViewModel model)
        {
			int id=0;
			if (!string.IsNullOrEmpty(model.Key))
			{
                id = int.Parse(_dataProtector.Unprotect(model.Key));
            }
			var sub=_dbContext.Subscribers.SingleOrDefault(s=>s.NationalId==  model.NationalId);
            var allow=sub is null || sub.Id==id;
            return Json(allow);

        }

      
        public IActionResult AllowEmail(SubscriberFormViewModel model)
        {
			int id=0;
			if (!string.IsNullOrEmpty(model.Key))
			{
				id = int.Parse( _dataProtector.Unprotect(model.Key));
			}
			var sub=_dbContext.Subscribers.SingleOrDefault(s=>s.Email==  model.Email);
            var allow=sub is null || sub.Id==id;
            return Json(allow);

        }

        private   SubscriberFormViewModel PopulateViewModel (SubscriberFormViewModel model)
        {
            model.Governorates = model.Governorates = _dbContext.Governorates
                .Where(g=>!g.Deleted)
                .Select(g => new SelectListItem(g.Name, g.Id.ToString())).ToList();
            if(model.GovernorateId!=0)
            {
                model.Areas = _dbContext.Areas
                    .Where(a => a.GovernorateId == model.GovernorateId &&!a.Deleted)
                    .Select(g => new SelectListItem(g.Name, g.Id.ToString())).ToList();
            }

            return model;
        }
    }
}
