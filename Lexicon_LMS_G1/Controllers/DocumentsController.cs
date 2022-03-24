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

namespace Lexicon_LMS_G1.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly string documentBasePath = "root";
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
            if (!Directory.Exists(documentBasePath))
            {
                Directory.CreateDirectory(documentBasePath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DocumentUploadViewModel<int, Course> view = new DocumentUploadViewModel<int, Course>()
            {
                Identifier = await _context.Courses.ToListAsync()
            };
            return View(view);
        }

        [HttpGet]
        public async Task<IActionResult> UploadDocument<T, Q>() where Q : BaseDocument
        {
            DocumentUploadViewModel<T, Q> view = new DocumentUploadViewModel<T, Q>()
            {
                Identifier = await _context.Set<Q>().ToListAsync()
            };

            return View(view);
        }

        [HttpPost]
        public async Task<IActionResult> UploadCourseDocument(IFormFile document, int courseId, string description)
        {
            string path = await SaveFileGetPath(document, "Course");
            CourseDocument courseDocument = new CourseDocument()
            {
                Name = document.FileName,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                CourseId = courseId
            };
            _context.Add(courseDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        [HttpPost]
        public async Task<IActionResult> UploadModuleDocument(IFormFile document, int moduleId, string description)
        {
            string path = await SaveFileGetPath(document, "Module");
            ModuleDocument moduleDocument = new ModuleDocument()
            {
                Name = document.Name,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                ModuleId = moduleId
            };
            _context.Add(moduleDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        [HttpPost]
        public async Task<IActionResult> UploadActivityDocument(IFormFile document, int activityId, string description)
        {
            string path = await SaveFileGetPath(document, "Activity");
            ActivityDocument activityDocument = new ActivityDocument()
            {
                Name = document.Name,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                ActivityId = activityId
            };
            _context.Add(activityDocument);
            await _context.SaveChangesAsync();

            return UploadDocumentReturnTo();
        }

        [HttpPost]
        public async Task<IActionResult> UploadStudentDocument(IFormFile document, string studentId, string description)
        {
            string path = await SaveFileGetPath(document, "Student");
            StudentDocument studentDocument = new StudentDocument()
            {
                Name = document.Name,
                FileType = document.ContentType,
                Description = description,
                CreatedOn = DateTime.Now,
                FilePath = path,
                UserId = studentId
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
