using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;
using WorkoutsXAM.Views;
using Xamarin.Forms;

namespace WorkoutsXAM.ViewModels
{
    public class WorkoutDetailViewModel : BaseViewModel
    {
        private Workout workout;
        public Workout Workout
        {
            get { return workout; }
            set { SetProperty(ref workout, value); }
        }

        private string workoutName;
        public string WorkoutName
        {
            get { return workoutName; }
            set
            {
                if (SetProperty(ref workoutName, value))
                {
                    Workout.Name = WorkoutName;
                    CheckValidity();
                }
            }
        }

        public Command SaveNewWorkoutCommand { get; set; }
        public Command UpdateWorkoutCommand { get; set; } 
        public Command DeleteWorkoutCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command RemoveExerciseCommand { get; set; }
        public Command AddExerciseCommand { get; set; }
        public Command RefreshWorkoutCommand { get; set; }

        private INavigation navigation;

        private bool isNew = false;

        public WorkoutDetailViewModel(Workout workout)
        {
            InitCommands();

            Title = workout?.Name;
            Workout = workout;
            WorkoutName = Workout?.Name;
        }

        public WorkoutDetailViewModel(INavigation nav)
        {
            InitCommands();

            Workout = new Workout();
            WorkoutName = "New Workout";
            Workout.Details = "New Workout Details";
            Workout.Exercises = new List<Exercise>();
            isNew = true;

            Title = Workout.Name;

            navigation = nav;
        }

        private void InitCommands()
        {
            SaveNewWorkoutCommand = new Command(async () => await SaveNewWorkout(), WorkoutNameValidation);
            UpdateWorkoutCommand = new Command(async () => await UpdateWorkout(), WorkoutNameValidation);
            DeleteWorkoutCommand = new Command(async () => await DeleteWorkout());
            CancelCommand = new Command(async () => await Cancel());
            RemoveExerciseCommand = new Command<object>(async (x) => await RemoveExercise(x));
            AddExerciseCommand = new Command(async () => await AddExercise());
            RefreshWorkoutCommand = new Command(async () => await RefreshWorkout());
        }

        public void SetNavigation(INavigation nav)
        {
            navigation = nav;
        }

        private void CheckValidity()
        {
            ((Command)SaveNewWorkoutCommand).ChangeCanExecute();
            ((Command)UpdateWorkoutCommand).ChangeCanExecute();
        }

        async Task SaveNewWorkout()
        {
            await Database.WorkoutRepo.Insert(Workout);
            await navigation.PopModalAsync();
        }

        async Task UpdateWorkout()
        {
            await Database.WorkoutRepo.Update(Workout);
            await navigation.PopModalAsync();
        }

        async Task DeleteWorkout()
        {
            if(!await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete " + Workout.Name + " ?", "Yes", "No")) return;
            await Database.WorkoutRepo.Delete(Workout);
            await navigation.PopModalAsync();
        }

        async Task Cancel()
        {
            await navigation.PopModalAsync();
        }

        async Task RemoveExercise(object exercise)
        {
            try
            {
                Exercise ex = (Exercise)exercise;
                Workout.Exercises.Remove(ex);
                await Database.WorkoutRepo.Update(Workout);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                await RefreshWorkout();
            }
        }

        async Task AddExercise()
        {
            try
            {
                if (isNew) await Database.WorkoutRepo.Insert(Workout);
                await navigation.PushModalAsync(new NavigationPage(new AddExercisePage(Workout)));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        async Task RefreshWorkout()
        {
            Workout w = await Database.WorkoutRepo.Get(Workout.Id);
            if (w != null) Workout = w;

            if(isNew) 
            {
                await Database.WorkoutRepo.Delete(Workout); 
            }

            OnPropertyChanged("Workout");
        }

        bool WorkoutNameValidation()
        {
            if (WorkoutName.Length > 0) return true;
            return false;
        }
    }
}
