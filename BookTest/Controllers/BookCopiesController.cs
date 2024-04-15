
using Bookify.Infrastructure.Services.BookCopies;
using Bookify.Infrastructure.Services.Rentals;
using Bookify.Web.Core.ViewModels.BookCopy;
using DocumentFormat.OpenXml.Office2010.Excel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;


namespace Bookify.Web.Controllers
{
	public class BookCopiesController : Controller
	{
		private readonly   IApplicationDbContext  _context;
		private readonly IMapper _mapper;
		private readonly IValidator<BookCopyFormViewModel> _validator;
		private readonly IBooksService _booksService;
		private readonly IBookCopiesService _bookCopiesService;
		private readonly IRentalService _rentalService;

		public BookCopiesController(IApplicationDbContext context, IMapper mapper,
			IValidator<BookCopyFormViewModel> validator, IBooksService booksService, IBookCopiesService bookCopiesService, IRentalService rentalService)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
			_booksService = booksService;
			_bookCopiesService = bookCopiesService;
			_rentalService = rentalService;
        }
        [AjaxOnly]

		public IActionResult Create(int bookId)
		{
			var isAvailableForRental=_booksService.BookAvailableForRental(bookId);
			if (isAvailableForRental is null) return NotFound();
			
			var model=new BookCopyFormViewModel
			{
				BookId=bookId,
				ShowRentalInput=isAvailableForRental.HasValue
            };
			return PartialView("_FormBookCopy", model);
		}

		[HttpPost]

        public IActionResult Create(BookCopyFormViewModel model)
		{//Fluent Validation
			var result=_validator.Validate(model);
			if(! result.IsValid) return BadRequest();
            //if (!ModelState.IsValid)
            //	return BadRequest();
            var bookIsAvailable=_booksService.BookAvailableForRental(model.BookId);
            if (bookIsAvailable is null) return NotFound();

            BookCopy copy = new()
			{
				EditionNumber = model.EditionNumber,
				BookId=model.BookId,
				IsAvailableForRental =bookIsAvailable.HasValue && model.IsAvailableForRental,
				CreatedById =User.GetUserId()
			};

            copy=_bookCopiesService.Add(copy);
			var viewModel = _mapper.Map<BookCopyViewModel>(copy);

			return PartialView("_BookCopyRow", viewModel);
		}



		[AjaxOnly]
		[HttpGet]
		public IActionResult Edit(int id)
		{
            (BookCopy? bookCopy, bool? BookAvailableForRental) = _bookCopiesService.GetById(id);  
		
			if (bookCopy is null) return NotFound();
			var model=_mapper.Map<BookCopyFormViewModel>(bookCopy);
			model.ShowRentalInput = BookAvailableForRental.HasValue;
            return PartialView("_FormBookCopy", model);
		}

		[HttpPost]

		public IActionResult Edit(BookCopyFormViewModel model)
		{

            // Fluent Validation
            var result = _validator.Validate(model);
            if (!result.IsValid) return BadRequest();
            //if (!ModelState.IsValid) return BadRequest();
            (BookCopy? bookCopy, bool? BookAvailableForRental) = _bookCopiesService.GetById(model.Id);
            if (bookCopy == null) return NotFound();
			bookCopy = _mapper.Map(model, bookCopy);
			bookCopy.IsAvailableForRental = BookAvailableForRental.HasValue ? model.IsAvailableForRental : false;
			bookCopy.LastUpdateById = User.GetUserId();
			bookCopy.LastUpdateOn = DateTime.Now;
			_bookCopiesService.Update(bookCopy);
			var bookCopyViewModel=_mapper.Map<BookCopyViewModel>(bookCopy);
			return PartialView("_BookCopyRow", bookCopyViewModel);


		}
		public IActionResult RentalHistory(int id)
		{
            var rentals=   _rentalService.RentalHistory(id);
         
			var model=_mapper.Map<IEnumerable<BookCopyRentalHistoryViewModel>>(rentals);
			return View(model);
		}

		[HttpPost]

		public IActionResult ToggleStatus(int id)
		{
			var copy =_bookCopiesService.ToggleStatus(id,User.GetUserId());
			if(copy == null) return NotFound();
            return Ok();

          
		}

		public IActionResult AllowItem(BookCopyFormViewModel model)
		{
			
			return Json(_bookCopiesService.AllowItem(model.BookId,model.Id,model.EditionNumber));
		}

		
	}
}
