using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class Question
    {
            [Key]
            public int Id { get; set; }

            [Required]
            public string QuestionText { get; set; }   // Question text

            public string OptionA { get; set; }
            public string OptionB { get; set; }
            public string OptionC { get; set; }
            public string OptionD { get; set; }

            [Required]
            public string CorrectOption { get; set; } // "A", "B", "C", or "D"

            // Foreign key to QuizCourse
            public int QuizCourseId { get; set; }
            [ForeignKey("QuizCourseId")]
            public QuizCourse QuizCourse { get; set; }

    }
}
