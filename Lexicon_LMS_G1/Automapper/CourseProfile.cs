using AutoMapper;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Models.ViewModels;

namespace Lexicon_LMS_G1.Automapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseCreateViewModel>().ReverseMap();
            //CreateMap<Activity, UserFinishedActivity>();
        }
    }
}
