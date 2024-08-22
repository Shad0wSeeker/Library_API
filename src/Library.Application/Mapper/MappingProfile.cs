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

            CreateMap<AuthorDto, Author>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AuthorFullName.Substring(0, src.AuthorFullName.IndexOf(' '))))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AuthorFullName.IndexOf(' ') >= 0
                                                                              ? src.AuthorFullName.Substring(src.AuthorFullName.IndexOf(' ') + 1)
                                                                              : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));




            CreateMap<Book, BookDto>()
                .ForMember(dest=>dest.AuthorId, opt=>opt.MapFrom(src=>src.AuthorId))
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))////////////////////////////
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role)));
        }
    }
}
