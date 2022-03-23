using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lexicon_LMS_G1.Entities.Entities
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime TimeOfCreation { get; set; }

        [Required]
        public string Path { get; set; }

        //Navprop
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public int? ModuleId { get; set; }
        public Module? Module { get; set; }

        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }
    }
}
