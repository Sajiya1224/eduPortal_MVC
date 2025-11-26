using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace eduPortal.DataAccess.Repository
{
   
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();

            _db.videoCourses.Include(c => c.Sections).ThenInclude(s => s.Lectures).Include(c => c.Category);

            //_db.courses.Include(u => u.Category).Include(u => u.CategoryId);
            //_db.videoCourses.Include(u => u.Sections);
            //_db.section.Include(u => u.Lectures);
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }
            return query.FirstOrDefault(filter);
        }

        public IEnumerable<T> GetAll(string? includeproperties = null)
        {

            IQueryable<T> query = dbSet;
            if(!string.IsNullOrEmpty(includeproperties))
            {
                foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
