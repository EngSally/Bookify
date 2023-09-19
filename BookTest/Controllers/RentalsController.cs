using BookTest.Core.ViewModels.BookCopy;
using BookTest.Core.ViewModels.Rental;
using BookTest.Core.ViewModels.Subscribers;
using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
	[Authorize(Roles = AppRole.Reception)]
	public class RentalsController : Controller
    {
        private readonly  ApplicationDbContext _context;
        private readonly IMapper _mapper;

		public RentalsController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
        public IActionResult Create(string sKey)
        {
            RentalFormViewModel model = new ()
            {
                SubscriberKey = sKey
            };
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult SearchBookCopy(SearchFormViewModel model)
        {
            if(!ModelState.IsValid) return BadRequest(); 
            var copy=_context.BooksCopies
                .Include(c=>c.Book)
                .SingleOrDefault(c=>c.SerialNumber.ToString()==model.Value &&!c.Book!.Deleted && !c.Deleted);
            if (copy is null)
                return NotFound(Errors.InvalidSertailNum);
            if (!copy.IsAvailableForRental || !copy.Book!.IsAvailableForRental)
                return BadRequest(Errors.NotAvailableForRental);

            //ToDo Check If Copy Is Not  Rental  By  Another Subscribe
            var bookCopy=_mapper.Map<BookCopyViewModel>(copy);
            return PartialView("_CopyDetails", bookCopy);

        }

	}
}
