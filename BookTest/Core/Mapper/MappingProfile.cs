using BookTest.Core.ViewModels.Authors;
using BookTest.Core.ViewModels.Categories;

namespace BookTest.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {////Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, CategoriesFormViewModel>().ReverseMap();
            ////Author
            CreateMap<Author, AuthorViewModel>();
            CreateMap<Author, AuthorsFormViewModel>().ReverseMap();



        }
    }
}
