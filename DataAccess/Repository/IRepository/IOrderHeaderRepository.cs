using eduPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);

        void UpdateStatus(int Id, string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentIdByCart(int Id, string sessionId, string PaymentIntendId);
    }
}
