using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"))
                .ReverseMap();

            CreateMap<Book, BookDto>()
                .ForMember(dest=>dest.AuthorFullName, opt =>opt.MapFrom(src=>$"{src.Author.Name}{src.Author.Surname}"))
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))////////////////////////////
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));
        }
    }
}
