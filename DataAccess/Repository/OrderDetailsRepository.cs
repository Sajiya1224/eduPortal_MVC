using eduPortal.DataAccess.dataContext;
using eduPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;

namespace eduPortal.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetails entity)
        {
            _db.OrderDetails.Update(entity);
        }

    }
}
