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
                    AddInfo = obj.VideoCourseVm.AddInfo,
                    IsPublished = obj.VideoCourseVm.IsPublished
                    
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
                course.AddInfo = obj.VideoCourseVm.AddInfo;
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

            int globalFileIndex = 0; // Track file index across ALL sections

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
                    _unitOfWork.Save();

                    section = _unitOfWork.Section.Get(s => s.Id == section.Id, "Lectures");
                    if (course.Sections == null)
                        course.Sections = new List<Section>();
                    course.Sections.Add(section);
                }
                else
                {
                    section = course.Sections.FirstOrDefault(s => s.Id == dto.Id);

                    if (section == null)
                    {
                        section = _unitOfWork.Section.Get(s => s.Id == dto.Id, "Lectures");
                        if (section == null)
                        {
                            section = new Section { VideoCourseId = course.Id, SectionTitle = dto.SectionTitle };
                            _unitOfWork.Section.Add(section);
                            _unitOfWork.Save();
                            section = _unitOfWork.Section.Get(s => s.Id == section.Id, "Lectures");
                        }
                        if (course.Sections == null)
                            course.Sections = new List<Section>();
                        course.Sections.Add(section);
                    }

                    section.VideoCourseId = course.Id;
                    section.SectionTitle = dto.SectionTitle;
                    _unitOfWork.Section.Update(section);
                    _unitOfWork.Save();
                }

                if (section.Lectures == null)
                    section.Lectures = new List<Lecture>();

                // Pass and receive updated file index
                globalFileIndex = HandelingLecture(section, dto.Lectures, vidFiles, globalFileIndex);
            }
        }

        //  Updated signature to accept and return file index
        private int HandelingLecture(Section section, List<LectureDTO> lectureDtos, IList<IFormFile> vidFiles, int startFileIndex)
        {
            // Ensure Lectures collection is initialized
            if (section.Lectures == null)
                section.Lectures = new List<Lecture>();

            // Filter out empty lectures
            lectureDtos = lectureDtos
                .Where(l => !string.IsNullOrWhiteSpace(l.LectureTitle))
                .ToList();

            // Delete removed lectures (only check existing lectures with Id > 0)
            var incomingIds = lectureDtos.Select(l => l.Id).Where(id => id != null && id != 0).ToList();
            var removedLectures = section.Lectures.Where(l => l.Id != 0 && !incomingIds.Contains(l.Id)).ToList();

            foreach (var lec in removedLectures)
            {
                _unitOfWork.Lecture.Remove(lec);
            }

            int fileIndex = startFileIndex; // Use the starting index from previous sections

            foreach (var dto in lectureDtos)
            {
                Lecture lecture;

                // Select the video file only if uploaded
                IFormFile? file = fileIndex < vidFiles.Count ? vidFiles[fileIndex] : null;

                string newUrl = file != null ? _mediaService.SaveLectureVideo(file, dto.VideoUrl) : dto.VideoUrl;

                if (dto.Id == 0 || dto.Id == null)
                {
                    // NEW LECTURE
                    lecture = new Lecture
                    {
                        LectureTitle = dto.LectureTitle,
                        VideoUrl = newUrl,
                        SectionId = section.Id
                    };
                    _unitOfWork.Lecture.Add(lecture);
                    _unitOfWork.Save();

                    // Add to section.Lectures collection to keep in sync
                    section.Lectures.Add(lecture);
                }
                else
                {
                    // UPDATE EXISTING LECTURE
                    lecture = section.Lectures.FirstOrDefault(l => l.Id == dto.Id);

                    if (lecture == null)
                    {
                        // Lecture not in section.Lectures, try to load from database
                        lecture = _unitOfWork.Lecture.Get(l => l.Id == dto.Id);

                        if (lecture == null)
                        {
                            // Lecture doesn't exist, create new one
                            lecture = new Lecture
                            {
                                LectureTitle = dto.LectureTitle,
                                VideoUrl = newUrl,
                                SectionId = section.Id
                            };
                            _unitOfWork.Lecture.Add(lecture);
                            _unitOfWork.Save();
                        }
                        else
                        {
                            // Found in DB, add to section.Lectures
                            section.Lectures.Add(lecture);
                        }
                    }

                    // Update lecture properties
                    lecture.LectureTitle = dto.LectureTitle;
                    lecture.VideoUrl = newUrl;
                    lecture.SectionId = section.Id;
                    _unitOfWork.Lecture.Update(lecture);
                }

                _unitOfWork.Save();
                fileIndex++;
            }

            return fileIndex; // Return updated index for next section
        }



    }


}


  
