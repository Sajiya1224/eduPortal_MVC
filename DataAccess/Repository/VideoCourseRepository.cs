using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Repository
{
    public class VideoCourseRepository : Repository<VideoCourse>, IVideoCourseRepository
    {
        private readonly ApplicationDbContext _db;

        public VideoCourseRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;

        }
        public void Update(VideoCourse obj)
        {
            _db.courses.Update(obj);
        }
    }
}
