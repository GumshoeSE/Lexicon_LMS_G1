using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lexicon_LMS_G1.Services
{
    public class CourseSelectListService : ICourseSelectListService
    {
        private readonly IBaseRepository<Course> _courseRepo;

        public CourseSelectListService(IBaseRepository<Course> courseRepo)
        {
            _courseRepo = courseRepo ?? throw new ArgumentNullException(nameof(courseRepo));
        }

        public async Task<List<SelectListItem>> GetSelectListAsync()
        {
            var courseList = new List<SelectListItem>();
            var courses = await _courseRepo.GetAsync();

            foreach (var course in courses)
            {
                courseList.Add(new SelectListItem { Text = course.Name, Value = course.Id.ToString() });
            }

            return courseList;
        }
    }
}
