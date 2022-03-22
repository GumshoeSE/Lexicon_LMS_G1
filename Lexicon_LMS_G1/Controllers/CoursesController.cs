#nullable disable
using AutoMapper;
using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Entities.Paging;
using Lexicon_LMS_G1.Entities.ViewModels;
using Lexicon_LMS_G1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_LMS_G1.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICourseRepository courseRepo;
        private readonly IBaseRepository<Course> repo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper _mapper;

        public CoursesController(ApplicationDbContext context, ICourseRepository courseRepo, IMapper mapper, IBaseRepository<Course> repo, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.repo = repo;
            this.userManager = userManager;
            _mapper = mapper;
            this.courseRepo = courseRepo;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction(nameof(IndexTeacher));
            }
            
            return RedirectToAction(nameof(StudentIndex));
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> IndexTeacher(int? pageIndex)
        {
            var paging = new CoursePagingParams
            {
                PageIndex = pageIndex ?? 1
            };
            var model = await courseRepo.GetCourseAsync();
            var viewModel = model.Select(c => new CourseViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                StartTime = c.StartTime,
                Modules = c.Modules,
                AttendingStudents = c.AttendingStudents,
            }).AsEnumerable();

            return View(await PaginatedList<CourseViewModel>.CreateAsync(viewModel.AsEnumerable().ToList(), paging.PageIndex, paging.PageSize));

        }


        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(CourseCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(viewModel);
                repo.Add(course);
                await repo.SaveChangesAsync();

                TempData["message"] = "Course successfully created!";
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<CourseEditViewModel>(course);
            return View(viewModel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, CourseEditViewModel viewModel)
        {
            var course = _mapper.Map<Course>(viewModel);
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseRepo.Update(course);

                    await repo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? deleteId)
        {
            if (deleteId == null)
            {
                return NotFound();
            }

            var course = repo.GetById(deleteId);
            //var course = await _context.Courses.FindAsync(id);

            if (repo.Delete(deleteId))
            {
                await repo.SaveChangesAsync();
                TempData["message"] = $"The course {course.Name} has been removed!";
                return RedirectToAction(nameof(IndexTeacher));
            }
            TempData["error"] = "Something went wrong while deleting!";
            return RedirectToAction(nameof(IndexTeacher));

        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StudentIndex()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);

            StudentViewCourseViewModel viewModel = await GetStudentViewCourseViewModel(user.CourseId);

            return View("IndexStudent", viewModel);
        }

        private async Task<StudentViewCourseViewModel> GetStudentViewCourseViewModel(int? courseId)
        {
            if (courseId == null)
                return new StudentViewCourseViewModel
                {
                    Name = "Nothing lmao",
                    Id = 0,
                    Assignments = new List<Activity>(),
                    FinishedAssignments = new List<Activity>(),
                    Attendees = new List<ApplicationUser>(),
                    Modules = new List<Module>()
                };
            /*
                        var model = _context.Courses.FirstOrDefault(c => c.Id ==courseId).Where //new StudentViewCourseViewModel
                        //{
                        //   // Assignments = c.Modules.Select(m => m.Activities).Where(a => )//Where(a => a.ActivityType.Name == "Assignment"))

                        //}).FirstOrDefault(c => c.)  

                        var modelt = _context.Courses.Select(c => new StudentViewCourseViewModel
                        {
                            Assignments = c.Modules.Select(m => m.Activities.Where(a => a.ActivityType.Name == "Assignment"))
                        });
            */
            string userId = (await userManager.GetUserAsync(User)).Id;

            var finishedAssignments = (await _context.Users
                    .Include(u => u.FinishedActivities)
                    .FirstOrDefaultAsync(u => u.Id == userId))
                    .FinishedActivities
                    .Select(f => f.Activity);

            StudentViewCourseViewModel viewModel = new StudentViewCourseViewModel
            {
                Name = (await _context.Courses.FindAsync(courseId)).Name,
                Id = (int)courseId,
                Assignments = await _context.Activities
                    .Include(a => a.ActivityType)
                    .Include(a => a.Module)
                    .ThenInclude(m => m.Activities)
                    .Where(a => a.Module.CourseId == courseId)
                    .Where(a => a.ActivityType.Name == "Assignment")
                    .ToListAsync(),
                FinishedAssignments = finishedAssignments,
                Attendees = (await _context.Courses
                    .Include(c => c.AttendingStudents)
                    .FirstOrDefaultAsync(a => a.Id == courseId))
                    .AttendingStudents,
                Modules = await _context.Modules
                    .Where(m => m.CourseId == courseId)
                    .ToListAsync()
            };

            return viewModel;
        }

        public IActionResult GetModulesForCourse(int courseId)
        {
            Course course = _context.Courses.Include(m => m.Modules).FirstOrDefault(c => c.Id == courseId);
            return PartialView("ModulePartialView", course.Modules);
        }

        public IActionResult GetActionsForModule(int moduleId)
        {
            Module module = _context.Modules.Include(m => m.Activities).FirstOrDefault(m => m.Id == moduleId);
            return PartialView("ModuleDetailsPartialView", module);
        }

        public IActionResult GetActionsForActivity(int activityId)
        {
            Activity activity = _context.Activities.FirstOrDefault(a => a.Id == activityId);
            return PartialView("ActivityDetailsPartialView", activity);
        }
    }
}
