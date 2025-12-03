using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Services.IServices
{
    public  interface IMediaSaveServices
    {
        string SaveThumbnail(IFormFile? file, string currentPath);
        string SaveLectureVideo(IFormFile? file, string currentPath);
    }
}
