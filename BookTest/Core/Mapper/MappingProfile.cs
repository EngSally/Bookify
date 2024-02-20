using Bookify.Web.Core.ViewModels.Authors;
using Bookify.Web.Core.ViewModels.BookCopy;
using Bookify.Web.Core.ViewModels.Books;
using Bookify.Web.Core.ViewModels.Categories;
using Bookify.Web.Core.ViewModels.Rental;
using Bookify.Web.Core.ViewModels.Subscribers;
using Bookify.Web.Core.ViewModels.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Core.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			//Category
			CreateMap<Category, CategoryViewModel>();
			CreateMap<Category, CategoriesFormViewModel>().ReverseMap();
			CreateMap<Category, SelectListItem>()
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
				 .ForMember(des => des.Categories, opt => opt.Ignore());

			CreateMap<Book, BookDetailsViewModel>()
				.ForMember(des => des.Author, opt => opt.MapFrom(src => src.Author!.Name))
				.ForMember(des => des.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Category!.Name).ToList()));

			CreateMap<Book, BookRowViewModel>()
				.ForMember(des => des.Author, opt => opt.MapFrom(src => src.Author!.Name));
				


			//BookCopy
			CreateMap<BookCopy, BookCopyViewModel>()
					.ForMember(des => des.BookTitle, opt => opt.MapFrom(src => src.Book!.Title))
					.ForMember(des => des.BookId, opt => opt.MapFrom(src => src.Book!.Id))
					.ForMember(des => des.BookThumbnailURL, opt => opt.MapFrom(src => src.Book!.ImageUrlThumbnail));

			CreateMap<BookCopyFormViewModel, BookCopy>().ReverseMap();

			//Users
			CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
			CreateMap<ApplicationUser, UserFormViewModel>();
			CreateMap<ApplicationUser, RestPasswordViewModel>();

			//Subscribers
			CreateMap<SubscriberFormViewModel, Subscriber>().ReverseMap();
			CreateMap<Subscriber, SubscriberSearchResultViewModel>()
				.ForMember(des => des.FullName, opt => opt.MapFrom(src => $"{src.FristName} {src.LastName}"));
			CreateMap<Subscriber, SubscriberViewModel>()
					 .ForMember(des => des.FullName, opt => opt.MapFrom(src => $"{src.FristName} {src.LastName}"))
					 .ForMember(des => des.Area, opt => opt.MapFrom(src => src.Area!.Name))
					 .ForMember(des => des.Governorate, opt => opt.MapFrom(src => src.Governorate!.Name));
			CreateMap<RenewalSubscribtion, RenewalSubscribtionViewModel>();

			//Rental
			CreateMap<Rental, RentalViewModel>();
			CreateMap<RentalCopy, RentalCopyViewModel>();

			//BookCopyRentalHistory
			CreateMap<RentalCopy, BookCopyRentalHistoryViewModel>()
					.ForMember(des => des.SubscriberName, opt => opt.MapFrom(src => $"{src.Rental!.Subscriber!.FristName} {src.Rental!.Subscriber!.LastName}"))
					.ForMember(des => des.SubscriberNum, opt => opt.MapFrom(src => src.Rental!.Subscriber!.MobilNum));

		}
	}
}
