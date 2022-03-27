using AutoMapper;
using Lexicon_LMS_G1.Entities.Dtos;
using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Automapper
{
    public class ActivitiesProfile : Profile
    {
        public ActivitiesProfile()
        {
            CreateMap<ActivityUpdateDto, Activity>();
            CreateMap<ActivityCreateDto, Activity>();
        }
    }
}
