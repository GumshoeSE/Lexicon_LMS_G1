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
            
            //pageIndex = pageIndex ?? 1;
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex >= int.MaxValue / 2 - (pageSize + 1))
                pageIndex = int.MaxValue / 2 - 1 - pageSize;
            //var userCount = userManager.Users.Count();

            var users = userManager.Users as IQueryable<ApplicationUser>;
            users = users.Include(c => c.Course).Include(c => c.FinishedActivities);

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

            var userCount = users.Count();

            var viewUsers = mapper.Map<List<UserViewModel>>(usersToReturn);

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
                .Include(u => u.FinishedActivities)
                .ThenInclude(a => a.Activity)
                .FirstOrDefaultAsync(u => u.Id == id);
            var user = mapper.Map<UserDetailsViewModel>(@module);
            user.Role = (await userManager.GetRolesAsync(@module)).First().ToString();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @module = await context.Users.Include(u => u.Course).FirstOrDefaultAsync(u => u.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            var user = mapper.Map<UserEditViewModel>(@module);
            return View(user);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                var user = await userManager.FindByIdAsync(viewModel.Id);
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.PhoneNumber = viewModel.PhoneNumber;
                user.CourseId= viewModel.CourseId;
                user.Email = viewModel.Email;
                user.UserName = user.Email;
                
                try
                {
                    
                    var user1 = await userManager.UpdateAsync(user);
                    
                    
                    //await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(viewModel);
        }
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @module = await context.Modules.FindAsync(id);
            context.Modules.Remove(@module);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
