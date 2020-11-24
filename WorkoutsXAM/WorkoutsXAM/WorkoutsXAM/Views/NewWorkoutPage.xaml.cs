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
    public partial class NewWorkoutPage : ContentPage
    {
        private WorkoutDetailViewModel viewModel;

        public NewWorkoutPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new WorkoutDetailViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.RefreshWorkoutCommand.Execute(null);
        }
    }
}