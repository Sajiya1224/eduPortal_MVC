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
    public class LectureRepository : Repository<Lecture>, ILectureRepository
    {
        private readonly ApplicationDbContext _db;
        public LectureRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;

        }


        public void Update(Lecture obj)
        {
            _db.lecture.Update(obj);
        }
    }
}
