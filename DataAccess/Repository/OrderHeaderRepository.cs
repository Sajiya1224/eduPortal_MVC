using eduPortal.DataAccess.dataContext;
using eduPortal.DataAccess.Repository.IRepository;
using eduPortal.Models;
using eduPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader entity)
        {
            _db.OrderHeader.Update(entity);
        }

        public void UpdateStatus(int Id, string orderStatus, string? paymentStatus)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(p => p.Id == Id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentIdByCart(int Id, string sessionId, string PaymentIntendId)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(p => p.Id == Id);
            if (orderFromDb != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    orderFromDb.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(PaymentIntendId))
                {
                    orderFromDb.PaymentIntendId = PaymentIntendId;
                    orderFromDb.PaymentDate = DateTime.UtcNow;
                }

                _db.SaveChanges();
            }
        } 
    }
}

