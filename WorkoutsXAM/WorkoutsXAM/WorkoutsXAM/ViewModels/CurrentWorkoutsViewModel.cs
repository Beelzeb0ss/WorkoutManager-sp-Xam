using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;
using Xamarin.Forms;

namespace WorkoutsXAM.ViewModels
{
    class CurrentWorkoutsViewModel : AllWorkoutsViewModel
    {
        public Command MoveWorkoutUpCommand { get; set; }

        public CurrentWorkoutsViewModel(INavigation nav) : base(nav)
        {
            InitCommands();

            Title = "Current";
        }

        private void InitCommands()
        {
            MoveWorkoutUpCommand = new Command<object>(async (x) => await MoveWorkoutUp(x), (x) => CanMoveWorkoutUp(x));
        }

        protected async override Task LoadWorkouts()
        {
            IsBusy = true;
            try
            {
                Workouts.Clear();
                List<Workout> items = await Database.WorkoutRepo.Get<Workout>(w => w.IsCurrent != 0);
                items = items.OrderBy(w => w.IsCurrent).ToList();
                for(int i = 0; i < items.Count; i++)
                {
                    items[i].IsCurrent = i + 2;
                    Debug.WriteLine(items[i].Name + " : " + items[i].IsCurrent);
                    await Database.WorkoutRepo.Update(items[i]);
                    Workouts.Add(items[i]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task MoveWorkoutUp(object para)
        {
            try
            {
                Workout w = (Workout)para;
                int index = Workouts.IndexOf(w);
                Debug.WriteLine(index);
                if(index > 0)
                {               
                    if(w.IsCurrent < 2)
                    {
                        foreach(Workout workout in Workouts)
                        {
                            workout.IsCurrent += 2;
                            await Database.WorkoutRepo.Update(workout);
                        }
                    }
                    Workouts[index-1].IsCurrent = w.IsCurrent;
                    await Database.WorkoutRepo.Update(Workouts[index-1]);
                    w.IsCurrent--;
                    await Database.WorkoutRepo.Update(w);
                    Workouts.Move(index, index - 1);
                }
                MoveWorkoutUpCommand.ChangeCanExecute();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        bool CanMoveWorkoutUp(object para)
        {
            Workout w = (Workout)para;
            int index = Workouts.IndexOf(w);
            if (index > 0) return true;
            return false;
        }
    }
}
