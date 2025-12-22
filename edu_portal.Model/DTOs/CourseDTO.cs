using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.DTOs
{
    public class VideoCourseDTO
    {
        public int? Id {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Level { get; set; }
        public double Price { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? AddInfo { get; set; }
        public string? courseContent { get; set; }
        public bool IsPublished { get; set; } = false;
        public List<SectionDTO> Sections { get; set; } = new();
    }

    public class SectionDTO
    {
        public int? Id { get; set; }
        public int? VideoCourseId { get; set; }
        public string SectionTitle { get; set; }
        public List<LectureDTO> Lectures { get; set; } = new();
    }

    public class LectureDTO
    {
        public int? Id { get; set; }
        public int? SectionId { get; set; }
        public string LectureTitle { get; set; }
        public string? VideoUrl { get; set; }
    }

    public class ApplicationUserDTO
    {
        public int Id { get; set; }
        public string Role {  get; set; }
    }
}
