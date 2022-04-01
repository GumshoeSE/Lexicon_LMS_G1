using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Services
{
    public class BorderColorService : IBorderColorService
    {
        public string GetBorderColor(ActivityType activityType)
        {
            if (activityType.Name == "Lecture")
                return "border-info";
            if (activityType.Name == "Exercise")
                return "border-success";
            if (activityType.Name == "Assignment")
                return "border-danger";

            return "border-warning";
        }
    }
}
