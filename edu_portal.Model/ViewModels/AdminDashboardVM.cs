using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalUsersCount { get; set; }
        public int TotalCoursesCount { get; set; }
        public int TotalEnrollmentsCount { get; set; }
        public double TotalRevenue { get; set; }
        public double ActiveInstructor { get; set; }
        public double PublishedCourses { get; set; }

     
    }
}
