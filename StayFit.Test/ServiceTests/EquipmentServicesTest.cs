using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Services.Equipments;
using StayFit.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.ServiceTests
{
    public class EquipmentServicesTest
    {
        private IMapper mapper;

        public EquipmentServicesTest()
        {
            if (this.mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                this.mapper = mapper;
            }
        }

        [Fact]
        public void AllShouldReturnAllEquipments()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var equipments = new EquipmentServices(data, mapper);

            var toAdd = new List<Equipment>()
            {
                new Equipment
                {
                    Name = "Barbell"
                },
                new Equipment
                {
                    Name = "Dumbell"
                },
                new Equipment
                {
                    Name = "BodyWeight"
                }
            };

            data.Equipments.AddRange(toAdd);
            data.SaveChanges();

            var result = equipments.All();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, e => e.Name == "Barbell");
            Assert.Contains(result, e => e.Name == "Dumbell");
            Assert.Contains(result, e => e.Name == "BodyWeight");
        }

        [Fact]
        public void AllWithEmptyDatabaseShouldReturnEmptyCollection()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var equipments = new EquipmentServices(data, mapper);

            Assert.Empty(equipments.All());
        }

        [Fact]
        public void DoesEquipmentExistShouldReturnTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var equipments = new EquipmentServices(data, mapper);

            var toAdd = new List<Equipment>()
            {
                new Equipment
                {
                    Name = "Barbell"
                },
                new Equipment
                {
                    Name = "Dumbell"
                },
                new Equipment
                {
                    Name = "BodyWeight"
                }
            };

            data.Equipments.AddRange(toAdd);
            data.SaveChanges();

            var equipment = data.Equipments.First();
            var result = equipments.DoesEquipmentExist(equipment.Id);

            Assert.True(result);
        }

        [Fact]
        public void DoesEquipmentExistShouldReturnFalse()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var equipments = new EquipmentServices(data, mapper);

            var result = equipments.DoesEquipmentExist(0);

            Assert.False(result);
        }
    }
}
