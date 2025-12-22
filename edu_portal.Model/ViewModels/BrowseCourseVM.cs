using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    public class BrowseCourseVM
    {
        [ValidateNever]
        public IEnumerable<VideoCourse> videoCourse { get; set; }
        [ValidateNever]
        public IEnumerable<Category> CategoryList { get; set; }

        

    }
}
