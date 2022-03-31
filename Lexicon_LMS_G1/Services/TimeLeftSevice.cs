namespace Lexicon_LMS_G1.Services
{
    public class TimeLeftSevice : ITimeLeftSevice
    {
        public (string, string) GetTimeLeft(DateTime endDate)
        {
            var timeleft = endDate - DateTime.Now;
            if (timeleft.TotalHours < 1)
                return (timeleft.Minutes.ToString() + " min", "border-warning");
            else if (timeleft.TotalDays < 1)
            {
                return timeleft.Hours.ToString() + " h";
            }
            else if (timeleft.Days < 8)
                return timeleft.Days.ToString() + " days";
            else
                return (timeleft.Days / 7).ToString() + " weeks";
        }
    }
}
