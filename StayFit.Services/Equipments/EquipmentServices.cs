using AutoMapper;
using AutoMapper.QueryableExtensions;
using StayFit.Data;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Services.Equipments
{
    public class EquipmentServices : IEquipmentServices
    {
        private readonly StayFitContext data;
        private readonly IConfigurationProvider mapper;

        public EquipmentServices(StayFitContext data, IMapper mapper)
        {
           this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }
        public IEnumerable<EquipmentServiceModel> All()
            => this.data.Equipments
            .ProjectTo<EquipmentServiceModel>(this.mapper)
            .OrderBy(e => e.Name).ToList();

        public bool DoesEquipmentExist(int id) => this.data.Equipments.Any(e => e.Id == id);
    }
}
