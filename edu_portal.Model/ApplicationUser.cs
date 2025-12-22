using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public  class ApplicationUser: IdentityUser
    {
        
        [Required]
        public string Name { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; }
        public string? CV { get; set; }
        public string? DemoVid { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool? IsApproved { get; set; } = null; // null = Pending, true = Approved, false = Rejected
    }
}
