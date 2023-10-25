using BookTest.Core.Models;
using BookTest.Core.ViewModels.BookCopy;
using BookTest.Core.ViewModels.Rental;
using BookTest.Core.ViewModels.Subscribers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            return View("Form",model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RentalFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form",model);
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

            var (rentalsError, copies) = ValidateCopies(model.SelectedCopies, subscriberId);

            if (!string.IsNullOrEmpty(rentalsError))
                return View("NotAllowedRental", rentalsError);
            Rental rental = new()
            {
                RentalCopies = copies,
                CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };

            subscriber.Rentals.Add(rental);
            _context.SaveChanges();
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = rental.Id });
        }



       
         public  IActionResult Edit(int id)
        {
            var rental=_context.Rentals
                        .Include(r=>r.RentalCopies)
                        .ThenInclude(r=>r.BookCopy)
                        .FirstOrDefault(r=>r.Id==id);
            if (rental is null ||rental.StartDate.Date!=DateTime.Today)
                return NotFound();
            var subscriber=_context.Subscribers
                .Include(s=>s.RenewalSubscribtions)
                .Include(s=>s.Rentals)
                .ThenInclude(r=>r.RentalCopies)
                .FirstOrDefault(s=>s.Id == rental.SubscriberId);
           
            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber!,rental.Id);
            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAvailbleForRental", errorMessage);
            var currentBookCopyIds=rental.RentalCopies.Select(r=>r.BookCopyId).ToList();
            var currentBookCopy=_context.BooksCopies
                                    .Where(b=>currentBookCopyIds.Contains(b.Id))
                                    .Include(b=>b.Book)
                                    .ToList();
            RentalFormViewModel model = new ()
            {   Id=rental.Id,
                SubscriberKey = _dataProtector.Protect(rental.SubscriberId.ToString()),
                CurrentCopies=_mapper.Map<IEnumerable<BookCopyViewModel>>(currentBookCopy),
                CountAvailableForRental=maxAllowedCopies

            };
            return View("Form", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit( int id,RentalFormViewModel model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            var rental=_context.Rentals
                .Include(r=>r.RentalCopies)
                .SingleOrDefault(r=>r.Id==model.Id);

            if (rental is null || rental.StartDate.Date != DateTime.Now.Date)
                return NotFound();

            var subscriberId=int.Parse(_dataProtector.Unprotect(model.SubscriberKey));

            var subscriber=_context.Subscribers
                .Include(s=>s.RenewalSubscribtions)
                .Include(s=>s.Rentals)
                .ThenInclude(r=>r.RentalCopies)
                .FirstOrDefault(s=>s.Id == subscriberId);
            if (subscriber is null)
                return NotFound();
            var (errorMessage, maxAllowedCopies) = ValidateSubscriber(subscriber,model.Id);
            if (!string.IsNullOrEmpty(errorMessage))
                return View("NotAvailbleForRental", errorMessage);

            var (rentalsError, copies) = ValidateCopies(model.SelectedCopies, subscriberId, rental.Id);

            if (!string.IsNullOrEmpty(rentalsError))
                return View("NotAllowedRental", rentalsError);
            rental.RentalCopies = copies;
            rental.LastUpdateById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            rental.LastUpdateOn = DateTime.Now;

            _context.SaveChanges();


            return RedirectToAction(nameof(Details), new { id = model.Id });

        }



        public  IActionResult Return(int id)
        {
            var rental = _context.Rentals
                .Include(r => r.RentalCopies)
                .ThenInclude(c => c.BookCopy)
                .ThenInclude(c => c!.Book)
                .SingleOrDefault(r => r.Id == id);

            if (rental is null || rental.CreatedOn.Date == DateTime.Today)
                return NotFound();

            var subscriber = _context.Subscribers
                .Include(s => s.RenewalSubscribtions)
                .SingleOrDefault(s => s.Id == rental.SubscriberId);

            var viewModel = new ReturnViewModel
            {
                Id = id,
                Copies = _mapper.Map<IList<RentalCopyViewModel>>(rental.RentalCopies.Where(c => !c.ReturnDate.HasValue).ToList()),
                SelectedCopies = rental.RentalCopies.Where(c => !c.ReturnDate.HasValue).Select(c => new ReturnCopyViewModel { Id = c.BookCopyId, IsReturn = c.ExtendedOn.HasValue ? false : null }).ToList(),
                AllowExtend = !subscriber!.IsBlackListed
                    && subscriber!.RenewalSubscribtions.Last().EndDate >= rental.StartDate.AddDays((int)RentalsConfigurations.MaxRentalExtended)
                    && rental.StartDate.AddDays((int)RentalsConfigurations.RentalDuration) >= DateTime.Today
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Return (ReturnViewModel model)
        {
            var rental = _context.Rentals
                .Include(r => r.RentalCopies)
                .ThenInclude(c => c.BookCopy)
                .ThenInclude(c => c!.Book)
                .SingleOrDefault(r => r.Id == model.Id);

            if (rental is null || rental.CreatedOn.Date == DateTime.Today)
                return NotFound();
            if(!ModelState.IsValid)
            {
                model.Copies = _mapper.Map<IList<RentalCopyViewModel>>(rental.RentalCopies.Where(c => !c.ReturnDate.HasValue).ToList());
                return View(model);
            }
            var subscriber = _context.Subscribers
                .Include(s => s.RenewalSubscribtions)
                .SingleOrDefault(s => s.Id == rental.SubscriberId);
            if(model.SelectedCopies.Any(c=>c.IsReturn.HasValue&&!c.IsReturn.Value))
            {
                string error="";
                if(subscriber!.IsBlackListed)
                         error = Errors.ExtendNotAllowForBlockList;
                else if(subscriber!.RenewalSubscribtions.Last().EndDate < rental.StartDate.AddDays((int)RentalsConfigurations.MaxRentalExtended))
                          error = Errors.ExtendNotAllowForInActive;
                else if(rental.StartDate.AddDays((int)RentalsConfigurations.RentalDuration) < DateTime.Today)
                         error= Errors.ExtendNotAllow;
                if(!string.IsNullOrEmpty(error))
                {
                    model.Copies = _mapper.Map<IList<RentalCopyViewModel>>(rental.RentalCopies.Where(c => !c.ReturnDate.HasValue).ToList());
                    ModelState.AddModelError("", error);
                    return View(model);
                }
            }
            var isUpdated = false;

            foreach (var copy in model.SelectedCopies)
            {
                if (!copy.IsReturn.HasValue) continue;

                var currentCopy = rental.RentalCopies.SingleOrDefault(c => c.BookCopyId == copy.Id);

                if (currentCopy is null) continue;

                if (copy.IsReturn.HasValue && copy.IsReturn.Value)
                {
                    if (currentCopy.ReturnDate.HasValue) continue;

                    currentCopy.ReturnDate = DateTime.Now;
                    isUpdated = true;
                }

                if (copy.IsReturn.HasValue && !copy.IsReturn.Value)
                {
                    if (currentCopy.ExtendedOn.HasValue) continue;

                    currentCopy.ExtendedOn = DateTime.Now;
                    currentCopy.EndDate = currentCopy.RentalDate.AddDays((int)RentalsConfigurations.MaxRentalExtended);
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                rental.LastUpdateOn = DateTime.Now;
                rental.LastUpdateById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                 rental.PenaltyPaid = model.PenaltyPaid;

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = rental.Id });


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
            var rental=_context.Rentals.Find(id);
            if(rental  is  null || rental.StartDate.Date!=DateTime.Today)
                return NotFound();
            rental.Deleted = true;
            rental.LastUpdateById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            rental.LastUpdateOn = DateTime.Now;
            _context.SaveChanges();
            return Ok();
        }

        public  IActionResult Details(int id)
        {
            var rental=_context.Rentals
                .Include(r=>r.RentalCopies)
                .ThenInclude(c=>c.BookCopy)
                .ThenInclude(b=>b!.Book)
                .SingleOrDefault(r=>r.Id==id);
            if(rental is null) return NotFound();
            RentalViewModel model=_mapper.Map<RentalViewModel>(rental);
            return View(model);
        }
       private (string errorMessage, int? maxAllowedCopies) ValidateSubscriber(Subscriber subscriber,int? rentalId=null)
        {
            if (subscriber.IsBlackListed)
                   return (errorMessage: Errors.BlackListedSubscriber, maxAllowedCopies: null);

            if (subscriber.RenewalSubscribtions.Last().EndDate < DateTime.Now.AddDays((int)RentalsConfigurations.RentalDuration))
                   return (errorMessage: Errors.InactiveSubscriber, maxAllowedCopies: null);
            var currentRentals = subscriber.Rentals
                .Where (r=> rentalId  is null ||r.Id != rentalId)
                .SelectMany(r => r.RentalCopies)
                .Count(c => !c.ReturnDate.HasValue);
            var allowRentalCopy=(int)RentalsConfigurations.MaxAllowedCopies-currentRentals;
            if (allowRentalCopy.Equals(0))
                   return (errorMessage: Errors.MaxCopiesReached, maxAllowedCopies: null);

            return (errorMessage: string.Empty, maxAllowedCopies: allowRentalCopy);


        }

        private (string errorMessage, ICollection<RentalCopy> copies) ValidateCopies(IEnumerable<int> selectedSerials, int subscriberId, int? rentalId = null)
        {
            var selectedCopies = _context.BooksCopies
                .Include(c => c.Book)
                .Include(c => c.Rentals)
                .Where(c => selectedSerials.Contains(c.SerialNumber))
                .ToList();

            var currentSubscriberRentals = _context.Rentals
                .Include(r => r.RentalCopies)
                .ThenInclude(c => c.BookCopy)
                .Where(r => r.SubscriberId == subscriberId && (rentalId == null || r.Id != rentalId))
                .SelectMany(r => r.RentalCopies)
                .Where(c => !c.ReturnDate.HasValue)
                .Select(c => c.BookCopy!.BookId)
                .ToList();

            List<RentalCopy> copies = new();

            foreach (var copy in selectedCopies)
            {
                if (!copy.IsAvailableForRental || !copy.Book!.IsAvailableForRental)
                    return (errorMessage: Errors.NotAvailableForRental, copies);

                if (copy.Rentals.Any(c => !c.ReturnDate.HasValue && (rentalId == null || c.RentalId != rentalId)))
                    return (errorMessage: Errors.CopyIsInRental, copies);

                if (currentSubscriberRentals.Any(bookId => bookId == copy.BookId))
                    return (errorMessage: $"This subscriber already has a copy for '{copy.Book.Title}' Book", copies);

                copies.Add(new RentalCopy { BookCopyId = copy.Id });
            }

            return (errorMessage: string.Empty, copies);
        }

    }
}
