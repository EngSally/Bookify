using Bookify.Domain;
using Bookify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Diagnostics.Activity;

namespace Bookify.Infrastructure.Services.Rentals
{
    public  class RentalService: IRentalService
    {
        private readonly  ApplicationDbContext _context;
        public RentalService(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public IEnumerable<CopyRentalHistoryDto> RentalHistory(int copyId)
        {
           
            var rentals=_context.RentalCopies
                .Include(c=>c.Rental)
                .ThenInclude(r=>r!.Subscriber)
                .Select(c=> new  CopyRentalHistoryDto()
                {
                    BookCopyId=  c.BookCopyId,
                    SubscriberName=c.Rental!.Subscriber!.FristName,
                    SubscriberNum=c.Rental.Subscriber.MobilNum,
                    RentalDate= c.RentalDate,
                    EndDate= c.EndDate,
                    ExtendedOn=  c.ExtendedOn,
                    ReturnDate= c.ReturnDate
                })
                .Where(c=>c.BookCopyId==copyId).ToList();

            return rentals;
        }

       
    }
}
