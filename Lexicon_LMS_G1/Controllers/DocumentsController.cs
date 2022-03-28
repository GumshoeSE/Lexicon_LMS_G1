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
using Lexicon_LMS_G1.Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Lexicon_LMS_G1.Entities;

namespace Lexicon_LMS_G1.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly string documentBasePath = GlobalStatics.SaveDocumentBase;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public DocumentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
            if (!Directory.Exists(documentBasePath))
            {
                Directory.CreateDirectory(documentBasePath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            return await UploadDocument<int, Module>();
        }

        private async Task<IActionResult> UploadDocument<T, Q>() where Q : class
        {
            DocumentUploadViewModel<T, Q> view = new DocumentUploadViewModel<T, Q>()
            {
                Identifier = await _context.Set<Q>().ToListAsync()
            };

            switch (typeof(Q).Name)
            {
                case "Course":
                    return View("UploadCoursePartialView", view);
                case "Module":
                    return View("UploadModulePartialView", view);
                case "Activity":
                    return View("UploadActivityPartialView", view);
                case "Student":
                    return View("UploadStudentPartialView", view);
            }

            return StatusCode(500, $"{typeof(Q).Name} is unavailable to upload for.");
        }

        [HttpPost]
        public async Task<IActionResult> UploadCourseDocument(IFormFile document, int courseId, string description)
        {
            string path = await SaveFileGetPath(document, "Course");
            ApplicationUser user = await userManager.GetUserAsync(User);
            CourseDocument courseDocument = new CourseDocument()
            {
                Name = document.FileName,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                CourseId = courseId,
                UserId = user.Id
            };
            _context.Add(courseDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        [HttpPost]
        public async Task<IActionResult> UploadModuleDocument(IFormFile document, int moduleId, string description)
        {
            string path = await SaveFileGetPath(document, "Module");
            ApplicationUser user = await userManager.GetUserAsync(User);
            ModuleDocument moduleDocument = new ModuleDocument()
            {
                Name = document.Name,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                ModuleId = moduleId,
                UserId = user.Id
            };
            _context.Add(moduleDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        [HttpPost]
        public async Task<IActionResult> UploadActivityDocument(IFormFile document, int activityId, string description)
        {
            string path = await SaveFileGetPath(document, "Activity");
            ApplicationUser user = await userManager.GetUserAsync(User);
            ActivityDocument activityDocument = new ActivityDocument()
            {
                Name = document.Name,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                ActivityId = activityId,
                UserId = user.Id
            };
            _context.Add(activityDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        [HttpPost]
        public async Task<IActionResult> UploadStudentDocument(IFormFile document, string description, int activityId)
        {
            string path = await SaveFileGetPath(document, "Student");
            ApplicationUser user = await userManager.GetUserAsync(User);
            StudentDocument studentDocument = new StudentDocument()
            {
                Name = document.Name,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                UserId = user.Id,
                ActivityId = activityId,
                IsApproved = false
            };
            _context.Add(studentDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        private IActionResult UploadDocumentReturnTo()
        {
            return Ok();
        }

        private async Task<string> SaveFileGetPath(IFormFile file, string folder = "default")
        {
            string directory = $"{documentBasePath}{Path.DirectorySeparatorChar}{folder}";
            string filename;
            string whereToSave;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            do
            {
                filename = Path.GetRandomFileName();
                whereToSave = $"{directory}{Path.DirectorySeparatorChar}{filename}";
            }
            while (System.IO.File.Exists(whereToSave));
            

            using (var stream = System.IO.File.Create(whereToSave))
            {
                await file.CopyToAsync(stream);
            }

            return whereToSave;
        }

    }
}
