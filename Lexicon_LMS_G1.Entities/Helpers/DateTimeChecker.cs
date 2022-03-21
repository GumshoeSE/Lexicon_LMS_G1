using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Entities.Helpers
{
    public static class DateTimeChecker
    {
        public static (bool, Module?) IsOverlappingWithList(DateTime startTime, DateTime endTime, ICollection<Module> modules)
        {

            foreach (var module in modules)
            {
                // Starts inside the timespan of another module
                if (startTime.Ticks >= module.StartTime.Ticks && startTime.Ticks < module.EndTime.Ticks)
                {
                    return (true, module);
                }
                // Ends inside the timespan of another module
                else if (endTime.Ticks < module.EndTime.Ticks && endTime.Ticks > module.StartTime.Ticks)
                {
                    return (true, module);
                }
                // Starts before and ends after the timespan of another module
                else if (startTime.Ticks <= module.StartTime.Ticks && endTime.Ticks >= module.EndTime.Ticks)
                {
                    return (true, module);
                }
            }

            return (false, null);
        }
    }
}
