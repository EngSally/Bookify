
using Bookify.Domain;
namespace Bookify.Web.Extensions;

    public static class FormCollectionExtensions
    {
        public static GetFilterDTO GetFilters(this IFormCollection form)
    {
        int skip= int.Parse( form["start"]!);
        int pageSize=int.Parse( form["length"]!);
        int colSortIndex=int.Parse( form["order[0][column]"]!);
        string  colSort=form[$"columns[{colSortIndex}][name]"]!;
        string  sortType=form["order[0][dir]"]!;
        string  searchValue=form["search[value]"]!;
        return new GetFilterDTO(skip, pageSize, colSortIndex, colSort, sortType, searchValue);
    }
}
