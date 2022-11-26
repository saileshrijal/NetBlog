using NetBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Services.Interfaces
{
    public interface IPostService
    {
        Task<List<PostViewModel>> GetPostsByUserId(string userId);
        Task<List<PostViewModel>> GetPosts();
        Task<PostViewModel> GetPost(int id);
        Task CreatePost(PostViewModel vm);
        Task UpdatePost(PostViewModel vm);
        Task DeletePost(int id);
        Task<List<PostViewModel>> GetRecentPosts();
        Task<List<PostViewModel>> GetBannerPosts();
    }
}
