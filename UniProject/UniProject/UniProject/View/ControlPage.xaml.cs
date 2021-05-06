using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : ContentPage
    {       
        public ControlPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.ViewModel_ControlPage();
        }
    }
}