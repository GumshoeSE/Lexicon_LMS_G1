#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Data.Repositores;
using System.Text.Json;
using Lexicon_LMS_G1.Entities.Dtos;
using AutoMapper;
using Lexicon_LMS_G1.Entities.Helpers;
using System.Net;
using Lexicon_LMS_G1.Entities.Paging;
using Lexicon_LMS_G1.Entities.ViewModels;
using AutoMapper;
using Lexicon_LMS_G1.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lexicon_LMS_G1.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository<Activity> _repo;
        private readonly IBaseRepository<Module> _baseModuleRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivitiesController(ApplicationDbContext context, IBaseRepository<Activity> repo,
            IBaseRepository<Module> baseModuleRepo, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _repo = repo;
            _baseModuleRepo = baseModuleRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Activities.Include(a => a.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,ModuleId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "Description", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteViewModel viewModel)
        {
            var activity = await _context.Activities
                .Include(a => a.Documents)
                .FirstOrDefaultAsync(v => v.Id == viewModel.DeleteId);

            foreach(var file in activity.Documents)
            {
                if (System.IO.File.Exists(file.FilePath))
                    System.IO.File.Delete(file.FilePath);
            }

            _context.RemoveRange(activity.Documents);
            await _context.SaveChangesAsync();

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            TempData["message"] = $"Activity '{activity.Name}' removed ";

            return RedirectToAction("Details", "Modules", new { id = viewModel.ReturnId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivity([FromBody] ActivityUpdateDto dto)
        {
            var orginialActivity = _repo.GetById(dto.Id);

            if (orginialActivity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var activity = _mapper.Map(dto, orginialActivity);
                _repo.Update(activity);
                await _repo.SaveChangesAsync();

                TempData["message"] = "Activity successfully updated!";
                return Json(new { redirectToUrl = Url.Action("Details", "Modules", new { id = activity.ModuleId }) });
            }

            return Json(false);
        }

        [HttpPut]
        public async Task<IActionResult> PutActivity(ActivityCreateDto dto)
        {
            var activity = _mapper.Map<Activity>(dto);
            var module = await _baseModuleRepo.GetByIdWithIncludedAsync(m => m.Activities, m => m.Id == activity.ModuleId);


            if (activity.StartDate.Ticks < module.StartTime.Ticks)
            {
                ModelState.AddModelError("StartDate", $"The activity can not start before the module starts ({module.StartTime}).");
            }
            else if (activity.StartDate.Ticks > module.EndTime.Ticks)
            {
                ModelState.AddModelError("StartDate", $"The activity can not start after the module ends ({module.EndTime}).");
            }

            if (activity.EndDate.Ticks < module.StartTime.Ticks)
            {
                ModelState.AddModelError("EndDate", $"The activity can not end before the module starts ({module.StartTime}).");
            }
            else if (activity.EndDate.Ticks > module.EndTime.Ticks)
            {
                ModelState.AddModelError("EndDate", $"The activity can not end after the module ends ({module.EndTime}).");
            }

            if (activity.StartDate.Ticks >= activity.EndDate.Ticks)
            {
                ModelState.AddModelError("EndDate", $"The activity has to start before it ends.");
                ModelState.AddModelError("StartDate", $"The activity has to start before it ends.");
            }

            var (isOverlap, conMod) = DateTimeChecker.IsOverlappingWithList(activity.StartDate, activity.EndDate, module.Activities);

            if (isOverlap)
            {
                ModelState.AddModelError("EndDate", $"Duration is overlapping with another activity '{conMod.Name}'");
                ModelState.AddModelError("StartDate", $"Duration is overlapping with another activity '{conMod.Name}'");
            }

            if (ModelState.IsValid)
            {

                if (dto.Document != null)
                {
                    string file;

                    do
                    {
                        file = $"{GlobalStatics.SaveDocumentModule}{Path.DirectorySeparatorChar}{Path.GetRandomFileName()}";
                    }
                    while (System.IO.File.Exists(file));

                    var doc = new ActivityDocument()
                    {
                        Name = dto.Document.FileName,
                        FileType = dto.Document.ContentType,
                        Description = dto.DocumentDescription,
                        CreatedOn = DateTime.Now,
                        FilePath = file,
                        Activity = activity,
                        UserId = _userManager.GetUserId(User)
                    };

                    using (var stream = System.IO.File.Create(file))
                    {
                        await dto.Document.CopyToAsync(stream);
                    }
                    _context.Add(doc);

                    activity.Documents.Add(doc);
                }

                _repo.Add(activity);
                await _repo.SaveChangesAsync();

                TempData["message"] = "Activity successfully added!";

                return Json(new {
                    success = true,
                    redirectToUrl = Url.Action("Details", "Modules", new { id = activity.ModuleId }) });
            }

            var modelErrors = new List<object>();

            foreach (var modelStateKey in ViewData.ModelState.Keys)
            {
                var value = ViewData.ModelState[modelStateKey];
                foreach (var error in value.Errors)
                {
                    modelErrors.Add(new { key = modelStateKey, message = error.ErrorMessage });
                }
            }

            return Json(new {
                success = false,
                errors = modelErrors
            });
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetActionsForCourse(int courseId, int? pageIndex, string activityType, bool showHistory)
        {
            var paging = new ActivitiesPagingParams()
            {
                PageIndex = pageIndex ?? 1
            };
            Course course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
            var modules = _context.Modules.Where(m => m.CourseId == courseId).ToList();
            List<ActivityTeacherViewModel> activites = new List<ActivityTeacherViewModel>();
            foreach (var module in modules)
            {
                var moduleactivities = _context.Activities.Include(a => a.ActivityType).Where(a => a.ModuleId == module.Id).OrderBy(a => a.StartDate);
                var viewModel = _mapper.ProjectTo<ActivityTeacherViewModel>(moduleactivities);
                
                if (activityType != null)
                {
                    if(activityType != "all")
                    viewModel = viewModel.Where(a => a.ActivityType.Name == activityType);
                }
                if (showHistory)
                {
                    viewModel = viewModel.Where(a => a.EndDate < DateTime.Now);
                }
                else 
                {
                    viewModel = viewModel.Where(a => a.EndDate >= DateTime.Now);
                }
                foreach (var activity in viewModel)
                {
                    
                    activites.Add(activity);
                }
            }
            int count = activites.Count();
            ViewData["activityType"] = _context.ActivitiesTypes.ToList();

            return PartialView("CourseActivitiesPartialView", await PaginatedList<ActivityTeacherViewModel>.CreateAsync(activites.AsEnumerable().ToList(), paging.PageIndex, paging.PageSize, count, course));
        }

       

    }
   
}
