
using BookTest.Core.ViewModels.Books;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Data;
using System.Linq.Dynamic.Core;

namespace BookTest.Controllers
{
	[Authorize(Roles = AppRole.Archive)]
	public class BooksController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly Cloudinary _cloudinary;
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly IImageService _imageService;



		private readonly List<string> _allowedImageExtension=new(){ ".jpg",".jpeg",".png",".ico"};
		private readonly int _allowedSize=3145728;
		public BooksController(ApplicationDbContext context, IMapper mapper
			, IWebHostEnvironment webHostEnvironment, IOptions<CloudinarySetting> cloudinarySetting, IImageService imageService)
		{
			_context = context;
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
		}
		public IActionResult Index()
		{
			return View();
		}


		public IActionResult Details(int Id)
		{
			var book=_context.Books
				.Include(b=>b.Author)
				.Include(b=>b.BookCopies)
				.Include(b=>b.Categories)
				.ThenInclude(c=>c.Category)
				.SingleOrDefault(b=>b.Id==Id);

			if (book is null) return NotFound();
			var BookDetails=_mapper.Map<BookDetailsViewModel>(book);

			return View(BookDetails);
		}

		[HttpPost]
		[IgnoreAntiforgeryToken]

		public IActionResult GetBooks()
		{
			int skip= int.Parse( Request.Form["start"]!);
			int pageSize=int.Parse( Request.Form["length"]!);
			int colSortIndex=int.Parse( Request.Form["order[0][column]"]!);
			string  colSort=Request.Form[$"columns[{colSortIndex}][name]"]!;
			string  sortType=Request.Form["order[0][dir]"]!;
			string  searchValue=Request.Form["search[value]"]!;

			IQueryable<Book> books=_context.Books
				.Include(b=>b.Author)
				.Include(c=>c.Categories)
				.ThenInclude(c=>c.Category);
			if (!string.IsNullOrEmpty(searchValue))
				books = books.Where(b => b.Title.Contains(searchValue) || b.Author!.Name.Contains(searchValue));
			books = books.OrderBy($" {colSort} {sortType}");

			var data=books.Skip(skip).Take(pageSize).ToList();
			var booksViewModel=_mapper.Map<IEnumerable<BookDetailsViewModel>>(data);
			var recordsTotal=books.Count();
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
			foreach (var category in model.SelectedCategories)
			{
				book.Categories.Add(new BookCategory { CategoryId = category });
			}
			book.CreatedById = User.GetUserId();
			_context.Books.Add(book);

			_context.SaveChanges();
			return RedirectToAction(nameof(Details), new { id = book.Id });

		}


		[HttpGet]

		public IActionResult Edit(int id)
		{
			var book=_context.Books.Include(b=>b.Categories).SingleOrDefault(b=>b.Id == id);
			if (book is null) return NotFound();
			var bookModelView=_mapper.Map<BooksFormViewModel>(book);
			bookModelView.SelectedCategories = book.Categories.Select(c => c.CategoryId).ToList();
			return View("Form", PopulateViewModel(bookModelView));
		}

		[HttpPost]

		public async Task<IActionResult> Edit([FromForm] BooksFormViewModel FormViewModel)
		{
			if (!ModelState.IsValid) return View("Form", PopulateViewModel(FormViewModel));
			var book=_context.Books.
				Include(b=>b.Categories)
				.Include(b=>b.BookCopies)
				.SingleOrDefault(b=>b.Id==FormViewModel.Id);
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

			foreach (var category in FormViewModel.SelectedCategories)
			{
				book.Categories.Add(new BookCategory { CategoryId = category });
			}
			book.LastUpdateOn = DateTime.Now;
			book.LastUpdateById = User.GetUserId();
			if (!FormViewModel.IsAvailableForRental)
			{
				foreach (var cop in book.BookCopies)
				{
					cop.IsAvailableForRental = false;
				}
			}
			//  book.ImageUrlThumbnail= GetThumbnailUrl(book.ImageUrl!);
			// book.ImageUrlPublicId = imageUrlPublicId;
			_context.SaveChanges();
			return RedirectToAction(nameof(Details), new { id = book.Id });
		}




		private BooksFormViewModel PopulateViewModel(BooksFormViewModel? model = null)
		{
			var viewModel=model is null? new BooksFormViewModel() : model;
			var authors=_context.Authors.Where(a=> !a.Deleted).OrderBy(a=>a.Name).AsNoTracking().ToList();
			var category=_context.Categories.Where(c=>!c.Deleted).OrderBy(c=>c.Name).AsNoTracking().ToList();
			viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
			viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(category);
			return viewModel;
		}






		[HttpPost]

		public IActionResult ToggleStatus(int id)
		{
			var book = _context.Books.Find(id);

			if (book is null)
				return NotFound();

			book.Deleted = !book.Deleted;
			book.LastUpdateOn = DateTime.Now;
			book.LastUpdateById = User.GetUserId();
			_context.SaveChanges();

			return Ok();
		}
		public IActionResult AllowItem(BooksFormViewModel model)
		{
			var book = _context.Books.SingleOrDefault(b=>b.Title==model.Title&&b.AuthorId==model.AuthorId);
			bool allow=book is null||book.Id.Equals(model.Id);
			return Json(allow);
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
