﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        //navigation properties
        public List<PostCategory>? PostCategories { get; set; }
    }
}