using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Country { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
