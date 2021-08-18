using MyTested.AspNetCore.Mvc;
using StayFit.Web.Controllers;
using Xunit;

namespace StayFit.Test.ControllerTests
{
    public class StatisticsControllerTest
    {
        [Fact]
        public void GetShouldBeForAuthorizedUsersAndReturnView()
        => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Statistics/Get")
                .WithUser())
            .To<StatisticsController>(e => e.Get())
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes.RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();
    }
}
