using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set;}
        public string ChangePassword { get; set; }
        public bool WrongPassword { get; set; }
    }
}