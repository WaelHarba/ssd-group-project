using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSD_Lab1_TeamsWithMembership.Data;
using SSD_Lab1_TeamsWithMembership.Models;

namespace SSD_Lab1_TeamsWithMembership
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            var configuration = app.Services.GetService<IConfiguration>();
            var hosting = app.Services.GetService<IWebHostEnvironment>();

            if (hosting.IsDevelopment())
            {
                var secrets = configuration.GetSection("Secrets").Get<AppSecrets>();
                DbInitializer.AppSecrets = secrets;
            }

            using (var scope = app.Services.CreateScope())
            {
                DbInitializer.SeedUsersAndRoles(scope.ServiceProvider).Wait();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; style-src 'self'; font-src 'self'; form-action 'self'; frame-ancestors 'self'");
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}