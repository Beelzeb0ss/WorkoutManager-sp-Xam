using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutsXAM.Models;
using WorkoutsXAM.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkoutsXAM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddExercisePage : ContentPage
    {
        public AddExercisePage(Workout w)
        {
            InitializeComponent();
            BindingContext = new AddExerciseViewModel(Navigation, w);
        }
    }
}