using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Services.Equipments;
using static StayFit.Web.Areas.Admin.AdminConstants;

namespace StayFit.Web.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly IEquipmentServices equipments;

        public EquipmentsController(IEquipmentServices equipments)
        {
            this.equipments = equipments;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult All()
        {
            return View(equipments.All());
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Add([FromBody] EquipmentServiceModel model)
        {
            this.equipments.Create(model);
            return Ok(new { Status = "Ok" });
        }
    }
}
