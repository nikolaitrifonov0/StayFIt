using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StayFit.Data;
using StayFit.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

using static StayFit.Web.Areas.Admin.AdminConstants;

namespace StayFit.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;

            var data = services.GetRequiredService<StayFitContext>();

            data.Database.Migrate();

            PopulateBodyParts(data);

            PopulateEquipments(data);

            SeedAdministrator(services);

            return app;
        }

        private static void PopulateEquipments(StayFitContext data)
        {
            if (data.Equipments.Any())
            {
                return;
            }

            data.Equipments.AddRange(new[]
            {
                new Equipment { Name = "Bands" },
                new Equipment { Name = "Foam Roll" },
                new Equipment { Name = "Barbell" },
                new Equipment { Name = "Kettlebells" },
                new Equipment { Name = "Bodyweight" },
                new Equipment { Name = "Machine" },
                new Equipment { Name = "Cable" },
                new Equipment { Name = "Medicine Ball" },
                new Equipment { Name = "Dumbbell" },
                new Equipment { Name = "None" },
                new Equipment { Name = "EZ-Curl Bar" },
                new Equipment { Name = "Other" },
                new Equipment { Name = "Exercise Ball" },
            });

            data.SaveChanges();
        }

        private static void PopulateBodyParts(StayFitContext data)
        {
            if (data.BodyParts.Any())
            {
                return;
            }

            data.BodyParts.AddRange(new[]
            {
                new BodyPart { Name = "Neck" },
                new BodyPart { Name = "Traps" },
                new BodyPart { Name = "Shoulders" },
                new BodyPart { Name = "Chest" },
                new BodyPart { Name = "Biceps" },
                new BodyPart { Name = "Triceps" },
                new BodyPart { Name = "Forearm" },
                new BodyPart { Name = "Abs" },
                new BodyPart { Name = "Lats" },
                new BodyPart { Name = "Middle Back" },
                new BodyPart { Name = "Lower Back" },
                new BodyPart { Name = "Glutes" },
                new BodyPart { Name = "Quads" },
                new BodyPart { Name = "Hamstrings" },
                new BodyPart { Name = "Calves" },
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        //return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@stayfit.com";
                    const string adminPassword = "admin123";

                    var user = new IdentityUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);

                    await userManager.UpdateSecurityStampAsync(user);                      
                })
                .GetAwaiter()
                .GetResult();            
        }
    }
}
