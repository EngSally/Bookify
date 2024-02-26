

namespace Bookify.Domain;

    public  record GetFilterDTO(
        int Skip,
        int PageSize,
        int ColSortIndex,
        string ColSort,
        string SortType,
        string SearchValue);


