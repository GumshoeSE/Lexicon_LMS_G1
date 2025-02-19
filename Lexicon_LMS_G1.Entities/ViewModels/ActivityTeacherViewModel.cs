﻿using Lexicon_LMS_G1.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class ActivityTeacherViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public int ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
