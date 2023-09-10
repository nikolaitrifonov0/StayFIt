using AutoMapper;
using StayFit.Data.Models;
using StayFit.Services.BodyParts;
using StayFit.Services.Equipments;
using StayFit.Services.Exercises;
using StayFit.Services.Workouts;
using StayFit.Web.Models.Workouts;
using System;
using System.Linq;

namespace StayFit.Web.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<WorkoutDetailsServiceModel, DetailsWorkoutViewModel>();
            this.CreateMap<DetailsWorkDayServiceModel, DetailsWorkDayViewModel>();

            this.CreateMap<BodyPart, BodyPartServiceModel>();

            this.CreateMap<Equipment, EquipmentServiceModel>();

            this.CreateMap<Exercise, ExerciseSearchServiceModel>();
            this.CreateMap<Exercise, ExerciseDetailsServiceModel>()
                .ForMember(e => e.VideoUrl, cfg => cfg.MapFrom(e => ExtractVideoUrl(e.VideoUrl)))
                .ForMember(e => e.Equipment, cfg => cfg.MapFrom(e => e.Equipment.Name))
                .ForMember(e => e.BodyParts, cfg => cfg.MapFrom(e => e.BodyParts.Select(bp => bp.Name).ToList()));
            this.CreateMap<Exercise, ExerciseEditServiceModel>()
                .ForMember(e => e.BodyParts, cfg => cfg.MapFrom(e => e.BodyParts.Select(b => b.Id).ToList()))
                .ForMember(e => e.Equipment, cfg => cfg.MapFrom(e => e.EquipmentId));
            this.CreateMap<Exercise, ExerciseSearchServiceModel>();

            this.CreateMap<Workout, AllWorkoutsServiceModel>()
                .ForMember(w => w.Creator, cfg => cfg.MapFrom(w => w.Creator.UserName))
                .ForMember(w => w.TotalWorkDays, cfg => cfg.MapFrom(w => w.WorkDays.Count));
            this.CreateMap<Workout, EditWorkoutsServiceModel>();
        }

        private static string ExtractVideoUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            var youtubeEmbedLink = "https://www.youtube.com/embed/";
            var youtubeLinkSeparator = "v=";

            return youtubeEmbedLink + url.Split(youtubeLinkSeparator, StringSplitOptions.RemoveEmptyEntries)[1];
        }
    }
}
