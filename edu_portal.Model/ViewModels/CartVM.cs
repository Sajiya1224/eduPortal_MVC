using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> cartList { get; set; }
        public OrderHeader orderHeader { get; set; }

    }
}
