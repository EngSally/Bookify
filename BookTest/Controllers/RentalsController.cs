using BookTest.Core.ViewModels.BookCopy;
using BookTest.Core.ViewModels.Rental;
using BookTest.Core.ViewModels.Subscribers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
	[Authorize(Roles = AppRole.Reception)]
	public class RentalsController : Controller
    {
        private readonly  ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly IMapper _mapper;

		public RentalsController(ApplicationDbContext context, IDataProtector dataProtector,IMapper mapper)
		{
			_context = context;
            _dataProtector = dataProtector;
			_mapper = mapper;
		}

     

        [HttpGet]
        public IActionResult Create(string sKey)

        {
            var subscriberId=int.Parse(_dataProtector.Unprotect(sKey));
            var subscriber=_context.Subscribers
                .Include(s=>s.RenewalSubscribtions)
                .Include(s=>s.Rentals)
                .ThenInclude(r=>r.RentalCopies)
                .FirstOrDefault(s=>s.Id == subscriberId);
            if (subscriber is null)
                return NotFound();
            if (subscriber.IsBlackListed)
                return View("NotAvailbleForRental", Errors.BlackListedSubscriber);
            if (subscriber.RenewalSubscribtions.Last().EndDate < DateTime.Now.AddDays((int)RentalsConfigurations.RentalDuration))
                return View("NotAvailbleForRental", Errors.InactiveSubscriber);
            var currentRentals = subscriber.Rentals.SelectMany(r => r.RentalCopies).Count(c => !c.ReturnDate.HasValue);
            var allowRentalCopy=(int)RentalsConfigurations.MaxAllowedCopies-currentRentals;
            if(allowRentalCopy.Equals(0))
                return View("NotAvailbleForRental", Errors.MaxCopiesReached);
            RentalFormViewModel model = new ()
            {
                SubscriberKey = sKey,
                CountAvailableForRntal=allowRentalCopy

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
