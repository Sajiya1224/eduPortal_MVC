using eduPortal.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICourseRepository Course { get; }
        IVideoCourseRepository VideoCourse { get; }
        ISectionReposirory Section { get; }
        ILectureRepository Lecture { get; }
        ICartRepository Cart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
