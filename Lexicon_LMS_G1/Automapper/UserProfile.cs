﻿using AutoMapper;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Entities.ViewModels;

namespace Lexicon_LMS_G1.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForPath(dest=>dest.CourseName, opt=>opt.MapFrom(src=>src.Course.Name));
            CreateMap<ApplicationUser, UserDetailsViewModel>().ReverseMap();
            CreateMap<ApplicationUser, UserEditViewModel>().ReverseMap();
            
        }
    }
}
