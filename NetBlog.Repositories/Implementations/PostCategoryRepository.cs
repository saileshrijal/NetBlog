using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Implementations
{
    internal class PostCategoryRepository : GenericRepository<PostCategory>, IPostCategoryRepository
    {
        public PostCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
