
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlog.Data;
using NetBlog.Models;
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

    builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedEmail = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddControllersWithViews();

    builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

    builder.Services.AddTransient<IUserService, UserService>();
    builder.Services.AddTransient<IDbInitializer, DbInitializer>();
}



var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    DataSeeding();
    app.UseRouting();
    app.UseNotyf();
    app.UseAuthentication();
    app.UseAuthorization();

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

