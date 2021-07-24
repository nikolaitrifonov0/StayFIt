using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Services.BodyParts;
using StayFit.Services.Equipments;
using StayFit.Services.Exercises;
using StayFit.Web.Models.Exercises;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Web.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IExerciseServices exercises;
        private readonly IEquipmentServices equipments;
        private readonly IBodyPartServices bodyParts;

        public ExercisesController(IExerciseServices exercises, 
            IEquipmentServices equipments, IBodyPartServices bodyParts)
        {
            this.exercises = exercises;
            this.equipments = equipments;
            this.bodyParts = bodyParts;
        }

        [Authorize]
        public IActionResult Add() => View(new AddExerciseFormModel
        {
            Equipments = this.equipments.All(),
            BodyPartsDisplay = this.bodyParts.All()
        });

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddExerciseFormModel exercise)
        {
            if (!this.equipments.DoesEquipmentExist(exercise.Equipment))
            {
                this.ModelState.AddModelError(nameof(exercise.Equipment), "Equipment does not exist.");
            }

            if (exercise.BodyParts != null)
            {
                foreach (var bodyPart in exercise.BodyParts)
                {
                    if (!this.bodyParts.DoesBodyPartExist(bodyPart))
                    {
                        this.ModelState.AddModelError(nameof(exercise.BodyParts), "Muscle group does not exist.");
                    }
                }
            }

            if (!this.ModelState.IsValid)
            {
                exercise.Equipments = this.equipments.All();
                exercise.BodyPartsDisplay = this.bodyParts.All();

                return View(exercise);
            }

            exercises.Add(exercise.Name, exercise.Description, exercise.ImageUrl, exercise.VideoUrl,
                exercise.Equipment, exercise.BodyParts);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Find(string keyword)
            => Ok(this.exercises.Find(keyword));

        public IActionResult Details(string id)
        {
            var exercise = this.exercises.Details(id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }            
    }
}
