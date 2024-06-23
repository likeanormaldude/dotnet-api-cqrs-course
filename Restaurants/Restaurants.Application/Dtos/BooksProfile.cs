using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos;

public class BooksProfile: Profile
{
    public BooksProfile()
    {
        CreateMap<Book, BookDto>();
    }
}
