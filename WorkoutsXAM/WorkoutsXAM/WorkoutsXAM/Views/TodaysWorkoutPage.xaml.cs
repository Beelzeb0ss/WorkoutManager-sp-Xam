using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutsXAM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodaysWorkoutPage : ContentPage
    {
        private TodaysWorkoutViewModel viewModel;
        public TodaysWorkoutPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new TodaysWorkoutViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                viewModel.LoadWorkoutCommand.Execute(null);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}