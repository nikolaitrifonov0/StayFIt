using AutoMapper;
using AutoMapper.QueryableExtensions;
using StayFit.Data;
using StayFit.Data.Models;
using System;
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

        public void Create(EquipmentServiceModel model)
        {
            if (this.data.Equipments.Any(e => e.Name == model.Name))
            {
                throw new ArgumentException("There is already an equipment with this name.");
            }

            var equipment = new Equipment
            {
                Name = model.Name
            };
            this.data.Equipments.Add(equipment);
            this.data.SaveChanges();
        }

        public bool DoesEquipmentExist(int id) => this.data.Equipments.Any(e => e.Id == id);
    }
}
