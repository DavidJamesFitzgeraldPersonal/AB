using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace PED_Gen_2_Debug_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.ViewModel_MainPage(Navigation);

            MessagingCenter.Subscribe<ViewModels.ViewModel_MainPage, string>(this, "Error", async (sender, arg) =>
            {
                await DisplayAlert("Error", arg, "OK");
            });
        }
    }
}
