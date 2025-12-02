using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.DataAccess.Services.IServices;
using eduPortal.Models;
using eduPortal.Models.DTOs;
using eduPortal.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Section = eduPortal.Models.Section;


namespace eduPortal.DataAccess.Services
{
    public class VideoCourseService: IVideoCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
      
        public VideoCourseService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) 
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }

        public VideoCourse saveCourse(VideoCourseVM obj, IFormFile? imgFile, IList<IFormFile> vidFile)
        {
            obj.VideoCourseVm.ThumbnailUrl = SaveCourseThumbnail(imgFile, obj.VideoCourseVm.ThumbnailUrl);
            var course = UpsertCourse(obj);
            HandelingSection(obj.VideoCourseVm.Sections, course.Id, vidFile);
            return course;
        }

        private string SaveCourseThumbnail(IFormFile? imgFile, string objFile)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (imgFile != null)
            {
                
                string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                string imgPath = Path.Combine(wwwRootPath, @"Courses\Thumbnails");

                if (!string.IsNullOrEmpty(objFile))
                {
                    var oldImgPath = Path.Combine(wwwRootPath, (objFile.TrimStart('\\')));

                    if (File.Exists(oldImgPath))
                    {
                        File.Delete(oldImgPath);
                    }

                }

                using (var imgfileStream = new FileStream(Path.Combine(imgPath, imgFileName), FileMode.Create))
                {
                    imgFile.CopyTo(imgfileStream);
                }

                return $@"\Courses\Thumbnails\{imgFileName}"; 
            }
            else return objFile;
        }


        private VideoCourse UpsertCourse(VideoCourseVM obj)
        {
            //Map DTO to entity 
            var course = new VideoCourse
            {
                Id = obj.VideoCourseVm.Id,
                Title = obj.VideoCourseVm.Title,
                Description = obj.VideoCourseVm.Description,
                Price = obj.VideoCourseVm.Price,
                Level = obj.VideoCourseVm.Level,
                Duration = obj.VideoCourseVm.Duration,
                CategoryId = obj.VideoCourseVm.CategoryId,
                ApplicationUserId = obj.VideoCourseVm.ApplicationUserId,
                ThumbnailUrl = obj.VideoCourseVm.ThumbnailUrl
            };

            if (course.Id == 0)
            {
                _unitOfWork.Course.Add(course);
            }
            else
            {
                _unitOfWork.Course.Update(course);
            }
            // Save course

            _unitOfWork.Save();
            return course;
        }
        private void HandelingSection(List<SectionDTO> Sections, int courseId, IList<IFormFile> vidFile)
        {
            foreach (var sec in Sections)
            {
                Section section;

                if (sec.Id == 0)
                {
                    // NEW SECTION
                    section = new Section
                    {
                        SectionTitle = sec.SectionTitle,
                        VideoCourseId = courseId
                    };

                    _unitOfWork.Section.Add(section);
                    _unitOfWork.Save(); // save first to generate DB Id

                    // Pass the correct DB-generated Id to lectures 
                    HandelingLecture(sec.Lectures, section.Id, vidFile);
                }
                else
                {
                    // EXISTING SECTION
                    section = _unitOfWork.Section.Get(s => s.Id == sec.Id);
                    if (section == null)
                        continue;

                    section.SectionTitle = sec.SectionTitle;
                    _unitOfWork.Section.Update(section);
                    _unitOfWork.Save();

                    HandelingLecture(sec.Lectures, section.Id, vidFile);
                }
            }
        }


        private void HandelingLecture(List<LectureDTO> Lectures, int secId, IList<IFormFile> vidFile)
        {
            int index = 0;
           

            foreach (var lec in Lectures)
            {
                string vidUrl = lec.VideoUrl;
                //var vidUrl = saveVideo(vidFile, lec.VideoUrl);
                if (index < vidFile.Count && vidFile[index] != null)
                     vidUrl = saveVideo(vidFile[index], lec.VideoUrl);

                Lecture lecture;
                if (lec.Id == 0)
                {
                    if (string.IsNullOrEmpty(vidUrl))
                    {
                        continue; 
                    }
                    // NEW LECTURE
                    lecture = new Lecture
                    {
                        Id = lec.Id,
                        LectureTitle = lec.LectureTitle,
                        VideoUrl = vidUrl,
                        SectionId = secId
                    };

                    _unitOfWork.Lecture.Add(lecture);

                }
                else
                {
                    // EXISTING LECTURE
                    lecture = _unitOfWork.Lecture.Get(l => l.Id == lec.Id);

                    if (lecture == null)
                        continue;

                    lecture.LectureTitle = lec.LectureTitle;
                    lecture.VideoUrl = vidUrl;

                    _unitOfWork.Lecture.Update(lecture);
                }
                _unitOfWork.Save();
                index++;
            }
           
        }
        

        private string saveVideo(IFormFile? vidFile, string objVid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (vidFile != null)
            {

                string vidFileName = Guid.NewGuid().ToString() + Path.GetExtension(vidFile.FileName);
                string vidPath = Path.Combine(wwwRootPath, @"Courses\Videos");

                if (!string.IsNullOrEmpty(objVid))
                {
                    var oldVidPath = Path.Combine(wwwRootPath, (objVid.TrimStart('\\')));

                    if (File.Exists(oldVidPath))
                    {
                        File.Delete(oldVidPath);
                    }

                }

                using (var vidfileStream = new FileStream(Path.Combine(vidPath, vidFileName), FileMode.Create))
                {
                    vidFile.CopyTo(vidfileStream);
                }

                return $@"\Courses\Videos\{vidFileName}";
            }

            else return objVid;
        }


    }


}


  
