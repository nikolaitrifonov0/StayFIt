﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StayFit.Web.Models.Workouts
{
    public class DetailsWorkDayViewModel
    {
        public string Day { get; set; }
        public IEnumerable<string> Exercises { get; set; }
        public DateTime NextWorkout { get; init; }
    }
}

