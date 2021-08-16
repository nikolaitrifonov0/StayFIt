using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Services.BodyParts;
using StayFit.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StayFit.Test.ServiceTests
{
    public class BodyPartServicesTest
    {
        private IMapper mapper;

        public BodyPartServicesTest()
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
        public void AllShouldReturnAllBodyParts()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var bodyParts = new BodyPartServices(data, mapper);

            var toAdd = new List<BodyPart>()
            {
                new BodyPart
                {
                    Name = "Back"
                },
                new BodyPart
                {
                    Name = "Chest"
                },
                new BodyPart
                {
                    Name = "Biceps"
                }
            };

            data.BodyParts.AddRange(toAdd);
            data.SaveChanges();

            var result = bodyParts.All();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, b => b.Name == "Back");
            Assert.Contains(result, b => b.Name == "Chest");
            Assert.Contains(result, b => b.Name == "Biceps");
        }

        [Fact]
        public void AllWithEmptyDatabaseShouldReturnEmptyCollection()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var bodyParts = new BodyPartServices(data, mapper);
            
            Assert.Empty(bodyParts.All());
        }

        [Fact]
        public void DoesBodyPartExistShouldReturnTrue()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var bodyParts = new BodyPartServices(data, mapper);

            var toAdd = new List<BodyPart>()
            {
                new BodyPart
                {
                    Name = "Back"
                },
                new BodyPart
                {
                    Name = "Chest"
                },
                new BodyPart
                {
                    Name = "Biceps"
                }
            };

            data.BodyParts.AddRange(toAdd);
            data.SaveChanges();

            var bodyPart = data.BodyParts.First();
            var result = bodyParts.DoesBodyPartExist(bodyPart.Id);

            Assert.True(result);
        }

        [Fact]
        public void DoesBodyPartExistShouldReturnFalse()
        {
            var optionsBuilder = new DbContextOptionsBuilder<StayFitContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var data = new StayFitContext(optionsBuilder.Options);
            var bodyParts = new BodyPartServices(data, mapper);

            var result = bodyParts.DoesBodyPartExist(0);

            Assert.False(result);
        }
    }
}
