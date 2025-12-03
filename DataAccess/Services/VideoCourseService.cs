using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.DataAccess.Services.IServices;
using eduPortal.Models;
using eduPortal.Models.DTOs;
using eduPortal.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using static System.Collections.Specialized.BitVector32;
using Section = eduPortal.Models.Section;


namespace eduPortal.DataAccess.Services
{
    public class VideoCourseService: IVideoCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediaSaveServices _mediaService;
        private readonly IWebHostEnvironment _webHostEnvironment;
      
        public VideoCourseService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMediaSaveServices mediaService) 
        {
            _unitOfWork = unitOfWork;
            _mediaService = mediaService;
            _webHostEnvironment = webHostEnvironment;

        }

        public VideoCourse saveCourse(VideoCourseVM obj, IFormFile? imgFile, IList<IFormFile> vidFiles)
        {
            obj.VideoCourseVm.ThumbnailUrl = _mediaService.SaveThumbnail(imgFile, obj.VideoCourseVm.ThumbnailUrl);
            var course = UpsertCourse(obj);

            HandelingSection(obj.VideoCourseVm.Sections, course,  vidFiles);



            return course;
        }

        private VideoCourse UpsertCourse(VideoCourseVM obj)
        {
            VideoCourse course;

            if (obj.VideoCourseVm.Id == 0 || obj.VideoCourseVm.Id == null)
            {
                // CREATE as id = 0 mean doesn't exist
                course = new VideoCourse
                {
                    Title = obj.VideoCourseVm.Title,
                    Description = obj.VideoCourseVm.Description,
                    Price = obj.VideoCourseVm.Price,
                    Level = obj.VideoCourseVm.Level,
                    Duration = obj.VideoCourseVm.Duration,
                    CategoryId = obj.VideoCourseVm.CategoryId,
                    ApplicationUserId = obj.VideoCourseVm.ApplicationUserId,
                    ThumbnailUrl = obj.VideoCourseVm.ThumbnailUrl,
                    
                };
               _unitOfWork.Course.Add(course);
            }
            else
            {
                // UPDATE – load from DB to keep children
                course = (VideoCourse)_unitOfWork.Course.Get(
                    c => c.Id == obj.VideoCourseVm.Id,
                    includeproperties: "Category, ApplicationUser, Sections, Sections.Lectures"
                );
                // Update fields
                //course.Id = (int)obj.VideoCourseVm.Id;
                course.Title = obj.VideoCourseVm.Title;
                course.Description = obj.VideoCourseVm.Description;
                course.Price = obj.VideoCourseVm.Price;
                course.Level = obj.VideoCourseVm.Level;
                course.Duration = obj.VideoCourseVm.Duration;
                course.CategoryId = obj.VideoCourseVm.CategoryId;
                course.ApplicationUserId = obj.VideoCourseVm.ApplicationUserId;
                course.ThumbnailUrl = obj.VideoCourseVm.ThumbnailUrl;
                _unitOfWork.Course.Update(course);
            }
            course.Sections ??= new List<Section>();
            _unitOfWork.Save();
            return course;
        }
      
        private void HandelingSection(List<SectionDTO> sectionDtos, VideoCourse course, IList<IFormFile> vidFiles)
        {
            // 1. Delete removed sections
            var existingSections = sectionDtos.Select(s => s.Id).ToList();
            var removedSections = course.Sections.Where(s => !existingSections.Contains(s.Id)).ToList();

            foreach (var sec in removedSections)
                _unitOfWork.Section.Remove(sec);


            foreach (var dto in sectionDtos)
            {
                Section section;
              
                if (dto.Id == 0 || dto.Id == null)
                {
                    // NEW SECTION
                    section = new Section
                    {
                       
                        SectionTitle = dto.SectionTitle,
                        VideoCourseId = course.Id
                    };
                    _unitOfWork.Section.Add(section);
                }
                else
                {
                    section = course.Sections.FirstOrDefault(s => s.Id == dto.Id);
                    if (section == null)
                    {
                        section = new Section { VideoCourseId = course.Id, SectionTitle = dto.SectionTitle };
                        _unitOfWork.Section.Add(section);
                    }
                    section.VideoCourseId = course.Id;
                    section.SectionTitle = dto.SectionTitle;
                    _unitOfWork.Section.Update(section);
                }
                _unitOfWork.Save();
                HandelingLecture(section, dto.Lectures, vidFiles);
            }
        }


        private void HandelingLecture(Section section, List<LectureDTO> lectureDtos, IList<IFormFile> vidFiles)
        {
            // delete removed lectures
            var incomingIds = lectureDtos.Select(l => l.Id).ToList();
            var removedLectures = section.Lectures.Where(l => !incomingIds.Contains(l.Id)).ToList();

            foreach (var lec in removedLectures)
                _unitOfWork.Lecture.Remove(lec);

            int fileIndex = 0;

            foreach (var dto in lectureDtos)
            {
                Lecture lecture;

                // Select the video file only if uploaded
                IFormFile? file = fileIndex < vidFiles.Count ? vidFiles[fileIndex] : null;

                string newUrl = file != null? _mediaService.SaveLectureVideo(file, dto.VideoUrl) : dto.VideoUrl; // keep old value

                if (dto.Id == 0 || dto.Id == null)
                {
                    // NEW
                    lecture = new Lecture
                    {
                        LectureTitle = dto.LectureTitle,
                        VideoUrl = newUrl,
                        SectionId = section.Id
                    };
                    _unitOfWork.Lecture.Add(lecture);
                }
                else
                {
                    // UPDATE
                    lecture = section.Lectures.First(l => l.Id == dto.Id);
                    lecture.LectureTitle = dto.LectureTitle;
                    lecture.VideoUrl = newUrl;
                    _unitOfWork.Lecture.Update(lecture);
                }
                _unitOfWork.Save();
                fileIndex++;
            }

        }
        
    

    }


}


  
