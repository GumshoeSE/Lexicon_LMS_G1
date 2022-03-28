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

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.context = context;
        }

        //GET: Users
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index(int? pageIndex = 1, string? searchQuery = null)
        {
            //var searchQuery = "banan";
            int pageSize = 2;
            var max = int.MaxValue;
            pageIndex ??= 1;

            //pageIndex = pageIndex ?? 1;
            if (pageIndex < 1) pageIndex = 1;
            if (pageIndex >= int.MaxValue / 2 - (pageSize + 1))
                pageIndex = int.MaxValue / 2 - 1 - pageSize;
            //var userCount = userManager.Users.Count();

            var users = userManager.Users as IQueryable<ApplicationUser>;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                users = users.Where(s =>
                    s.UserName.Contains(searchQuery.Trim()) ||
                    s.LastName.Contains(searchQuery.Trim()) ||
                    s.FirstName.Contains(searchQuery.Trim()) ||
                    s.Email.Contains(searchQuery.Trim())

                );
            }
            var usersToReturn = await users.OrderBy(o => o.FirstName).ThenBy(o => o.LastName)
                .Skip(((int)pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userCount = users.Count();

            var viewUsers = mapper.Map<List<UserViewModel>>(usersToReturn);

            var viewUser = new UsersViewModel
            {

                Users = viewUsers,
                CurrentPageIndex = (int)pageIndex,
                TotalCount = userCount,
                TotalPages = (int)Math.Ceiling(userCount / (double)pageSize),
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
