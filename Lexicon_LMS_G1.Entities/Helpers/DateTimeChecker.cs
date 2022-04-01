using Lexicon_LMS_G1.Entities.Entities;

namespace Lexicon_LMS_G1.Entities.Helpers
{
    public static class DateTimeChecker
    {
        public static (bool, Module?) IsOverlappingWithList(DateTime startTime, DateTime endTime, ICollection<Module> modules)
        {

            foreach (var module in modules)
            {
                // Remove seconds or less 
                module.StartTime = module.StartTime.Date.Add(
                    new TimeSpan(module.StartTime.TimeOfDay.Hours, module.StartTime.TimeOfDay.Minutes, 0));

                module.EndTime = module.EndTime.Date.Add(
                    new TimeSpan(module.EndTime.TimeOfDay.Hours, module.EndTime.TimeOfDay.Minutes, 0));

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
                else if (startTime.Ticks < module.StartTime.Ticks && endTime.Ticks > module.EndTime.Ticks)
                {
                    return (true, module);
                }
            }

            return (false, null);
        }

        public static (bool, Activity?) IsOverlappingWithList(DateTime startTime, DateTime endTime, ICollection<Activity> activities)
        {

            foreach (var activity in activities)
            {
                // Remove seconds or less 
                activity.StartDate = activity.StartDate.Date.Add(
                    new TimeSpan(activity.StartDate.TimeOfDay.Hours, activity.StartDate.TimeOfDay.Minutes, 0));

                activity.EndDate = activity.EndDate.Date.Add(
                    new TimeSpan(activity.EndDate.TimeOfDay.Hours, activity.EndDate.TimeOfDay.Minutes, 0));

                // Starts inside the timespan of another module
                if (startTime.Ticks >= activity.StartDate.Ticks && startTime.Ticks < activity.EndDate.Ticks)
                {
                    return (true, activity);
                }
                // Ends inside the timespan of another module
                else if (endTime.Ticks < activity.EndDate.Ticks && endTime.Ticks > activity.StartDate.Ticks)
                {
                    return (true, activity);
                }
                // Starts before and ends after the timespan of another module
                else if (startTime.Ticks < activity.StartDate.Ticks && endTime.Ticks > activity.EndDate.Ticks)
                {
                    return (true, activity);
                }
            }

            return (false, null);
        }
    }
}
