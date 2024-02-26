
using Bookify.Domain.Entities;
using Bookify.Infrastructure.Services;
using Bookify.Web.Core.ViewModels.Books;
using CloudinaryDotNet;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Data;
using System.Linq.Dynamic.Core;

namespace Bookify.Web.Controllers
{
	[Authorize(Roles = AppRole.Archive)]
	public class BooksController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly Cloudinary _cloudinary;
		private  readonly IValidator<BooksFormViewModel>    _validator;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;
		private readonly IBooksService _booksService;
		private readonly IAuthorsService _authorsService;
		private readonly ICategoriesService _categoriesService;

		

		private readonly List<string> _allowedImageExtension=new(){ ".jpg",".jpeg",".png",".ico"};
		private readonly int _allowedSize=3145728;
        public BooksController( IMapper mapper
            , IWebHostEnvironment webHostEnvironment, IOptions<CloudinarySetting> cloudinarySetting, IImageService imageService,
			IValidator<BooksFormViewModel> validator, 
			IBooksService booksService,
            IAuthorsService authorsService,
			ICategoriesService categoriesService)
        {
          
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _imageService = imageService;
            Account account=new ()
            {
                Cloud=cloudinarySetting.Value.CloudName,
                ApiKey=cloudinarySetting.Value.APIKey,
                ApiSecret=cloudinarySetting.Value.APISecret
            };
            _cloudinary = new Cloudinary(account);
            _validator = validator;
			_booksService = booksService;
			_authorsService = authorsService;
			_categoriesService = categoriesService;	
        }
        public IActionResult Index()
		{
			return View();
		}


		public IActionResult Details(int Id)
		{
			var quary=_booksService.GetDetails();
			var BookDetails=_mapper.ProjectTo<BookDetailsViewModel>(quary).SingleOrDefault(b=>b.Id==Id);
            if (BookDetails is null) return NotFound();
            return View(BookDetails);
		}

		[HttpPost]
		[IgnoreAntiforgeryToken]

		public IActionResult GetBooks()
		{

			var filterDto=Request.Form.GetFilters();
            var (books, recordsTotal) = _booksService.GetFiltered(filterDto);
            var booksViewModel=_mapper.ProjectTo<BookRowViewModel>( books).ToList();
			var jsonData=new {  recordsFiltered=recordsTotal,recordsTotal,data=booksViewModel};
			return Ok(jsonData);
		}

		public IActionResult Create()
		{

			var bookFormViewModel=PopulateViewModel();
			return View("Form", bookFormViewModel);
		}






		[HttpPost]

		public async Task<IActionResult> Create(int id, BooksFormViewModel model)
		{
            //Fluent Validation with data annotation
			var validationResult=_validator.Validate(model);
			if (!validationResult.IsValid)
				validationResult.AddToModelState(ModelState);
            if (!ModelState.IsValid)
			{
				return View("Form", PopulateViewModel(model));
			}
			var book=_mapper.Map<Book>(model);
			if (model.Image is not null)
			{
				string extension=Path.GetExtension(model.Image.FileName.ToLower());
				string imageName=$"{Guid.NewGuid()}{extension}";
				var (isUploded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, "/images/books", hasThumbnail: true);
				if (!isUploded)
				{
					ModelState.AddModelError(nameof(model.Image), errorMessage!);
					return View("Form", PopulateViewModel(model));
				}

				book.ImageUrl = $"/images/books/{imageName}";
				book.ImageUrlThumbnail = $"/images/books/thumb/{imageName}";

				#region For Save Image at Cloudinary Cloud API

				//using Stream imageStream=model.Image.OpenReadStream();
				//ImageUploadParams imageUploadParams=new ImageUploadParams
				//{
				//    File=new FileDescription (imageName,imageStream )

				//};
				//var result= await   _cloudinary.UploadAsync(imageUploadParams);


				//book.ImageUrl = result.SecureUrl.ToString();
				//book.ImageUrlThumbnail = GetThumbnailUrl(book.ImageUrl);
				//book.ImageUrlPublicId = result.PublicId;
				#endregion
			}
			
			book = _booksService.Add(book, model.SelectedCategories, User.GetUserId());
			return RedirectToAction(nameof(Details), new { id = book.Id });

		}


		[HttpGet]

		public IActionResult Edit(int id)
		{
			var book=_booksService.GetWithCategories(id);
			if (book is null) return NotFound();
			var bookModelView=_mapper.Map<BooksFormViewModel>(book);
			bookModelView.SelectedCategories = book.Categories.Select(c => c.CategoryId).ToList();
			return View("Form", PopulateViewModel(bookModelView));
		}

		[HttpPost]

		public async Task<IActionResult> Edit([FromForm] BooksFormViewModel FormViewModel)
		{
            //Fluent Validation with data annotation
            var validationResult=_validator.Validate(FormViewModel);
            if (!validationResult.IsValid)
                validationResult.AddToModelState(ModelState);
            if (!ModelState.IsValid) return View("Form", PopulateViewModel(FormViewModel));
            var book = _booksService.GetWithCategories(FormViewModel.Id);
            if (book is null) return NotFound();
			string imageUrlPublicId=null;
			if (FormViewModel.Image is not null)
			{
				if (book.ImageUrl is not null)//delete old image 
				{
					_imageService.Delete(book.ImageUrl, book.ImageUrlThumbnail);

					//await _cloudinary.DeleteResourcesAsync(book.ImageUrlPublicId);

				}
				string extension=Path.GetExtension(FormViewModel.Image.FileName);
				#region Save Image On HardDisk
				string imageName=$"{Guid.NewGuid()}{extension}";
				var (isUploded, errorMessage) = await _imageService.UploadAsync(FormViewModel.Image, imageName, "/images/books", hasThumbnail: true);
				if (!isUploded)
				{
					ModelState.AddModelError(nameof(FormViewModel.Image), errorMessage!);
					return View("Form", PopulateViewModel(FormViewModel));
				}
				FormViewModel.ImageUrl = $"/images/books/{imageName}";
				FormViewModel.ImageUrlThumbnail = $"/images/books/thumb/{imageName}";
				#endregion

				// using Stream stream = model.Image.OpenReadStream();
				// var imageUplaodParametr=new ImageUploadParams
				// {
				//     File=new FileDescription (imageName,stream )
				// };
				//var result= await  _cloudinary.UploadAsync(imageUplaodParametr);

				// model.ImageUrl = result.Url.ToString();
				// imageUrlPublicId = result.PublicId;


			}
			book = _mapper.Map(FormViewModel, book);

			if (!FormViewModel.IsAvailableForRental)
			{
				foreach (var cop in book.BookCopies)
				{
					cop.IsAvailableForRental = false;
				}
			}
			//  book.ImageUrlThumbnail= GetThumbnailUrl(book.ImageUrl!);
			// book.ImageUrlPublicId = imageUrlPublicId;
			_booksService.Update(book, FormViewModel.SelectedCategories, User.GetUserId());
			return RedirectToAction(nameof(Details), new { id = book.Id });
		}




		private BooksFormViewModel PopulateViewModel(BooksFormViewModel? model = null)
		{
			var viewModel=model is null? new BooksFormViewModel() : model;
			var authors=_authorsService.LoadActive();
            var category=_categoriesService.LoadActive();
			viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
			viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(category);
			return viewModel;
		}






		[HttpPost]

		public IActionResult ToggleStatus(int id)
		{
            var book = _booksService.ToggleStatus(id, User.GetUserId());

            return book is null ? NotFound() : Ok();
        }
		public IActionResult AllowItem(BooksFormViewModel model)
		{
			return Json(_booksService.AllowTitle(model.Id, model.Title, model.AuthorId));
		}


		private string GetThumbnailUrl(string url)//For cloudinary
		{
			var separator = "image/upload/";
			var urlParts = url.Split(separator);

			var thumbnailUrl = $"{urlParts[0]}{separator}c_thumb,w_200,g_face/{urlParts[1]}";

			return thumbnailUrl;
		}





	}
}
