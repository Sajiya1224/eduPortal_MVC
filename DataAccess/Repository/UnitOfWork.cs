using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Repository
{
   

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public ICourseRepository Course { get; private set; }
        public ISectionReposirory Section { get; private set; }
        public ILectureRepository Lecture { get; private set; }
        public UnitOfWork(ApplicationDbContext db) 
        {

            _db = db;
            Category = new CategoryRepository(_db);
            Course = new CourseRepository(_db);
            Section = new SectionRepository(_db);
            Lecture = new LectureRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
