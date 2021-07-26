using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Equipments
{
    public class EquipmentServices : IEquipmentServices
    {
        private readonly StayFitContext data;

        public EquipmentServices(StayFitContext data) => this.data = data;

        public IEnumerable<EquipmentServiceModel> All()
            => this.data.Equipments.Select(e => new EquipmentServiceModel
            {
                Id = e.Id,
                Name = e.Name
            }).OrderBy(e => e.Name).ToList();

        public bool DoesEquipmentExist(int id) => this.data.Equipments.Any(e => e.Id == id);
    }
}
