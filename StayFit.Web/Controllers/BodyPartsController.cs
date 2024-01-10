using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.BodyParts;

using static StayFit.Web.Areas.Admin.AdminConstants;

namespace StayFit.Web.Controllers
{
    public class BodyPartsController : Controller
    {
        private readonly IBodyPartServices bodyParts;

        public BodyPartsController(IBodyPartServices bodyParts)
        {
            this.bodyParts = bodyParts;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult All()
        {
            return View(bodyParts.All());
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Add([FromBody] BodyPartServiceModel model)
        {
            this.bodyParts.Create(model);
            return Ok(new { Status = "Ok" });
        }
    }
}
