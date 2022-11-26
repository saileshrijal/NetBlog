using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.ViewModels
{
    public class BlogPostViewModel
    {
        public PostViewModel? Post { get; set; }
        public List<PostViewModel>? RecentPosts { get; set; }
    }
}
