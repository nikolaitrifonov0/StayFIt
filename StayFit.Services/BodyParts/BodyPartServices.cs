using AutoMapper;
using AutoMapper.QueryableExtensions;
using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.BodyParts
{
    public class BodyPartServices : IBodyPartServices
    {
        private readonly StayFitContext data;
        private readonly IConfigurationProvider mapper;

        public BodyPartServices(StayFitContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<BodyPartServiceModel> All()
            => this.data.BodyParts
            .ProjectTo<BodyPartServiceModel>(this.mapper)
            .OrderBy(bp => bp.Name).ToList();

        public bool DoesBodyPartExist(int id) => this.data.BodyParts.Any(bp => bp.Id == id);
    }
}
