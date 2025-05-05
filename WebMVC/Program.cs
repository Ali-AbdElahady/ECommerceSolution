using Infrastructure;
using Web.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebMVC
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            #region add services
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddWebServices();
            #endregion

            var app = builder.Build();

            #region Add Midlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            await app.InitialiseDatabaseAsync();
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{area?}/{controller=Home}/{action=Index}/{id?}");
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Client/Home/Index");
                return Task.CompletedTask;
            });
            app.MapRazorPages();
            app.Run();
        }
    }
}
