using BookTest.Core.ViewModels.BookCopy;
using BookTest.Core.ViewModels.Rental;
using BookTest.Core.ViewModels.Subscribers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookTest.Controllers
{
	[Authorize(Roles = AppRole.Reception)]
	public class RentalsController : Controller
    {
        private readonly  ApplicationDbContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly IMapper _mapper;

		public RentalsController(ApplicationDbContext context, IDataProtectionProvider dataProtector, IMapper mapper)
		{
			_context = context;
            _dataProtector = dataProtector.CreateProtector("MySecureKey");
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
           var(errorMessage,  maxAllowedCopies) =  ValidateSubscriber(subscriber);
            if(!string .IsNullOrEmpty(errorMessage))
                return View("NotAvailbleForRental", errorMessage);
                       RentalFormViewModel model = new ()
            {
                SubscriberKey = sKey,
                CountAvailableForRental=maxAllowedCopies

            };
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RentalFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var subscriberId=int.Parse(_dataProtector.Unprotect(model.SubscriberKey));
            var subscriber=_context.Subscribers
                .Include(s=>s.RenewalSubscribtions)
                .Include(s=>s.Rentals)
                .ThenInclude(r=>r.RentalCopies)
                .FirstOrDefault(s=>s.Id == subscriberId);
            if (subscriber is null)
                return NotFound();
            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber);
            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAvailbleForRental", errorMessage);

            var selectedBookCopies=_context.BooksCopies
                .Include(c=>c.Book)
                .Include(c=>c.Rentals)
                .Where(c=>model.SelectedCopies.Contains(c.SerialNumber))
                .ToList();

            var subscriberRentals=_context.Rentals
                .Include(r=>r.RentalCopies)
                .ThenInclude(rc=>rc.BookCopy)
                .Where(r=>r.SubscriberId== subscriberId )
                .SelectMany(r=>r.RentalCopies)
                .Where(rc=>!rc.ReturnDate.HasValue)
                .Select(rc=>rc.BookCopy!.BookId)
                .ToList();
            List<RentalCopy>rentalCopies = new List<RentalCopy>();
            foreach (var copy in selectedBookCopies)
            {
                if (!copy.IsAvailableForRental || !copy.Book!.IsAvailableForRental)
                    return View("NotAvailbleForRental", Errors.NotAvailableForRental);
                if (copy.Rentals.Any(c => c.BookCopyId == copy.Id && c.ReturnDate.HasValue))
                    return View("NotAvailbleForRental", Errors.CopyIsInRental);
                if (subscriberRentals.Any(BookId => BookId == copy.BookId))
                    return View("NotAvailbleForRental", $"This Subscriber Already  has acopy of {copy.Book.Title} book");
                rentalCopies.Add(new() { BookCopyId = copy.Id });
            }
            Rental rental= new()
            {
                RentalCopies = rentalCopies,
                SubscriberId = subscriberId,
                CreatedById=User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };
            _context.Rentals.Add(rental);
            _context.SaveChanges();
            return Ok();
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
                return NotFound(Errors.InvalidSerialNumber);
            if (!copy.IsAvailableForRental || !copy.Book!.IsAvailableForRental)
                return BadRequest(Errors.NotAvailableForRental);

            // Check If Copy Is Not  Rental  By  Another Subscribe
            var copyIsRental=_context.RentalCopies.Any(c=>c.BookCopyId==copy.Id&&!c.ReturnDate.HasValue);
            if(copyIsRental)
                return BadRequest(Errors.CopyIsInRental);
            var bookCopy=_mapper.Map<BookCopyViewModel>(copy);
            return PartialView("_CopyDetails", bookCopy);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelRental(int id)
        {
            return Ok("Sallly");
        }

        private (string errorMessage, int? maxAllowedCopies) ValidateSubscriber(Subscriber subscriber)
        {
            if (subscriber.IsBlackListed)
                   return (errorMessage: Errors.BlackListedSubscriber, maxAllowedCopies: null);

            if (subscriber.RenewalSubscribtions.Last().EndDate < DateTime.Now.AddDays((int)RentalsConfigurations.RentalDuration))
                   return (errorMessage: Errors.InactiveSubscriber, maxAllowedCopies: null);
            var currentRentals = subscriber.Rentals.SelectMany(r => r.RentalCopies).Count(c => !c.ReturnDate.HasValue);
            var allowRentalCopy=(int)RentalsConfigurations.MaxAllowedCopies-currentRentals;
            if (allowRentalCopy.Equals(0))
                   return (errorMessage: Errors.MaxCopiesReached, maxAllowedCopies: null);

            return (errorMessage: string.Empty, maxAllowedCopies: allowRentalCopy);


        }

    }
}
