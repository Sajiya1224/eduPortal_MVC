using eduPortal.Models;
using eduPortal.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Services.IServices
{
    public interface IVideoCourseService
    {
        VideoCourse saveCourse(VideoCourseVM obj, IFormFile? imgFile, IList<IFormFile> Videos);
    }
}
