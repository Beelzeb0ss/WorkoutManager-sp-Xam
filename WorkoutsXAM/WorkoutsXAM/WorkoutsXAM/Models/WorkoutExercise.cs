using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutsXAM.Models
{
    public class WorkoutExercise
    {
        [ForeignKey(typeof(Workout))]
        public int WorkoutId { get; set; }

        [ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }
    }
}
