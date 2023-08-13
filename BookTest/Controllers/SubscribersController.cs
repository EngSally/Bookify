using BookTest.Core.Models;
using BookTest.Core.ViewModels.Subscribers;
using BookTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookTest.Controllers
{
    [Authorize(Roles = AppRole.Reception)]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public SubscribersController(ApplicationDbContext dbContext, IImageService imageService, IMapper mapper)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _mapper = mapper;
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
        [ValidateAntiForgeryToken]
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
          

            _dbContext.Subscribers.Add(subscriber);
            _dbContext.SaveChanges();
			return RedirectToAction(nameof(Details), new { id = subscriber.Id });
		}




        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subscriber=_dbContext.Subscribers.Find(id);
            if (subscriber is null) return NotFound();
            SubscriberFormViewModel model= _mapper.Map<SubscriberFormViewModel>(subscriber);
            model.Governorates = _dbContext.Governorates.Select(g => new SelectListItem(g.Name, g.Id.ToString())).ToList();
            return View("Form", PopulateViewModel(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> Edit(SubscriberFormViewModel model)
        {
            var subscriber = _dbContext.Subscribers.Find(model.Id);
            if (subscriber is null) return NotFound();
            if (subscriber.ImageUrl != null)
            {
                _imageService.Delete(subscriber.ImageUrl, subscriber.ImageUrlThumbnail);
            }
            subscriber = _mapper.Map(model, subscriber);

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
            subscriber.LastUpdateOn = DateTime.Now;
            _dbContext.SaveChanges();
			return RedirectToAction(nameof(Details), new { id = subscriber.Id });
		}

        [AjaxOnly]
        [HttpPost]
        public IActionResult LoadAreas(int governorateId)
        {
            var areas=  _dbContext.Areas.Where(a=>a.GovernorateId==governorateId).ToList();
            return Json(areas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Search(SearchFormViewModel model)
        {
			var subscriber = _dbContext.Subscribers
							.SingleOrDefault(s =>
									s.Email == model.Value
								|| s.NationalId == model.Value
								|| s.MobilNum == model.Value);
			var viewModel=_mapper.Map<SubscriberSearchResultViewModel>(subscriber);
            return PartialView("_Result", viewModel);
        }

        public IActionResult Details(int  id)
        {
            var subscriber = _dbContext.Subscribers
                .Include(s=>s.Area)
                .Include(s=>s.Governorate).FirstOrDefault(s=>s.Id==id);
            if (subscriber is null) return NotFound();
            var viewModel=_mapper.Map<SubscriberViewModel>(subscriber);
            return View("Details",viewModel);
        }

        public IActionResult AllowMobilNum(SubscriberFormViewModel model)
        {
            var sub=_dbContext.Subscribers.SingleOrDefault(s=>s.MobilNum==  model.MobilNum);
            var allow=sub is null || sub.Id==model.Id;
            return Json(allow);

        }


     
        public IActionResult AllowNationalId(SubscriberFormViewModel model)
        {
            var sub=_dbContext.Subscribers.SingleOrDefault(s=>s.NationalId==  model.NationalId);
            var allow=sub is null || sub.Id==model.Id;
            return Json(allow);

        }

      
        public IActionResult AllowEmail(SubscriberFormViewModel model)
        {
            var sub=_dbContext.Subscribers.SingleOrDefault(s=>s.Email==  model.Email);
            var allow=sub is null || sub.Id==model.Id;
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
