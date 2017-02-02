using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.ViewModel
{
    public class ItemDetailsViewModel
    {
        public ShoppingApp.Models.Item Item { get; set; }
        public bool IsAdded { get; set; }
    }
}