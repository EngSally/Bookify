

using Bookify.Domain;

namespace Bookify.Infrastructure.Services.Rentals
{
    public  interface IRentalService
    {
        IEnumerable<CopyRentalHistoryDto> RentalHistory(int copyId);
    }
}
