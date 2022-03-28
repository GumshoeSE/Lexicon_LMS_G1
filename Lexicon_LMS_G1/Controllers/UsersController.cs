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
            int pageSize;
            if (int.TryParse(_config["LMS:Users:PageSize"], out int appsettingPageSize))
                pageSize = appsettingPageSize;
            else
                pageSize = 5;

            var max = int.MaxValue;
            pageIndex ??= 1;

            if (pageIndex < 1) pageIndex = 1;

            var users = userManager.Users as IQueryable<ApplicationUser>;
            users = users.Include(c => c.Course);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();

                users = users.Where(s =>
                    (s.UserName.Contains(searchQuery) ||
                    s.LastName.Contains(searchQuery) ||
                    s.FirstName.Contains(searchQuery) ||
                    s.Email.Contains(searchQuery) ||
                    (s.Course != null) && s.Course.Name.Contains(searchQuery)
                ));
            }

            var userCount = users.Count();
            var totalPages = (int)Math.Ceiling(userCount / (double)pageSize);
            if (pageIndex > totalPages)
                pageIndex = totalPages;
            var usersToReturn = new List<ApplicationUser>();
            if (userCount > 0)
            {
                usersToReturn = await users
                    .OrderBy(o => o.FirstName).ThenBy(o => o.LastName)
                    .Skip(((int)pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            }
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
    }
}
