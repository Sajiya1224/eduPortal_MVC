using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class QuizCourse: Course
    {
        public int TotalQuestions { get; set; }

        public int TotalMarks { get; set; }

        public int PassingScore { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
