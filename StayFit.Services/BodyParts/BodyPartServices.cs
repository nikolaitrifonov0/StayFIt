using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.BodyParts
{
    public class BodyPartServices : IBodyPartServices
    {
        private readonly StayFitContext data;

        public BodyPartServices(StayFitContext data) => this.data = data;

        public IEnumerable<BodyPartServiceModel> All()
            => this.data.BodyParts.Select(bp => new BodyPartServiceModel
            {
                Id = bp.Id,
                Name = bp.Name
            }).OrderBy(bp => bp.Name).ToList();

        public bool DoesBodyPartExist(int id) => this.data.BodyParts.Any(bp => bp.Id == id);
    }
}
