using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    public class QuizCourseVM
    {
        public QuizCourse QuizCourse { get; set; }
        public Question Quesstion{ get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }   

    }
}
