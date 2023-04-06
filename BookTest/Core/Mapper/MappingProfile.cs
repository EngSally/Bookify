using AutoMapper;
using BookTest.Core.Models;

namespace BookTest.Core.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Category, CategoriesFormViewModel>().ReverseMap();
        }
    }
}
