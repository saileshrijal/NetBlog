using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.ViewModels
{
    public class HomeViewModel
    {
        public List<PostViewModel>? BannerPosts { get; set; }
        public List<PostViewModel>? RecentPosts { get; set; }
    }
}
