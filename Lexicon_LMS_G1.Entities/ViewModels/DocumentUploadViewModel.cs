using Lexicon_LMS_G1.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon_LMS_G1.Entities.ViewModels
{
    public class DocumentUploadViewModel<T, Q> where Q : class
    {
        public IFormFile Document { get; set; }
        public IEnumerable<Q> Identifier { get; set; }
        public string Description { get; set; }

    }
}