using eduPortal.DataAccess.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Services
{
    public class MediaSaveServices : IMediaSaveServices
    {
        private readonly IWebHostEnvironment _env;

        public MediaSaveServices(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveThumbnail(IFormFile? file, string currentPath)
        {
            if (file == null) return currentPath;
            var folder = Path.Combine(_env.WebRootPath, @"Courses\Thumbnails");
            Directory.CreateDirectory(folder);

            DeleteIfExists(currentPath);
            var newName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var fs = new FileStream(Path.Combine(folder, newName), FileMode.Create);
            file.CopyTo(fs);
            return $@"\Courses\Thumbnails\{newName}";
        }

        public string SaveLectureVideo(IFormFile? file, string currentPath)
        {
            if (file == null) return currentPath;
            var folder = Path.Combine(_env.WebRootPath, @"Courses\Videos");
            Directory.CreateDirectory(folder);

            DeleteIfExists(currentPath);
            var newName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var fs = new FileStream(Path.Combine(folder, newName), FileMode.Create);
            file.CopyTo(fs);
            return $@"\Courses\Videos\{newName}";
        }

        private void DeleteIfExists(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;
            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('\\'));
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
