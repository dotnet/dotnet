#if (IndividualLocalAuth)
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Company.WebApplication1.Data;
using Company.WebApplication1.Models;

#endif

namespace Company.WebApplication1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #if (IndividualLocalAuth)
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        #if (UseLocalDB)
            options.UseSqlServer(connectionString));
        #else
            options.UseSqlite(connectionString));
        #endif
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        builder.Services.AddAuthentication()
            .AddIdentityServerJwt();
        #endif

        builder.Services.AddControllersWithViews();
        #if (IndividualLocalAuth)
        builder.Services.AddRazorPages();
        #endif

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        #if (IndividualLocalAuth)
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        #else
        if (!app.Environment.IsDevelopment())
        #endif
        {
        #if (RequiresHttps)
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        #else
        }

        #endif
        app.UseStaticFiles();
        app.UseRouting();

        #if (IndividualLocalAuth)
        app.UseAuthentication();
        app.UseIdentityServer();
        #endif
        #if (!NoAuth)
        app.UseAuthorization();
        #endif

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");
        #if (IndividualLocalAuth)
        app.MapRazorPages();
        #endif

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
