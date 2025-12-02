using eduPortal.DataAccess.Services.IServices;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Services
{
    public class DashboardInstructorServices: IDashboardInstructorServices
    {

        public IVideoCourseService _VideoCourseService { get; private set; }

        public DashboardInstructorServices(IVideoCourseService VideoCourseService) 
        {
            _VideoCourseService = VideoCourseService;
        }

    
    }
}
