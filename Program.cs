using CLDVPart3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using CLDVPart3.Services;


namespace CLDVPart3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Access the configuration object
            var configuration = builder.Configuration;

            // Register BlobService with configuration
            builder.Services.AddSingleton(new BlobService(configuration.GetConnectionString("AzureStorage")));

            //Adding DB Context builder services with options
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                       options.UseSqlServer(builder.Configuration.GetConnectionString("ABCRetailersDEV")));

            //Added service for Authorization for Role based Access
            builder.Services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
                           .AddRoles<IdentityRole>()
                           .AddEntityFrameworkStores<ApplicationDBContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
