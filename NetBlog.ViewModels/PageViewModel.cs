﻿using NetBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.ViewModels
{
    public class PageViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Slug { get; set; } = "About";

        public PageViewModel()
        {

        }
        public PageViewModel(Page model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            ShortDescription = model.ShortDescription;
            CreatedDate = model.CreatedDate;
            Slug = model.Slug;
        }
        public Page ConvertViewModel(PageViewModel model)
        {
            return new Page
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                CreatedDate = model.CreatedDate,
                Slug = model.Slug,
            };
        }
    }
}