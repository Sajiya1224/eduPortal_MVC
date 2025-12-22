using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUsers { get; set; }
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntendId { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
