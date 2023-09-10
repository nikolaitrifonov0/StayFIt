using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Services.BodyParts;
using StayFit.Services.Calendar;
using StayFit.Services.Equipments;
using StayFit.Services.Exercises;
using StayFit.Services.Statistics;
using StayFit.Services.Users;
using StayFit.Services.Workouts;
using StayFit.Web.Infrastructure;
using System.IO;

namespace StayFit.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)        
            => Configuration = configuration;        

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StayFitContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")))
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StayFitContext>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();

            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.AddTransient<IExerciseServices, ExerciseServices>();
            services.AddTransient<IWorkoutServices, WorkoutServices>();
            services.AddTransient<IBodyPartServices, BodyPartServices>();
            services.AddTransient<IEquipmentServices, EquipmentServices>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IStatisticsServices, StatisticsServices>();
            services.AddTransient<ICalendarDataProvider, CalendarDataProvider>();
            services.AddTransient<ICalendarUpdater, CalendarUpdater>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();
            app.UseStatusCodePages();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                    .UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error")
                    .UseHsts();
            }
            app.UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
        }
    }
}
