using NetBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Services.Interfaces
{
    public interface IPageService
    {
        Task<PageViewModel> GetPage(string Slug);
        Task CreatePage(PageViewModel vm);
        Task UpdatePage(PageViewModel vm);
    }
}
