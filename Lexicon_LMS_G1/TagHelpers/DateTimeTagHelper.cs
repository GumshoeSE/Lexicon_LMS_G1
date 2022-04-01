using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.Helpers.TagHelpers
{
    [HtmlTargetElement("time")]
    public class DateTimeTagHelper : TagHelper
    {
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("me-3", HtmlEncoder.Default);
            output.AddClass("d-flex", HtmlEncoder.Default);
            output.Attributes.SetAttribute("style", "width:100%");

            var (timeText, statusText, badgeType) = GetBadge();

            output.Content.SetHtmlContent(
                $"<div class='left text-start'>"
                    + $"{Title}" +
                $"</div>" +
                $"<div class='badge {badgeType}'>"
                    + $"{statusText}" +
                $"</div>" +
                $"<div class='right text-end'>"
                    + $"{timeText}" + 
                $"</div>");

            output.TagMode = TagMode.StartTagAndEndTag;
        }

        private (string timeText, string statusText, string badgeType) GetBadge()
        {
            var timeText = ""; // 2022-03-01, 5 days, 10 h 50 min
            var statusText = ""; // finished, current, planned
            var badgeType = ""; // green, red, blue

            var startDif = StartDate - DateTime.Now;
            var endDif = EndDate - DateTime.Now;

            var hasStarted = startDif.CompareTo(TimeSpan.Zero) < 0;
            var hasEnded = endDif.CompareTo(TimeSpan.Zero) < 0;

            var isOngoing = hasStarted && !hasEnded;
            var isToday = startDif.TotalDays < 1;
            var isThisWeek = startDif.TotalDays <= 7;

            if (hasEnded)
            {
                statusText += "finished";
                badgeType += "bg-success";
                timeText += $"{EndDate.ToString("yyyy/MM/dd")}";
            }
            else if (isOngoing)
            {
                statusText += "current";
                badgeType += "bg-warning text-dark";
            }
            else
            {
                statusText += "planned";
                badgeType += "bg-info text-dark";

                if (isToday)
                {
                    timeText += (startDif.Hours > 0) ? $"{startDif.Hours} h " : $"";
                    timeText += (startDif.Minutes > 0) ? $"{startDif.Minutes} min " : $"";
                }
                else if (isThisWeek)
                {
                    timeText += $"{startDif.Days} days";
                }
                else
                {
                    timeText += $"{StartDate.ToString("yyyy/MM/dd")}";
                }
            }

            return (timeText, statusText, badgeType);
        }
    }
}
