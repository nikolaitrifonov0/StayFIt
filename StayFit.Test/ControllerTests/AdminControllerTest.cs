using MyTested.AspNetCore.Mvc;
using StayFit.Test.Data;
using StayFit.Web.Areas.Admin;
using StayFit.Web.Areas.Admin.Controllers;
using System.Linq;
using Xunit;

namespace StayFit.Test.ControllerTests
{
    public class AdminControllerTest
    {
        [Fact]
        public void AllShouldBeForAdminsAndReturnView()
         => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithUser(u => u.InRole(AdminConstants.AdministratorRoleName))
                .WithPath($"/{AdminConstants.AreaName}/Exercises/All"))
            .To<ExercisesController>(e => e.All())
            .Which()
            .ShouldReturn()
            .View();
        [Fact]
        public void ShowShouldBeForAdminsAndReturnView()
        {
            var exercise = ExerciseProvider.FourExercises().First();
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithUser(u => u.InRole(AdminConstants.AdministratorRoleName))
                    .WithPath($"/{AdminConstants.AreaName}/Exercises/Show/{exercise.Id}"))
                .To<ExercisesController>(e => e.Show(exercise.Id))
                .Which(c => c.WithData(exercise))
                .ShouldReturn()
                .Redirect(redirect => redirect.To<ExercisesController>(e => e.All()));
        }

        [Fact]
        public void HideShouldBeForAdminsAndReturnView()
        {
            var exercise = ExerciseProvider.FourExercises().First();
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithUser(u => u.InRole(AdminConstants.AdministratorRoleName))
                    .WithPath($"/{AdminConstants.AreaName}/Exercises/Hide/{exercise.Id}"))
                .To<ExercisesController>(e => e.Hide(exercise.Id))
                .Which(c => c.WithData(exercise))
                .ShouldReturn()
                .Redirect(redirect => redirect.To<ExercisesController>(e => e.All()));
        }
    }
}
