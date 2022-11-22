using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Services.Implementations
{

    public class PostService:IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePost(PostViewModel vm)
        {
            var selectedCateogries = vm.Categories?.Where(x => x.Selected).Select(x => x.Value).Select(int.Parse).ToList();
            var categories = selectedCateogries?.Select(x => new Category());
            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                Slug = vm.Slug,
                ShortDescription = vm.ShortDescription,
                CreatedDate = vm.CreatedDate,
                IsBanner = vm.IsBanner,
                Status = vm.Status,
                UserId = vm.UserId,
                ThumbnailUrl = vm.ThumbnailUrl
            };
            
            if (vm.Title != null)
            {
                string slug = vm.Title.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            post.PostCategories = new List<PostCategory>();
            foreach (var categoryId in selectedCateogries)
            {
                post.PostCategories?.Add(new PostCategory
                {
                    Post = post,
                    CategoryId = categoryId,
                });
            }

            await _unitOfWork.Post.Create(post);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PostViewModel> GetPost(int id)
        {
            var model = await _unitOfWork.Post.GetBy(x => x.Id == id);
            var vm = new PostViewModel(model);
            return vm;
        }

        public async Task<List<PostViewModel>> GetPosts()
        {
            var listOfPosts = await _unitOfWork.Post.GetAll();
            var listOfPostsVM = ConvertModelToViewModelList(listOfPosts);
            return listOfPostsVM;
        }

        public async Task<List<PostViewModel>> GetPostsByUserId(string userId)
        {
            var listOfPosts = await _unitOfWork.Post.GetAllPostByUserId(userId);
            var listOfPostsVM = ConvertModelToViewModelList(listOfPosts);
            return listOfPostsVM;
        }

        private List<PostViewModel> ConvertModelToViewModelList(List<Post> listOfPosts)
        {
            return listOfPosts.Select(x => new PostViewModel(x)).ToList();
        }
    }
}
