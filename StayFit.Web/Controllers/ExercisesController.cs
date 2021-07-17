using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Data.Models;
using StayFit.Web.Models.Exercises;
using System.Collections.Generic;
using System.Linq;

namespace StayFit.Web.Controllers
{
    public class ExercisesController : Controller
    {
        private StayFitContext data;

        public ExercisesController(StayFitContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Add() => View(new AddExerciseFormModel
        {
            Equipments = this.SelectEquipments(),
            BodyPartsDisplay = this.SelectBodyParts()
        });

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddExerciseFormModel exercise)
        {
            if (!this.data.Equipments.Any(e => e.Id == exercise.Equipment))
            {
                this.ModelState.AddModelError(nameof(exercise.Equipment), "Equipment does not exist.");
            }

            if (exercise.BodyParts != null)
            {
                foreach (var bodyPart in exercise.BodyParts)
                {
                    if (!this.data.BodyParts.Any(bp => bp.Id == bodyPart))
                    {
                        this.ModelState.AddModelError(nameof(exercise.BodyParts), "Muscle group does not exist.");
                    }
                }
            }

            if (!this.ModelState.IsValid)
            {
                exercise.Equipments = this.SelectEquipments();
                exercise.BodyPartsDisplay = this.SelectBodyParts();

                return View(exercise);
            }

            var toAdd = new Exercise
            {
                Name = exercise.Name,
                Description = exercise.Description,
                ImageUrl = exercise.ImageUrl,
                VideoUrl = exercise.VideoUrl,
                EquipmentId = exercise.Equipment
            };

            foreach (var bodyPart in exercise.BodyParts)
            {
                toAdd.BodyParts.Add(data.BodyParts.Find(bodyPart));
            }

            data.Exercises.Add(toAdd);
            data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Find(string keyword)
        {
            var exercises = data.Exercises
                .Select(e => new ExerciseSearchViewModel { Id = e.Id, Name = e.Name })
                .Where(e => e.Name.Contains(keyword))
                .ToList();

            return Json(exercises);
        }

        private IEnumerable<ExerciseBodyPartViewModel> SelectBodyParts()
        => this.data.BodyParts.Select(bp => new ExerciseBodyPartViewModel
        {
            Id = bp.Id,
            Name = bp.Name
        }).OrderBy(bp => bp.Name).ToList();

        private IEnumerable<ExerciseEquipmentViewModel> SelectEquipments()
        => this.data.Equipments.Select(e => new ExerciseEquipmentViewModel
        {
            Id = e.Id,
            Name = e.Name
        }).OrderBy(e => e.Name).ToList();
    }
}
