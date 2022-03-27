using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lexicon_LMS_G1.Services
{
    public class ActivityTypeSelectListService : IActivityTypeSelectListService
    {
        private readonly IBaseRepository<ActivityType> _repo;

        public ActivityTypeSelectListService(IBaseRepository<ActivityType> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<SelectListItem>> GetSelectListAsync()
        {
            var activityTypeList = new List<SelectListItem>();
            var activityTypes = await _repo.GetAsync();

            foreach (var activityType in activityTypes)
            {
                activityTypeList.Add(new SelectListItem { Text = activityType.Name, Value = activityType.Id.ToString() });
            }

            return activityTypeList;
        }
    }
}
