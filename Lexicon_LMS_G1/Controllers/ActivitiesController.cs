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
using Lexicon_LMS_G1.Entities.Paging;
using Lexicon_LMS_G1.Entities.ViewModels;

namespace Lexicon_LMS_G1.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
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

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetActionsForCourse(int courseId, int? pageIndex, string activityType)
        {
            var paging = new ActivitiesPagingParams()
            {
                PageIndex = pageIndex ?? 1
            };

            var modules = _context.Modules.Where(m => m.CourseId == courseId).ToList();
            List<ActivityTeacherViewModel> activites = new List<ActivityTeacherViewModel>();
            foreach (var module in modules)
            {
                var moduleactivities = _context.Activities.Include(a => a.ActivityType).Where(a => a.ModuleId == module.Id).OrderBy(a => a.StartDate).ToList();
                var viewmodel = moduleactivities.Select(a => new ActivityTeacherViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    ModuleId = a.ModuleId,
                    ActivityType = a.ActivityType,
                    Module = a.Module,
                    ActivityTypeId = a.ActivityTypeId,
                    Description = a.Description
                });
                if(activityType != null)
                {
                    if(activityType != "all")
                    viewmodel = viewmodel.Where(a => a.ActivityType.Name == activityType);
                }
                foreach (var activity in viewmodel)
                {
                    
                    activites.Add(activity);
                }
            }
            ViewData["activityType"] = _context.ActivitiesTypes.ToList();

            return PartialView("CourseActivitiesPartialView", await PaginatedList<ActivityTeacherViewModel>.CreateAsync(activites.AsEnumerable().ToList(), paging.PageIndex, paging.PageSize, courseId));
        }
    }
}
