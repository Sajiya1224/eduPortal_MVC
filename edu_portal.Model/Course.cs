using System.ComponentModel.DataAnnotations;

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
        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }
        public string Duration { get; set; }
        [Required]
        public string Level { get; set; }
        public bool IsPublished { get; set; } = false;

        //public int CategoryId { get; set; }
        //public int InstructorId { get; set; }
        // foreignkey
    }
}
