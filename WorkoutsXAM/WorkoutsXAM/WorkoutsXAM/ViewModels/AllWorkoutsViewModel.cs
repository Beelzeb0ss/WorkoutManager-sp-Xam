using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;
using WorkoutsXAM.Views;
using Xamarin.Forms;

namespace WorkoutsXAM.ViewModels
{
    class AllWorkoutsViewModel : BaseViewModel
    {
        public ObservableCollection<Workout> Workouts { get; set; }

        public Command LoadWorkoutsCommand { get; set; }
        public Command OpenAddWorkoutCommand { get; set; }
        public Command OpenWorkoutDetailsCommand { get; set; }

        private INavigation navigation;

        public AllWorkoutsViewModel(INavigation nav)
        {
            InitCommands();

            navigation = nav;

            Title = "All";
            Workouts = new ObservableCollection<Workout>();        
        }

        private void InitCommands()
        {
            LoadWorkoutsCommand = new Command(async () => await LoadWorkouts());
            OpenAddWorkoutCommand = new Command(async () => await OpenAddWorkoutPage());
            OpenWorkoutDetailsCommand = new Command<object>(async (x) => await OpenWorkoutDetailPage(x));
        }

        protected async virtual Task LoadWorkouts()
        {
            IsBusy = true;
            try
            {
                Workouts.Clear();
                var items = await Database.WorkoutRepo.Get();
                foreach (var item in items)
                {
                    Workouts.Add(item);
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

        async Task OpenAddWorkoutPage()
        {
            try
            {
                await navigation.PushModalAsync(new NavigationPage(new NewWorkoutPage()));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        async Task OpenWorkoutDetailPage(object value)
        {
            try
            {
                Workout w = (Workout)value;
                WorkoutDetailViewModel vm = new WorkoutDetailViewModel(w);
                Debug.WriteLine(navigation == null);
                await navigation.PushModalAsync(new NavigationPage(new WorkoutDetailPage(vm)));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
