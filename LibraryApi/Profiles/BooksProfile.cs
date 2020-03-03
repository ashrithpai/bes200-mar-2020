using AutoMapper;
using LibraryApi.Domain;
using LibraryApi.Models;

namespace LibraryApi.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, GetABookResponse>();
            CreateMap<Book, BookSummaryItem>();
            CreateMap<PostBooksRequest, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InInventory, opt => opt.MapFrom(_ => true));
        }
    }
}
