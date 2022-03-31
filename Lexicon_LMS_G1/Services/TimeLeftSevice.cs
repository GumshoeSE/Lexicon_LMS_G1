namespace Lexicon_LMS_G1.Services
{
    public class TimeLeftSevice : ITimeLeftSevice
    {
        public string GetTimeLeft(DateTime endDate, bool border)
        {
            var timeleft = endDate - DateTime.Now;
            if (timeleft.TotalHours < 1)
                return border ? "border-warning" : timeleft.Minutes.ToString() + " min";

            else if (timeleft.TotalDays < 1)
                return border ? "border-warning" : timeleft.Hours.ToString() + " h";

            else if (timeleft.Days < 8)
                return border ? "border-info" : timeleft.Days.ToString() + " days";

            else
                return border ? "border-secondary" : (timeleft.Days / 7).ToString() + " weeks";
        }
    }
}
