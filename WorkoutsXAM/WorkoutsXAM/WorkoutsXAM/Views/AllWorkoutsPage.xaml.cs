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
    public partial class AllWorkoutsPage : ContentPage
    {
        AllWorkoutsViewModel viewModel;

        public AllWorkoutsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AllWorkoutsViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.IsBusy = true;
        }
    }
}