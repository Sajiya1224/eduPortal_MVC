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
    public class InstructorDashboardVM
    {
        // Raw Data Collections
        public IEnumerable<VideoCourse> VideoCourses { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }

        // Computed Properties for Easy Access in View
        public int TotalCoursesCount => VideoCourses?.Count() ?? 0;
        public int TotalEnrolledStudentsCount => OrderDetails?
            .Select(o => o.OrderHeader?.ApplicationUserId)
            .Where(id => id != null)
            .Distinct()
            .Count() ?? 0;
        public double TotalEarnings => OrderDetails?.Sum(o => o.Price) ?? 0;

        // List of Distinct Enrolled Students (for displaying student list)
        public IEnumerable<ApplicationUser> EnrolledStudents => OrderDetails?
            .Where(o => o.OrderHeader?.ApplicationUsers != null)
            .Select(o => o.OrderHeader.ApplicationUsers)
            .Distinct()
            .ToList() ?? Enumerable.Empty<ApplicationUser>();

        // Courses with Enrollment Count (for displaying course stats)
        public IEnumerable<CourseEnrollmentVM> CoursesWithStats => VideoCourses?
            .Select(course => new CourseEnrollmentVM
            {
                Course = course,
                EnrollmentCount = OrderDetails?
                    .Count(od => od.VCourseId == course.Id) ?? 0,
                CourseRevenue = OrderDetails?
                    .Where(od => od.VCourseId == course.Id)
                    .Sum(od => od.Price) ?? 0
            })
            .ToList() ?? Enumerable.Empty<CourseEnrollmentVM>();
    }

    // Helper ViewModel for Course Statistics
    public class CourseEnrollmentVM
    {
        public VideoCourse Course { get; set; }
        public int EnrollmentCount { get; set; }
        public double CourseRevenue { get; set; }
    }
}
//{
//    [Area("Instructor")]
//    [Authorize(Roles = SD.Role_Instructor)]
//    public class InstructorDashboardVM
//    {

//        // Collection of all courses for instructor
//        public IEnumerable<VideoCourse> VideoCourses { get; set; }
//        //public IEnumerable<OrderHeader> orderHeader { get; set; }
//        public IEnumerable<OrderDetails> orderDetails { get; set; }


//    }
//}
