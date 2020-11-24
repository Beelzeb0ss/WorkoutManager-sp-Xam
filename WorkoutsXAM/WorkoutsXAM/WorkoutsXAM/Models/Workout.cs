using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WorkoutsXAM.Models
{
    public class Workout
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [ManyToMany(typeof(WorkoutExercise))]
        public List<Exercise> Exercises { get; set; }

        public string Details { get; set; }

        public int IsCurrent { get; set; }

        public int IsNext { get; set; }
    }
}
