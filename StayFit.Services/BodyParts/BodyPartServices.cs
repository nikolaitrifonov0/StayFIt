using AutoMapper;
using AutoMapper.QueryableExtensions;
using StayFit.Data;
using StayFit.Data.Models;
using System;
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

        public void Create(BodyPartServiceModel model)
        {
            if (this.data.BodyParts.Any(b => b.Name == model.Name))
            {
                throw new ArgumentException("There is already a body part with this name.");
            }

            var bodyPart = new BodyPart
            {
                Name = model.Name
            };

            this.data.BodyParts.Add(bodyPart);
            this.data.SaveChanges();
        }

        public bool DoesBodyPartExist(int id) => this.data.BodyParts.Any(bp => bp.Id == id);
    }
}
