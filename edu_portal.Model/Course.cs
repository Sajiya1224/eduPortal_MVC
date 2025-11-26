using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eduPortal.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MinLength(20)]
        public string Description { get; set; }
        public string Duration { get; set; }
        [Required]
        [RegularExpression("Beginner|Intermediate|Advanced")]
        public string Level { get; set; }
        public bool IsPublished { get; set; } = false;

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        

        //public int InstructorId { get; set; }
        // foreignkey
    }
}
