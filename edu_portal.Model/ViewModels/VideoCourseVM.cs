using eduPortal.Models.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    public class VideoCourseVM
    {
        public VideoCourseDTO VideoCourseVm { get; set; }

        // UI Only
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
