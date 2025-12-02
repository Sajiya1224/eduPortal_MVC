using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class Lecture
    {
        
        public int Id { get; set; }
        [Required]
        
        public string LectureTitle { get; set; }
        
        public string? VideoUrl { get; set; }

        [Required]
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public Section Section { get; set; }


    }
}
