namespace Lexicon_LMS_G1.Services
{
    public class TimeLeftSevice : ITimeLeftSevice
    {
        public string GetTimeLeft(DateTime endDate, bool border)
        {
            var timeleft = endDate - DateTime.Now;
            if (timeleft.TotalDays < 0)
            {
                if (timeleft.TotalDays < -1)
                    return $"{timeleft.Days * -1} d, {timeleft.Hours * -1} h";
                else if (timeleft.Days > -1)
                    return timeleft.Hours.ToString() + " h";
                else
                    return timeleft.Minutes.ToString() + " min";
            }
            else
            {
                if (timeleft.Days < 1 && timeleft.Hours < 1)
                    return border ? "border-warning" : $"TODAY! {timeleft.Minutes} min!!!";

                else if (timeleft.TotalDays < 1)
                    return border ? "border-warning" : $"TODAY! {timeleft.Hours} h";

                else if (timeleft.Days < 8)
                    return border ? "border-info" : timeleft.Days.ToString() + " days";

                else
                    return border ? "border-secondary" : (timeleft.Days / 7).ToString() + " weeks";
            }
        }
    }
}
