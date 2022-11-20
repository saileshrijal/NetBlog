﻿using NetBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public CategoryViewModel()
        {
        }

        public CategoryViewModel(Category model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            CreatedDate = model.CreatedDate;
            UserId = model.UserId;
            User = model.User;
        }

        public Category ConvertViewModel(CategoryViewModel model)
        {
            return new Category
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                UserId = model.UserId,
                User = model.User
            };
        }
    }

}
