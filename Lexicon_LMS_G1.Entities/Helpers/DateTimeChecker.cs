using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Entities.Helpers
{
    public static class DateTimeChecker
    {
        public static bool IsOverlappingWithList(DateTime startTime, DateTime endTime, ICollection<Module> modules)
        {
            bool isOverlapping = false;

            foreach (var module in modules)
            {
                if (startTime.Ticks > module.StartTime.Ticks && startTime.Ticks < module.EndTime.Ticks)
                {
                    isOverlapping = true;
                    break;
                }
                else if (endTime.Ticks > module.EndTime.Ticks && endTime.Ticks < module.StartTime.Ticks)
                {
                    isOverlapping = true;
                    break;
                }
            }

            return isOverlapping;
        }
    }
}
