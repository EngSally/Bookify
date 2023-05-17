using BookTest.Core.ViewModels.Authors;
using BookTest.Core.ViewModels.Books;
using BookTest.Core.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTest.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, CategoriesFormViewModel>().ReverseMap();
            CreateMap<Category,SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            ////Author
            CreateMap<Author, AuthorViewModel>();
            CreateMap<Author, AuthorsFormViewModel>().ReverseMap();
            CreateMap<Author, SelectListItem>()
               .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
               .ForMember(des => des.Text, opt => opt.MapFrom(src => src.Name));

            //Books
           CreateMap<BooksFormViewModel, Book>()
                .ReverseMap()
                .ForMember(des=>des.Categories,opt=>opt.Ignore());
           

        }
    }
}
