using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string SectionTitle{ get; set; }
        public int VideoCourseId { get; set; }
        [ForeignKey("VideoCourseId")]
        public VideoCourse VideoCourse { get; set; }

        public List<Lecture> Lectures { get; set; } = new List<Lecture>();
    }
}
