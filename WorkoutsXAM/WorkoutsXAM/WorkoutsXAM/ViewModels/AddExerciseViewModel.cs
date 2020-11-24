using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;
using Xamarin.Forms;

namespace WorkoutsXAM.ViewModels
{
    class AddExerciseViewModel : BaseViewModel
    {
        public ObservableCollection<Exercise> Exercises { get; set; }

        private string exerciseName = "";
        public string ExerciseName
        {
            get { return exerciseName; }
            set
            {
                if (SetProperty(ref exerciseName, value))
                {
                    LoadExercises();
                    AddNewExerciseCommand?.ChangeCanExecute();
                }
            }
        }

        public Command AddNewExerciseCommand { get; set; }
        public Command AddExerciseCommand { get; set; }
        public Command CancelCommand { get; set; }
        public Command LoadExercisesCommand { get; set; }

        private INavigation navigation;
        private Workout workout;

        public AddExerciseViewModel(INavigation nav, Workout w)
        {
            InitCommands();
            Exercises = new ObservableCollection<Exercise>();

            Title = "Add Exercise";

            navigation = nav;
            workout = w;       
        }

        private void InitCommands()
        {
            AddNewExerciseCommand = new Command(async () => await AddNewExercise(), IsExerciseNameValid);
            AddExerciseCommand = new Command<object>(async (x) => await AddExercise(x));
            CancelCommand = new Command(async () => await Cancel());
            LoadExercisesCommand = new Command(async () => await LoadExercises());
        }

        async Task LoadExercises()
        {
            IsBusy = true;
            try
            {
                Exercises.Clear();
                var items = await Database.ExerciseRepo.Get<Exercise>(e => e.Name.StartsWith(exerciseName));
                foreach (var item in items)
                {
                    Exercises.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }

        }

        async Task AddNewExercise()
        {
            Exercise ex = new Exercise() { Name = ExerciseName };
            workout.Exercises.Add(ex);
            await Database.ExerciseRepo.Insert(ex);
            await Database.WorkoutRepo.Update(workout);
            await navigation.PopModalAsync();
        }

        async Task AddExercise(object element)
        {
            try
            {
                Exercise ex = (Exercise)element;
                workout.Exercises.Add(ex);
                await Database.WorkoutRepo.Update(workout);
                await navigation.PopModalAsync();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        async Task Cancel()
        {
            await navigation.PopModalAsync();
        }

        bool IsExerciseNameValid()
        {
            if (ExerciseName.Length > 0) return true;
            return false;
        }
    }
}
