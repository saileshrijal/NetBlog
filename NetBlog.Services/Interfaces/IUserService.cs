﻿using NetBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Services.Interfaces
{
    public interface IUserService
    {
        Task Login(LoginViewModel vm);
        Task Logout();
    }
}
