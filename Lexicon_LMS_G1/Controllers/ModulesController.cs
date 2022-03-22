﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Data.Data;
using AutoMapper;
using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.ViewModels;
using Lexicon_LMS_G1.Entities.Helpers;

namespace Lexicon_LMS_G1.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Module> _moduleRepo;
        private readonly IBaseRepository<Course> _courseRepo;

        public ModulesController(ApplicationDbContext context, IMapper mapper,
            IBaseRepository<Module> moduleRepo, IBaseRepository<Course> courseRepo)
        {
            _context = context;
            _mapper = mapper;
            _moduleRepo = moduleRepo;
            _courseRepo = courseRepo;
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Modules.Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _moduleRepo.GetByIdWithIncludedAsync(m => m.Activities, m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ModuleDetailsViewModel>(module);

            return View(viewModel);
        }

        // GET: Modules/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepo.GetByIdWithIncludedAsync(c => c.Modules, c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            course.Modules = course.Modules.OrderBy(m => m.StartTime).ToList();

            // ToDo: Check if seconds is needed or not (validation)
            // set default start time to the when the last module ends and remove seconds.
            var lastModuleEndDateTime = course.Modules.Last().EndTime;
            var defaultStartTime = lastModuleEndDateTime.Date.Add(
                new TimeSpan(lastModuleEndDateTime.TimeOfDay.Hours, lastModuleEndDateTime.TimeOfDay.Minutes, 0));

            var viewModel = new ModuleCreateViewModel
            {
                CourseId = course.Id,
                Course = course,
                StartTime = defaultStartTime,
                EndTime = defaultStartTime,
            };

            return View(viewModel);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuleCreateViewModel viewModel)
        {
            var course = await _courseRepo.GetByIdWithIncludedAsync(c => c.Modules, c => c.Id == viewModel.CourseId);
            viewModel.Course = course;

            if (viewModel.StartTime.Ticks < course.StartTime.Ticks)
            {
                ModelState.AddModelError("StartTime", $"The module can not start before the course.");
                return View(viewModel);
            }

            if (viewModel.StartTime.Ticks >= viewModel.EndTime.Ticks)
            {
                ModelState.AddModelError("EndTime", $"The module has to start before it ends.");
                ModelState.AddModelError("StartTime", $"The module has to start before it ends.");
                return View(viewModel);
            }

            var (isOverlap, conMod) = DateTimeChecker.IsOverlappingWithList(viewModel.StartTime, viewModel.EndTime, course.Modules);

            if (isOverlap)
            {
                ModelState.AddModelError("EndTime", $"Duration is overlapping with another module '{conMod.Name}'");
                ModelState.AddModelError("StartTime", $"Duration is overlapping with another module '{conMod.Name}'");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                var module = _mapper.Map<Module>(viewModel);

                _moduleRepo.Add(module);
                await _moduleRepo.SaveChangesAsync();

                TempData["message"] = "Module successfully added!";
                return RedirectToAction("IndexTeacher", "Courses");
            }

            return View(viewModel);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await _context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Description", @module.CourseId);
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartTime,EndTime,CourseId")] Module @module)
        {
            if (id != @module.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Description", @module.CourseId);
            return View(@module);
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var @module = await _context.Modules
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await _context.Modules.FindAsync(id);
            _context.Modules.Remove(@module);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}
