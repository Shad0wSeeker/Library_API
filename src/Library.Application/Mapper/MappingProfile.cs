﻿using AutoMapper;
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

            CreateMap<AuthorRequestDto, Library.Domain.Models.Author>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AuthorFullName.Substring(0, src.AuthorFullName.IndexOf(' '))))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.AuthorFullName.IndexOf(' ') >= 0
                                                                              ? src.AuthorFullName.Substring(src.AuthorFullName.IndexOf(' ') + 1)
                                                                              : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));

            CreateMap<Library.Domain.Models.Author, AuthorResponseDto>()
                .ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));



            CreateMap<Library.Domain.Models.Book, BookResponseDto>()
                .ForMember(dest=>dest.AuthorId, opt=>opt.MapFrom(src=>src.AuthorId))
                .ReverseMap();

            CreateMap<BookRequestDto, Library.Domain.Models.Book>()
                .ReverseMap();



            CreateMap<UserRequestDto, Library.Domain.Models.User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap();

            CreateMap<Library.Domain.Models.User, UserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ReverseMap();
        }
    }
}
