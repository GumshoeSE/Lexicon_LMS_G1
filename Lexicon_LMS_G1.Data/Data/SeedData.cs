﻿using Bogus;
using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Data.Data
{
    public static class SeedData
    {
        private static Faker faker = null!;
        private static ApplicationDbContext context = null!;
        private static RoleManager<IdentityRole> roleManager = null!;
        private static UserManager<ApplicationUser> userManager = null!;

        public static async Task InitAsync(ApplicationDbContext _context, IServiceProvider services)
        {
            context = _context ?? throw new ArgumentNullException(nameof(_context));

            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>() ?? throw new ArgumentNullException(nameof(services));
            userManager = services.GetRequiredService<UserManager<ApplicationUser>>() ?? throw new ArgumentNullException(nameof(services));

            faker = new Faker("sv");

            await GenerateTeacher();

            await GenerateCourses();


            await context.SaveChangesAsync();
        }



        private static async Task<ICollection<IdentityRole>> GenerateAndGetRoles()
        {
            ICollection<IdentityRole> identityRoles = await context.Roles.ToListAsync();

            if(identityRoles.Any()) return identityRoles;

            identityRoles.Add(new IdentityRole { Name = "Student" });
            identityRoles.Add(new IdentityRole { Name = "Teacher" });

            await context.AddRangeAsync(identityRoles);

            return identityRoles;
        }

        private static async Task GenerateTeacher()
        {
            if (await HasTeacherAsync()) return;

            IdentityRole teacherRole = (await GenerateAndGetRoles()).First(i => i.Name == "Teacher");

            ApplicationUser teacher = new ApplicationUser
            {
                Email = "dimitris@banana.se",
                FirstName = "Dimitris",
                LastName = "BananaMaster III"
            };

            string teacherPassword = "banan";

            if (!(await userManager.CreateAsync(teacher, teacherPassword)).Succeeded) throw new Exception($"Failed to seed Teacher {teacher.FirstName} with password \"{teacherPassword}\"");

            if (!(await userManager.AddToRoleAsync(teacher, teacherRole.Name)).Succeeded) throw new Exception($"Failed to set {teacher.FirstName} as a {teacherRole.Name}");

            await context.AddAsync(teacher);
        }

        private static async Task GenerateCourses()
        {
            ICollection<Course> courses = await context.Courses.ToListAsync();

            if(courses.Any()) return;

            int coursesToAddAmount = faker.Random.Int(4, 10);

            for(int i = 0; i < coursesToAddAmount; i++)
            {
                Course courseToAdd = new Course
                {
                    Name = faker.Company.CompanyName(),
                    Description = faker.Lorem.Paragraphs(),
                    StartTime = DateTime.Now.AddDays(faker.Random.Int(10 * (i + 1), 10 * (i + 7)))
                };

                courseToAdd.Modules = await GenerateAndGetModules(courseToAdd.StartTime);

                courses.Add(courseToAdd);
            }

            await context.AddRangeAsync(courses);
        }
        
        private static async Task<ICollection<Module>> GenerateAndGetModules(DateTime start)
        {
            ICollection<Module> modules = new List<Module>();

            Module firstModule = new Module 
            { 
                Name = faker.Company.CompanyName(),
                Description = faker.Lorem.Paragraphs(),
                StartTime = start
            };

            firstModule.Activities = await GenerateAndGetActivities(start);

            DateTime end = firstModule.Activities.Last().EndDate;

            firstModule.EndTime = end;

            modules.Add(firstModule);

            int modulesToAddAmount = faker.Random.Int(2, 6);

            for (int i = 0; i < modulesToAddAmount; i++)
            {
                Module moduleToAdd = new Module
                {
                    Name = faker.Company.CompanyName(),
                    Description = faker.Lorem.Paragraphs(),
                    StartTime = end
                };

                moduleToAdd.Activities = await GenerateAndGetActivities(end);

                end = moduleToAdd.Activities.Last().EndDate;

                moduleToAdd.EndTime = end;

                modules.Add(moduleToAdd);
            }

            await context.AddRangeAsync(modules);

            return modules;
        }

        private static async Task<ICollection<Activity>> GenerateAndGetActivities(DateTime start)
        {
            ICollection<Activity> activities = new List<Activity>();

            ICollection<ActivityType> activityTypes = await GenerateAndOrGetActivityTypes();
            int activityTypeSize = activityTypes.Count;

            Activity firstActivity = new Activity
            {
                Name = faker.Hacker.Phrase(),
                Description = faker.Lorem.Paragraph(),
                StartDate = start,
                EndDate = start.AddDays(faker.Random.Int(2, 5)),
                ActivityType = activityTypes.ElementAt(faker.Random.Int(0, activityTypeSize))
            };

            activities.Add(firstActivity);

            int activitiesAmountToAdd = faker.Random.Int(3, 10);

            for (int i = 0; i < activitiesAmountToAdd; i++)
            {
                Activity activityToAdd = new Activity
                {
                    Name = faker.Hacker.Phrase(),
                    Description = faker.Lorem.Paragraph(),
                    StartDate = activities.Last().EndDate,
                    EndDate = activities.Last().EndDate.AddDays(faker.Random.Int(2, 5)),
                    ActivityType = activityTypes.ElementAt(faker.Random.Int(0, activityTypeSize))
                };

                activities.Add(activityToAdd);
            }

            await context.AddRangeAsync(activities);

            return activities;
        }

        private static async Task<ICollection<ActivityType>> GenerateAndOrGetActivityTypes()
        {
            ICollection<ActivityType> activityTypes = await context.ActivitiesTypes.ToListAsync();

            if (activityTypes.Any()) return activityTypes;

            activityTypes.Add(new ActivityType { Name = "Assignment" });
            activityTypes.Add(new ActivityType { Name = "E-Learning" });
            activityTypes.Add(new ActivityType { Name = "Lecture" });
            activityTypes.Add(new ActivityType { Name = "Excersize" });

            await context.AddRangeAsync(activityTypes);

            return activityTypes;
        }

        private static async Task<bool> HasTeacherAsync()
        {
            if (!await context.Users.AnyAsync()) return false;

            if (!await context.Roles.AnyAsync()) return false;

            List<ApplicationUser> users = await context.Users.ToListAsync();

            List<IdentityRole> roles = await context.Roles.ToListAsync();

            IdentityRole? role = null;

            foreach (IdentityRole r in roles)
            {
                if (r.Name == "Teacher")
                {
                    role = r;
                    break;
                }
            }

            if (role == null) return false;

            foreach (ApplicationUser user in users)
            {
                if ((await userManager.GetRolesAsync(user)).Contains(role.Name))
                {
                    return true;
                }
            }

            return false;
        }

    }
}