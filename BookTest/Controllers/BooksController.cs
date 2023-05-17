
using BookTest.Core.ViewModels.Books;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;


namespace BookTest.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Cloudinary _cloudinary;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly List<string> _allowedImageExtension=new(){ ".jpg",".jpeg",".png",".ico"};
        private readonly int _allowedSize=3145728;
        public BooksController(ApplicationDbContext context, IMapper mapper
            , IWebHostEnvironment webHostEnvironment, IOptions<CloudinarySetting> cloudinarySetting)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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

        public IActionResult Create()
        {
          
            var bookFormViewModel=PopulateViewModel();
            return View("Form", bookFormViewModel);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public  async Task<IActionResult> Create(BooksFormViewModel model)
        {
            if(!ModelState.IsValid) {
            
                return View("Form", PopulateViewModel(model));
                    }
            var book=_mapper.Map<Book>(model);

            if(model.Image is not null) 
            {
                string extension=Path.GetExtension(model.Image.FileName);
                if (!_allowedImageExtension.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if(model.Image.Length>_allowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageSize);
                    return View("Form", PopulateViewModel(model));
                }

                #region Save Image On HardDisk
                string imageName=$"{Guid.NewGuid()}{extension}";
                var path=Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books",imageName);
                var pathThumbnail=Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books/Thumbnail",imageName);
                using var stream=System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                stream.Dispose();
                using   var image=   Image.Load(model.Image.OpenReadStream());
                var width=(float) image.Width/200;
                var height= image.Height/width;
                image.Mutate(i=>i.Resize( width: 200,height:(int) height));
                image.Save(pathThumbnail);
                book.ImageUrl = $"/images/books/{imageName}";
                book.ImageUrlThumbnail = $"/images/books/Thumbnail/{imageName}";
                #endregion

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
# endregion
            }
            foreach (var category in model.SelectedCategories)
            {
                book.Categories.Add(new BookCategory { CategoryId = category });
            }
            _context.Books.Add(book);
          
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var book=_context.Books.Include(b=>b.Categories).SingleOrDefault(b=>b.Id == id);
            if (book is null) return NotFound();
            var bookModelView=_mapper.Map<BooksFormViewModel>(book);
            bookModelView.SelectedCategories = book.Categories.Select(c => c.CategoryId).ToList();
            return View("Form", PopulateViewModel( bookModelView));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(BooksFormViewModel model)
        {
            if (!ModelState.IsValid) return View("Form", PopulateViewModel(model));
            var book=_context.Books.Include(b=>b.Categories).SingleOrDefault(b=>b.Id==model.Id);
            if (book is null) return NotFound();
            string imageUrlPublicId=null;
            if (model.Image is not null)
            {
                if(book.ImageUrl is not null)//delete old image 
                {
                     string oldimage=$"{_webHostEnvironment.WebRootPath}{book.ImageUrl}";
                     string oldThumbnail=$"{_webHostEnvironment.WebRootPath}{ book.ImageUrlThumbnail!}";
                    if( System.IO.File.Exists(oldimage)) System.IO.File.Delete(oldimage);
                    if( System.IO.File.Exists(oldThumbnail)) System.IO.File.Delete(oldThumbnail);
                  
                    //await _cloudinary.DeleteResourcesAsync(book.ImageUrlPublicId);

                }

                string extension=Path.GetExtension(model.Image.FileName);
                if (!_allowedImageExtension.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _allowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.AllowedImageSize);
                    return View("Form", PopulateViewModel(model));
                }
              

                #region Save Image On HardDisk
                string imageName=$"{Guid.NewGuid()}{extension}";
                var path=Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books",imageName);
                var pathThumbnail=Path.Combine($"{_webHostEnvironment.WebRootPath}/images/books/Thumbnail",imageName);
                using var stream=System.IO.File.Create(path);
                model.Image.CopyTo(stream);
                stream.Dispose();
                using   var image=   Image.Load(model.Image.OpenReadStream());
                var width=(float) image.Width/200;
                var height= image.Height/width;
                image.Mutate(i => i.Resize(width: 200, height: (int)height));
                image.Save(pathThumbnail);
                model.ImageUrl = $"/images/books/{imageName}";
                model.ImageUrlThumbnail = $"/images/books/Thumbnail/{imageName}";
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
            book = _mapper.Map(model, book);
            foreach (var category in model.SelectedCategories)
            {
                book.Categories.Add(new BookCategory { CategoryId = category });
            }
            book.LastUpdate=DateTime.Now;
          //  book.ImageUrlThumbnail= GetThumbnailUrl(book.ImageUrl!);
           // book.ImageUrlPublicId = imageUrlPublicId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

       private BooksFormViewModel PopulateViewModel(BooksFormViewModel? model=null)
        {
            var viewModel=model is null? new BooksFormViewModel() : model;
            var authors=_context.Authors.Where(a=> !a.Deleted).OrderBy(a=>a.Name).AsNoTracking().ToList();
            var category=_context.Categories.Where(c=>!c.Deleted).OrderBy(c=>c.Name).AsNoTracking().ToList();
            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(category);
            return viewModel;
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
