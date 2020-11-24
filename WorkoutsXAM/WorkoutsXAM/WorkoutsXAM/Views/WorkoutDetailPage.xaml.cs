using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutsXAM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkoutDetailPage : ContentPage
    {
        private WorkoutDetailViewModel vm;

        public WorkoutDetailPage(WorkoutDetailViewModel viewModel)
        {
            InitializeComponent();
            viewModel.SetNavigation(Navigation);
            BindingContext = vm = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.RefreshWorkoutCommand.Execute(null);
        }
    }
}