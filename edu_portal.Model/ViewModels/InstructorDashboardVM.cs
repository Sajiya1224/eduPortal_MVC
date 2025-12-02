using eduPortal.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    [Area("Instructor")]
    [Authorize(Roles = SD.Role_Instructor)]
    public class InstructorDashboardVM
    {
        // Collection of all users
        public IEnumerable<ApplicationUser> Users { get; set; }

        // Collection of all courses for instructor
        public IEnumerable<VideoCourse> VideoCourses { get; set; }

        // Role dropdown for UI
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

        // Optional: store number of learners directly
        public int TotalLearners => Users?.Count(u => u.Role == SD.Role_Learner) ?? 0;
    }
}
