﻿using Lexicon_LMS_G1.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }


        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<ApplicationUser> AttendingStudents { get; set; } = new List<ApplicationUser>();
    }
}
