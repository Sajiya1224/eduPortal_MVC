using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class VideoCourse: Course
    {
       
        public string? ThumbnailUrl { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Language { get; set; }

        [Required]
        [ Range(0, 1000)]
        public double Price { get; set; }

        public ICollection<Section> Sections { get; set; }
    }
}
