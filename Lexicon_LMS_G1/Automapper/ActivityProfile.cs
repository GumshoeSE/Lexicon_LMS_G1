using AutoMapper;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Entities.ViewModels;

namespace Lexicon_LMS_G1.Automapper
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Activity, ActivityTeacherViewModel>().ReverseMap();
        }
    }
}
