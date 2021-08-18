using MyTested.AspNetCore.Mvc;
using StayFit.Services.Workouts;
using StayFit.Test.Data;
using StayFit.Web.Areas.Admin;
using StayFit.Web.Controllers;
using StayFit.Web.Models.Workouts;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.ControllerTests
{
    public class WorkoutsControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Workouts/All"))
            .To<WorkoutsController>(w => w.All())
            .Which()
            .ShouldReturn()
            .View();

        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Workouts/Add")
                .WithUser())
            .To<WorkoutsController>(e => e.Add())
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData("PPL",
            "Tough workout",
            null,
            1)]
        public void PostAddShouldBeForAuthorizedUsersAndHaveValidModelAndRedirects(string name, string description, int? cycleDays,
            int workoutCycleType)
        {
            MyRouting
               .Configuration()
               .ShouldMap(request => request
                   .WithPath("/Workouts/Add")
                   .WithMethod(HttpMethod.Post))
               .To<WorkoutsController>(w => w.Add(With.Any<AddWorkoutFormModel>()));

            var exercise = ExerciseProvider.FourExercises().First();


            MyController<WorkoutsController>
                .Instance(controller => controller.WithUser()
                .WithData(exercise))
                .Calling(w => w.Add(new AddWorkoutFormModel
                {
                    Name = name,
                    Description = description,
                    CycleDays = cycleDays,
                    WorkoutCycleType = workoutCycleType,
                    Exercises = new List<string>
                    {
                        "Day1 - " + exercise.Name
                    }
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .Redirect(redirect => redirect
                    .To<HomeController>(h => h.Index()));
        }

        [Fact]
        public void DetailsShouldReturnView()
        {
            var workout = WorkoutProvider.TwoWorkouts().First();

            MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath($"/Workouts/Details/{workout.Id}")
                .WithUser())
            .To<WorkoutsController>(e => e.Details(workout.Id))
            .Which(controller => controller
                .WithData(workout))
            .ShouldReturn()
            .View();
        }

        [Fact]
        public void AssignShouldBeForAuthorizedUsersAndShouldRedirect()
        {
            var workout = WorkoutProvider.TwoWorkouts().First();

            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Workouts/Assign/{workout.Id}")
                    .WithUser())
                .To<WorkoutsController>(e => e.Assign(workout.Id))
            .Which(controller => controller
                .WithData(workout))
            .ShouldReturn()
            .Redirect(redirect => redirect.To<UsersController>(u => u.Log()));
        }

        [Fact]
        public void GetEditShouldBeForCreatorAndReturnView()
        {
            var workout = WorkoutProvider.TwoWorkouts().First();

            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Workouts/Edit/{workout.Id}")
                    .WithUser(workout.Creator.Id, workout.Creator.UserName))
                .To<WorkoutsController>(e => e.Edit(workout.Id))
            .Which(controller => controller
                .WithData(workout))
            .ShouldReturn()
            .View();
        }

        [Theory]
        [InlineData("PPL",
            "Tough workout",
            null,
            1)]
        public void PostEditShouldBeForAdminAndRedirect(string name, string description, int? cycleDays,
            int workoutCycleType)
        {
            var workout = WorkoutProvider.TwoWorkouts().First();

            MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Workouts/Edit/{workout.Id}")
                    .WithMethod(HttpMethod.Post))
                .To<WorkoutsController>(w => w.Edit(workout.Id, With.Any<EditWorkoutsServiceModel>()));

            MyController<WorkoutsController>
                 .Instance(controller => controller.WithUser(u => u.InRole(AdminConstants.AdministratorRoleName))
                 .WithData(workout))
                 .Calling(w => w.Edit(workout.Id, new EditWorkoutsServiceModel
                 {
                     Name = name,
                     Description = description,
                     CycleDays = cycleDays,
                     WorkoutCycleType = workoutCycleType
                 }))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                     .RestrictingForHttpMethod(HttpMethod.Post))
                 .ValidModelState()
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                     .To<WorkoutsController>(e => e.Details(workout.Id)));
        }
    }
}
