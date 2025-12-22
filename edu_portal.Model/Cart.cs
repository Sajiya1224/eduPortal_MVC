using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int VCourseId { get; set; }
        [ForeignKey("VCourseId")]
        [ValidateNever]
        public VideoCourse VCourse { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUsers { get; set; }

        public double Price { get; set; }
    }
}
