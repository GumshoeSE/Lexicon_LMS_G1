using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Lexicon_LMS_G1.Entities.ViewModels;
using Lexicon_LMS_G1.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lexicon_LMS_G1.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly IConfiguration _config;

        public UsersController(UserManager<ApplicationUser> userManager,
                               IMapper mapper,
                               ApplicationDbContext context,
                               IConfiguration configuration
                               )
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.context = context;
            this._config = configuration;
        }

        //GET: Users
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index(int? pageIndex = 1, string? searchQuery = null)
        {            
            if (!int.TryParse(_config["LMS:Users:PageSize"], out int pageSize))
                pageSize = 5;  
            
            var users = userManager.Users as IQueryable<ApplicationUser>;
            users = users.Include(c => c.Course);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();

                users = users.Where(s => 
                    s.UserName.Contains(searchQuery) ||
                    s.LastName.Contains(searchQuery) ||
                    s.FirstName.Contains(searchQuery) ||
                    s.Email.Contains(searchQuery) ||
                    (s.Course != null) && s.Course.Name.Contains(searchQuery)
                );
            }
                      
            var userCount = users.Count();

            var totalPages = (int)Math.Ceiling(userCount / (double)pageSize);

            pageIndex ??= 1;
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex > totalPages) pageIndex = totalPages;

            var usersToReturn = new List<ApplicationUser>();
            if (userCount > 0)
            {
                usersToReturn = await users
                    .OrderBy(o => o.FirstName).ThenBy(o => o.LastName)
                    .Skip(((int)pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            List<UserViewModel> viewUsers = new List<UserViewModel>();
            for (int i = 0; i < usersToReturn.Count; i++)
            {
                var user = mapper.Map<UserViewModel>(usersToReturn[i]);
                user.Role = (await userManager.GetRolesAsync(usersToReturn[i])).First().ToString();
                viewUsers.Add(user);
            }


            var viewUser = new UsersViewModel
            {

                Users = viewUsers,

                CurrentPageIndex = (int)pageIndex,
                TotalCount = userCount,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchQuery = searchQuery
            };
            //ToDo Paginationhandling of big values as parameters
            //pageIndex>=1073741825 ger detta error
            //SqlException: The offset specified in a OFFSET clause may not be negative.

            return View(viewUser);
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await context.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(u => u.Id == id);
            var user = mapper.Map<UserDetailsViewModel>(@module);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await context.Modules.FindAsync(id);
            if (@module == null)
            {
                return NotFound();
            }
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
                    context.Update(@module);
                    await context.SaveChangesAsync();
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
            return View(@module);
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await context.Modules.FindAsync(id);
            context.Modules.Remove(@module);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
            return context.Modules.Any(e => e.Id == id);
        }
    }
}
