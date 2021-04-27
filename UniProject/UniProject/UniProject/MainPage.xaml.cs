using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            bool proceed = false;
            if (ViewModels.VM_UserAuthentication._UserAuthenticated)
            {
                // Background authentication checks have already been performed and the user is authenticated
                proceed = true;
            }
            else
            {
                if (ViewModels.VM_UserAuthentication._FingerPrintAuthenticationUsed)
                {
                    proceed = await ViewModels.VM_UserAuthentication.CheckFingerPrintIsAuthenticated();
                }
                else
                {
                    if (ViewModels.VM_UserAuthentication._FingerPrintAuthenticationPossible)
                    {
                        await DisplayAlert("ERROR!", "Ensure at least one finger print is enrolled for user identification.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("ERROR!", "This device does not support finger print identifaction.", "OK");
                    }
                }
            }

            if (proceed)
            {
                await Navigation.PushAsync(new HomePage());
            }
        }
    }
}
