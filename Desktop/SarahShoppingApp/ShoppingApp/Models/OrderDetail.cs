using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}