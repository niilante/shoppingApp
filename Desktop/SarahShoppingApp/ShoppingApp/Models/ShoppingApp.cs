﻿using System;
using System.Web.Mvc;

namespace ShoppingApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string MediaUrl { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
