using StayFit.Services.BodyParts;
using StayFit.Services.Equipments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static StayFit.Data.DataConstants;

namespace StayFit.Web.Models.Exercises
{
    public class AddExerciseFormModel
    {
        [Required]
        [StringLength(ExerciseNameMaxLength,
            MinimumLength = ExerciseNameMinLength,
            ErrorMessage = "The name should be between {2} and {1} symbols long.")]
        public string Name { get; init; }
        [Required]
        [StringLength(ExerciseDescriptionMaxLength, 
            MinimumLength = ExerciseDescriptionMinLength,
            ErrorMessage = "The description should be between {2} and {1} symbols long.")]
        public string Description { get; init; }
        [Required]
        public int Equipment { get; init; }
        [Required]
        public IEnumerable<int> BodyParts { get; init; }
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }
        [Url]
        [Display(Name = "Video URL")]
        public string VideoUrl { get; init; }

        public IEnumerable<EquipmentServiceModel> Equipments { get; set; }
        public IEnumerable<BodyPartServiceModel> BodyPartsDisplay { get; set; }
    }
}
