using MyTested.AspNetCore.Mvc;
using StayFit.Web.Controllers;
using Xunit;

namespace StayFit.Test.ControllerTests
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldRedirectWithAuthenticatedUser()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Home/")
                .WithUser())
            .To<HomeController>(e => e.Index())
            .Which()
            .ShouldReturn()
            .Redirect(redirect => redirect.To<UsersController>(u => u.Log()));

        [Fact]
        public void ErrorShouldReturnView()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Home/Error"))
            .To<HomeController>(e => e.Error())
            .Which()
            .ShouldReturn()
            .View();
    }
}
