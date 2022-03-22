using AutoMapper;
using Lexicon_LMS_G1.Controllers;
using Lexicon_LMS_G1.Data.Data;
using Lexicon_LMS_G1.Data.Repositores;
using Lexicon_LMS_G1.Entities.Entities;
using Lexicon_LMS_G1.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private static ICourseRepository repository;
        private Mock<UserManager<ApplicationUser>> mockUserManager;
        private CoursesController controller;
        private IMapper mapper;

        [ClassInitialize]
        public static void ClassSetUp(TestContext testContext)
        {
            testContext.WriteLine(testContext.TestName);
            testContext.WriteLine("CoursControllerTests starts");

            context = CreateContext();
            repository = null;

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.AddRange(
                     new Course
                     {
                         Name = "Course 1",
                         Description = "lorem",
                         StartTime =DateTime.Now.AddDays(1),
                                                  
                     }, new Course
                     {
                         Name = "Course 2",
                         Description = "lorem",
                         StartTime = DateTime.Now.AddDays(1),
                         Modules = new List<Module> { new Module {
                             Name = "Modul 1",
                             Description = "Descrition",
                             StartTime=DateTime.Now.AddDays(1), 
                             EndTime=DateTime.Now.AddDays(1).AddHours(1),
                             
                         
                         } }
                     });

            
            context.SaveChanges();
        }

        [TestInitialize]
        public void TestSetUp()
        {
            TestContext.WriteLine($"TestInit starts");

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            controller = new CoursesController(context, repository, mapper, null);
        }



        [TestMethod]
        public async Task MyTestMethodAsync()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticated(true);
            var result = await controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(typeof(ViewResult), result.GetType());
            
            var test = result as ViewResult;
            var t = test.ViewName;

            Assert.AreEqual(typeof( List<Course>), test?.Model?.GetType());

            List<Course> list = (List<Course>)(test.Model);

            Assert.IsNotNull(list);
            Assert.AreEqual("Course 1", list[0].Name);
            Assert.AreEqual("Course 2", list[1].Name);
            
        }



        [TestMethod]
        public async Task MyTestMethodAsync3()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticated(true);

            mockUserManager.Setup(mgr => mgr.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);    

            var result = await controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(typeof(ViewResult), result.GetType());

            var test = result as ViewResult;
            var t2 = test?.ViewData.Model;

            Assert.AreEqual("", test?.Model?.GetType());

        }

        [TestMethod]
        public async Task MyTestMethodAsync4()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticated(true);
            var result = await controller.Index();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(typeof(ViewResult), result.GetType());

            var test = result as ViewResult;
            var t2 = test.TempData;

            Assert.AreEqual("", test?.Model?.GetType());     
        }


        [TestMethod]
        public async Task MyTestMethodAsync2()
        {
            TestContext.WriteLine($"{TestContext.TestName} starts");
            controller.SetUserIsAutenticated(true);
            var result = await controller.IndexTeacher(1);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(typeof(ViewResult), result.GetType());

            var test = result as ViewResult;
            var t2 = test?.Model;

            Assert.AreEqual("", test?.Model?.GetType());

        }



        private static ApplicationDbContext CreateContext()
        {
            
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Lexicon_LMS_G1.Tests;Trusted_Connection=True;MultipleActiveResultSets=true")
                                                            .Options;

            return new ApplicationDbContext(contextOptions);
        }
    }
}
