using eduPortal.DataAccess.dataContext;
using eduPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using System.Linq.Expressions;

namespace eduPortal.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser entity)
        {
            _db.ApplicationUsers.Update(entity);
        }

    }
}
