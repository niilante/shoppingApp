using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models
{
    public class OrderSummary
    {
        public OrderSummary(float v)
        {
            SubTotal = v;
        }
        public float SubTotal;
    }
}