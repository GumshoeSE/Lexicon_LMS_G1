using AutoMapper;
using Lexicon_LMS_G1.Automapper;
using Lexicon_LMS_G1.Controllers;
using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Entities.Paging;
using Lexicon_LMS_G1.Entities.ViewModels;
using Lexicon_LMS_G1.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Tests
{
    [TestClass]
    public class CourseControllerTests
    {
        public TestContext? TestContext { get; set; }
        private static ApplicationDbContext context;
        private static ICourseRepository courseRepository;
        private static IBaseRepository<Course> baseRepository;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private CoursesController controller;
        private static IMapper mapper;

        [ClassInitialize]
        public static void ClassSetUp(TestContext testContext)
        {
            testContext.WriteLine(testContext.TestName);
            testContext.WriteLine("CoursControllerTests starts");

            mapper = new Mapper(new MapperConfiguration(mcf =>
            {
                mcf.AddProfile<CourseProfile>();
            }));

            context = CreateContext();
            courseRepository = null;


            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(
                     new Course
                     {
                         Name = "Course 1",
                         Description = "lorem1",
                         StartTime = DateTime.Now.AddDays(1),

                     }, new Course
                     {
                         Name = "Course 2",
                         Description = "lorem2",
                         StartTime = DateTime.Now.AddDays(1),
                         Modules = new List<Module> {
                             new Module {
                                 Name = "Modul 1",
                                 Description = "Descrition",
                                 StartTime=DateTime.Now.AddDays(1),
                                 EndTime=DateTime.Now.AddDays(1).AddHours(1),
                             }
                         }
                     });


            context.SaveChanges();
        }

        [TestInitialize]
        public void TestSetUp()
        {
            TestContext.WriteLine($"TestInit starts");

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            courseRepository = new CourseRepository(context);
            baseRepository = new CourseRepository(context);


            controller = new CoursesController(context, courseRepository, mapper, baseRepository, mockUserManager.Object);
            var mockTempData = new Mock<ITempDataDictionary>();
            controller.TempData = mockTempData.Object;
        }

        #region Index

        [TestMethod]
        public async Task Index_AuthorizedTeacher_ShouldReturnPaginatedListCourseViewModel()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticatedWithRole(true, "Teacher", true);

            var result = (ViewResult)await controller.IndexTeacher(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(PaginatedList<CourseViewModel>));
            Assert.AreEqual(typeof(ViewResult), result.GetType());

            var test = result;

            Assert.AreEqual(typeof(PaginatedList<CourseViewModel>), test?.Model?.GetType());

            PaginatedList<CourseViewModel> list = (PaginatedList<CourseViewModel>)(test.Model);

            Assert.IsNotNull(list);
            var c1 = list.Find(c => c.Name == "Course 1");
            var c2 = list.Find(c => c.Name == "Course 2");
            Assert.AreEqual("Course 1", c1.Name);
            Assert.AreEqual("Course 2", c2.Name);

            Assert.AreEqual("Modul 1", c2.Modules.First().Name);
        }

        [TestMethod]
        public async Task Index_NotAuthorizedTeacher_ShouldReturnNotAuthorized()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            //controller.SetUserIsAutenticatedWithRole(false, "Teacher", true);
            controller.SetUserIsAutenticated(false);

            var result = (ViewResult)await controller.IndexTeacher(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(PaginatedList<CourseViewModel>));
            Assert.AreEqual(typeof(ViewResult), result.GetType());

            var test = result;

            Assert.AreEqual(typeof(PaginatedList<CourseViewModel>), test?.Model?.GetType());

            PaginatedList<CourseViewModel> list = (PaginatedList<CourseViewModel>)(test.Model);

            Assert.IsNotNull(list);

        }


        [TestMethod]
        public async Task Index_AuthorizedTeacher_ShouldReturnIndexTeacher()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticatedWithRole(true, "Teacher", true);

            var result = await controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual(typeof(RedirectToActionResult), result.GetType());

            var action = result as RedirectToActionResult;

            Assert.AreEqual("IndexTeacher", action.ActionName);
        }

        [TestMethod]
        public async Task Index_AuthorizedStudent_ShouldReturnStudentIndex()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticatedWithRole(true, "Student", true);

            var result = await controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual(typeof(RedirectToActionResult), result.GetType());

            var action = result as RedirectToActionResult;

            Assert.AreEqual("StudentIndex", action.ActionName);

        }

        [TestMethod]
        public async Task Index_Authorized_ShouldReturnCourseView()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticatedWithRole(true, "NoRole", true);

            var result = await controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(typeof(ViewResult), result.GetType());

            var viewResult = result as ViewResult;

            Assert.IsNull(viewResult.ViewName);
            Assert.AreEqual(typeof(List<Course>), viewResult.Model.GetType() );
        }
        #endregion Index

        #region Create
        [TestMethod]
        public async Task Create_IsAuthorisedTeacher_ShouldReturnViewModel()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticatedWithRole(true, "Teacher", true);

            var courseCountBefore = courseRepository.GetAsync().Result.Count();
            var result = await controller.Create(new CourseCreateViewModel { Name = "Testcourse1", Description = "Hej hopp", StartTime = DateTime.Now });
            var courseCountAfter = courseRepository.GetAsync().Result.Count();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual(typeof(RedirectToActionResult), result.GetType());

            var test = result as RedirectToActionResult;
            Assert.AreEqual("Index", test.ActionName);
            Assert.AreEqual(courseCountBefore + 1, courseCountAfter);

            //var t2 = test?.ViewData.Model;

            //Assert.AreEqual("", test?.Model?.GetType());

        }



        #endregion Create








        private static ApplicationDbContext CreateContext()
        {

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Lexicon_LMS_G1.Tests;Trusted_Connection=True;MultipleActiveResultSets=true")
                                                            .Options;

            return new ApplicationDbContext(contextOptions);
        }
    }
}
