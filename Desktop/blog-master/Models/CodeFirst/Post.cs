using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models.CodeFirst
{
    public class Post
    {
        public Post()
        {
            this.Comments = new HashSet<Comment>();
        }

        //model.Id -- auto
        //model.Created -- auto
        //model.Updated -- auto
        //model.Title [req]
        //model.Body [req]
        //model.Category
        //model.MediaUrl
        //model.Slug -- auto
        //model.Comments (list)

        public int Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Body { get; set; }
        public string MediaUrl { get; set; }
        public string Slug { get; set; }
        public string Category { get; set; }    

        public virtual ICollection<Comment> Comments { get; set; }



    }
}