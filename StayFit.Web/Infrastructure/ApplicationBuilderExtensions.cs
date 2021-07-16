using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StayFit.Data;
using StayFit.Data.Models;
using System.Linq;

namespace StayFit.Web.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<StayFitContext>();

            PopulateBodyParts(data);

            PopulateEquipments(data);

            data.Database.Migrate();

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
    }
}
