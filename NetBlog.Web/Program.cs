using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Implementations;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Implementations;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities.Implementations;
using NetBlog.Utilities.Interfaces;


var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllersWithViews();
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


    builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
    });

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IPostService, PostService>();
    builder.Services.AddScoped<IPageService, PageService>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IDbInitializer, DbInitializer>();
}

var app = builder.Build();
{

    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == 404)
        {
            if (context.User.Identity!.IsAuthenticated)
            {
                if (context.Request.Path.StartsWithSegments("/Dashboard"))
                {
                    context.Request.Path = "/Dashboard/Error/NotFound";
                }
                else
                {
                    context.Request.Path = "/NotFound";
                }
            }
            else
            {
                context.Request.Path = "/NotFound";
            }
            await next();
        }
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    DataSeeding();
    app.UseRouting();
    app.UseNotyf();
    app.UseCookiePolicy();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );
    });


    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

    void DataSeeding()
    {
        using (var scope = app.Services.CreateScope())
        {
            var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            DbInitializer.Initialize();
        }
    }
}

