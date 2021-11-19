using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PED_Gen_2_Debug_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : TabbedPage
    {
        public HomePage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ViewModels.ViewModel_ControlPage, string>(this, "Exception", async (sender, arg) =>
            {
                await DisplayAlert("Exception", arg, "OK");
            });
        }
    }
}