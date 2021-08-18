using MyTested.AspNetCore.Mvc;
using StayFit.Services.Users;
using StayFit.Web.Controllers;
using Xunit;

namespace StayFit.Test.ControllerTests
{
    public class UsersControllerTest
    {
        [Fact]
        public void GetLogShouldBeForAuthorizedUsersAndReturnView()
        => MyPipeline
        .Configuration()
        .ShouldMap(request => request
            .WithPath("/Users/Log")
            .WithUser())
        .To<UsersController>(e => e.Log())
        .Which()
        .ShouldHave()
        .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
        .AndAlso()
        .ShouldReturn()
        .View();

        [Fact]
        public void PostLogShouldBeForAuthorizedUsersAndRedirect() 
        => MyRouting
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Users/Log")
                .WithMethod(HttpMethod.Post))
            .To<UsersController>(e => e.Log(With.Any<LogWorkoutForUserServiceModel>()));
    }
}
