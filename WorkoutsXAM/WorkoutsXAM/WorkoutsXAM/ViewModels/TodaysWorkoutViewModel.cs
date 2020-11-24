using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;
using WorkoutsXAM.Utility;
using Xamarin.Forms;

namespace WorkoutsXAM.ViewModels
{
    class TodaysWorkoutViewModel : BaseViewModel
    {
        private Workout workout;
        public Workout Workout
        {
            get{ return workout; }
            set{ SetProperty(ref workout, value); }
        }

        public Command NextWorkoutCommand { get; set; }
        public Command LoadWorkoutCommand { get; set; }

        public TodaysWorkoutViewModel()
        {
            InitCommands();

            Title = "Today's workout";
        }

        private void InitCommands()
        {
            NextWorkoutCommand = new Command(async () => await NextWorkout());
            LoadWorkoutCommand = new Command(async () => await LoadWorkout());
        }

        async Task LoadWorkout()
        {
            var items = await Database.WorkoutRepo.Get<Workout>(w => w.IsNext != 0);
            if(items.Count < 1)
            {
                var currentWorkouts = await Database.WorkoutRepo.Get<Workout>(x => x.IsCurrent != 0);
                if (currentWorkouts.Count == 0) return;
                currentWorkouts = currentWorkouts.OrderBy(x => x.IsCurrent).ToList();
                if (currentWorkouts.Count > 0) currentWorkouts[0].IsNext = 1;
                Workout = currentWorkouts[0];
                await Database.WorkoutRepo.SetNext(currentWorkouts[0]);
                return;
            }
            Workout = items[0];
        }

        async Task NextWorkout()
        {
            var current = await Database.WorkoutRepo.Get<Workout>(x => x.IsCurrent != 0);
            if (current.Count == 0) return;
            current = current.OrderBy(x => x.IsCurrent).ToList();

            int index = -1;         
            for(int i = 0; i < current.Count; i++)
            {
                if(current[i].Id == Workout.Id)
                {
                    index = i;
                }
            }

            if(index < current.Count - 1)
            {
                await Database.WorkoutRepo.SetNext(current[index+1]);
                Workout = current[index + 1];
            }
            else
            {
                await Database.WorkoutRepo.SetNext(current[0]);
                Workout = current[0];
            }
            OnPropertyChanged("Workout");
        }
    }
}
