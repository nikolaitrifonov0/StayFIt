using MyTested.AspNetCore.Mvc;
using StayFit.Data.Models;
using StayFit.Web.Controllers;
using StayFit.Web.Models.Exercises;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.Pipeline
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
            4,
            new int[] { 1, 2, 3 },
            "https://image.shutterstock.com/image-vector/sequence-weightlifter-doing-deadlift-exercise-260nw-1482332555.jpg",
            "https://www.youtube.com/watch?v=r4MzxtBKyNE")]
        public void PostAddShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(string name,
            string description,
            int equipment,
            IEnumerable<int> bodyParts,
            string imageUrl,
            string videoUrl)
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Exercises/Add")
                .WithMethod(HttpMethod.Post))
                .To<ExercisesController>(e => e.Add(With.Any<AddExerciseFormModel>()));

            MyController<ExercisesController>
                .Instance(controller => controller
                .WithUser())
                .Calling(c => c.Add(new AddExerciseFormModel
                {
                    Name = name,
                    Description = description,
                    Equipment = equipment,
                    BodyParts = bodyParts,
                    ImageUrl = imageUrl,
                    VideoUrl = videoUrl
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState()
                .Data(data => data
            .WithSet<Exercise>(exercises =>
                exercises.ToList()
                .Any(e =>
                    e.Name == name &&
                    e.Description == description &&
                    e.EquipmentId == equipment &&
                    e.BodyParts.Select(b => b.Id).All(b => bodyParts.Contains(b)) &&
                    e.ImageUrl == imageUrl &&
                    e.VideoUrl == videoUrl)))
            .AndAlso()
            .ShouldReturn()
            .Redirect(redirect => redirect
            .To<HomeController>(h => h.Index()));
            ;
        }
    }
}
