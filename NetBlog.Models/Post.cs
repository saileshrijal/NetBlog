using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? CreatedDate { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        //navigation properties
        public List<PostCategory>? PostCategories { get; set; }
    }
}
