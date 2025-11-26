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
    public class SectionRepository : Repository<Section>, ISectionReposirory
    {
        private readonly ApplicationDbContext _db;
        public SectionRepository(ApplicationDbContext db) : base(db)
        {

            _db = db;

        }

        public void Update(Section obj)
        {
            _db.section.Update(obj);
        }
    }
}
