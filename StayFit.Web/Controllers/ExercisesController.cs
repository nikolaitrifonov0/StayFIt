using Microsoft.AspNetCore.Mvc;
using StayFit.Data;
using StayFit.Web.Models.Exercises;
using System;
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

        public IActionResult Add() => View(new AddExerciseFormModel
        {
            Equipments = this.SelectEquipments(),
            BodyParts = this.SelectBodyParts()
        });

        private IEnumerable<ExerciseBodyPartViewModel> SelectBodyParts()
        => this.data.BodyParts.Select(bp => new ExerciseBodyPartViewModel
        {
            Id = bp.Id,
            Name = bp.Name
        }).ToList();

        private IEnumerable<ExerciseEquipmentViewModel> SelectEquipments()
        => this.data.Equipments.Select(e => new ExerciseEquipmentViewModel
        {
            Id = e.Id,
            Name = e.Name
        }).ToList();
    }
}
