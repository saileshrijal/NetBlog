using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Utilities
{
    public static class FileHelper
    {
        public static string UploadImage(IFormFile file, IWebHostEnvironment webHost, string fileName)
        {
            string uniqueFilename = "";
            var rootPath = Path.Combine(webHost.WebRootPath, fileName);
            uniqueFilename = Guid.NewGuid().ToString() + file.FileName;
            var filePath = Path.Combine(rootPath, uniqueFilename);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFilename;
        }
    }
}
