﻿using Microsoft.EntityFrameworkCore;
using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Repositories.Implementations
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Post?> GetBy(Expression<Func<Post, bool>> predicate)
        {
            return await _context.Posts.Include(x => x.User).Include(x=>x.PostCategories).ThenInclude(x=>x.Category).FirstOrDefaultAsync(predicate);
        }

        public override async Task<List<Post>> GetAll()
        {
            return await _context.Posts.Include(x => x.User).Include(x=>x.PostCategories).ThenInclude(x=>x.Category).ToListAsync();
        }

        public async Task<List<Post>> GetAllPostByUserId(string userId)
        {
            return await _context.Posts.Include(x => x.User).Include(x => x.PostCategories).ThenInclude(x => x.Category).Where(x=>x.UserId==userId).ToListAsync();
        }
    }
}