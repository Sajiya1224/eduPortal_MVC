using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    public class OrderVM
    {
        public IEnumerable<OrderHeader> OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetail { get; set; }
    }
}
