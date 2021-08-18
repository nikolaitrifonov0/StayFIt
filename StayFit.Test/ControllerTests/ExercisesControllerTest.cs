using MyTested.AspNetCore.Mvc;
using StayFit.Data.Models;
using StayFit.Services.Exercises;
using StayFit.Test.Data;
using StayFit.Web.Areas.Admin;
using StayFit.Web.Controllers;
using StayFit.Web.Models.Exercises;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.ControllerTests
{
    public class ExercisesControllerTest
    {
        [Fact]
        public void GetAddShouldBeForAuthorizedUsersAndReturnView()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Exercises/Add")
                .WithUser())
            .To<ExercisesController>(e => e.Add())
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        [Theory]
        [InlineData("My Exercise",
            "The best exercise for the back",
            "https://image.shutterstock.com/image-vector/sequence-weightlifter-doing-deadlift-exercise-260nw-1482332555.jpg",
            "https://www.youtube.com/watch?v=r4MzxtBKyNE",
            1,
            new int[0])]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(string name,
        string description,
        string imageUrl,
        string videoUrl,
        int equipment,
        IEnumerable<int> bodyParts)
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Exercises/Add")
                    .WithMethod(HttpMethod.Post))
                .To<ExercisesController>(e => e.Add(With.Any<AddExerciseFormModel>()));
            
            MyController<ExercisesController>
                .Instance(controller => controller.WithUser()
                .WithData(new Equipment { Id = 1, Name = "Barbel" }))
                .Calling(e => e.Add(new AddExerciseFormModel
                {
                    Name = name,
                    Description = description,
                    ImageUrl = imageUrl, 
                    VideoUrl = videoUrl,
                    Equipment = equipment,
                    BodyParts = bodyParts
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
        public void FindShouldReturnTheMatchingExercises()
        => MyController<ExercisesController>
            .Instance(controller => controller.WithData(ExerciseProvider.FourExercises()))
            .Calling(e => e.Find("press"))
            .ShouldReturn()
            .Ok(res => res.WithModelOfType<List<ExerciseSearchServiceModel>>()
                .Passing(res => res.Count == 2));

        [Fact]
        public void AllShouldReturnView()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Exercises/All"))
            .To<ExercisesController>(e => e.All())
            .Which()
            .ShouldReturn()
            .View();

        [Fact]
        public void DetailsShouldReturnView()
        {
            var exercise = ExerciseProvider.FourExercises().First();

            MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath($"/Exercises/Details/{exercise.Id}")
                .WithUser())
            .To<ExercisesController>(e => e.Details(exercise.Id))
            .Which(controller => controller
                .WithData(exercise))
            .ShouldReturn()
            .View();
        }

        [Fact]
        public void GetEditShouldBeForAdministratorsOnlyAndShoudReturnView()
        {
            var exercise = ExerciseProvider.FourExercises().First();

            MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath($"/Exercises/Edit/{exercise.Id}")
                .WithUser(u => u.InRole(AdminConstants.AdministratorRoleName)))
            .To<ExercisesController>(e => e.Edit(exercise.Id))
            .Which(controller => controller
                .WithData(exercise))
            .ShouldHave()
            .ActionAttributes(attributes =>
                attributes.RestrictingForAuthorizedRequests(AdminConstants.AdministratorRoleName))
            .AndAlso()
            .ShouldReturn()
            .View();
        }

        [Theory]
        [InlineData("My Exercise",
            "The best exercise for the back",
            "https://image.shutterstock.com/image-vector/sequence-weightlifter-doing-deadlift-exercise-260nw-1482332555.jpg",
            "https://www.youtube.com/watch?v=r4MzxtBKyNE",
            1,
            new int[0])]
        public void PostEditShouldBeForAdministratorsAndReturnRedirectWithValidModel(string name,
        string description,
        string imageUrl,
        string videoUrl,
        int equipment,
        IEnumerable<int> bodyParts)
        {
            var exercise = ExerciseProvider.FourExercises().First();

            MyRouting
                .Configuration()
                .ShouldMap(request => request
                    .WithPath($"/Exercises/Edit/{exercise.Id}")
                    .WithMethod(HttpMethod.Post))
                .To<ExercisesController>(e => e.Edit(exercise.Id ,With.Any<ExerciseEditServiceModel>()));

            MyController<ExercisesController>
                 .Instance(controller => controller.WithUser(u => u.InRole(AdminConstants.AdministratorRoleName))
                 .WithData(exercise))
                 .Calling(e => e.Edit(exercise.Id ,new ExerciseEditServiceModel
                 {
                     Name = name,
                     Description = description,
                     ImageUrl = imageUrl,
                     VideoUrl = videoUrl,
                     Equipment = equipment,
                     BodyParts = bodyParts
                 }))
                 .ShouldHave()
                 .ActionAttributes(attributes => attributes
                     .RestrictingForHttpMethod(HttpMethod.Post)
                     .RestrictingForAuthorizedRequests(AdminConstants.AdministratorRoleName))
                 .ValidModelState()
                 .AndAlso()
                 .ShouldReturn()
                 .Redirect(redirect => redirect
                     .To<ExercisesController>(e => e.Details(exercise.Id)));
        }
    }
}
