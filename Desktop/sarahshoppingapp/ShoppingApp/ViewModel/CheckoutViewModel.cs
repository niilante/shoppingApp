using ShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.ViewModel
{
    public class CheckoutViewModel
    { 
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}