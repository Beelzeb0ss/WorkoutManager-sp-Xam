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
    public partial class CurrentWorkoutsPage : ContentPage
    {
        CurrentWorkoutsViewModel viewModel;

        public CurrentWorkoutsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CurrentWorkoutsViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.IsBusy = true;
        }
    }
}