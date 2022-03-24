using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Services
{
    public interface IBorderColorService
    {
        string GetBorderColor(ActivityType activityType);
    }
}