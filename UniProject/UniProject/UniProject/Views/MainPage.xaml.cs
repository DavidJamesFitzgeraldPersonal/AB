using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace UniProject
{
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
                if (false == ViewModels.VM_UserAuthentication._FingerPrintAuthenticationUsed)
                {
                    await DisplayAlert("ERROR!", "TODO: The device does not make use of finger print authentication","OK");
                }
                else
                {                                
                    proceed = await ViewModels.VM_UserAuthentication.CheckFingerPrintIsAuthenticated();                 
                }
            }

            if (proceed)
            {
                await Navigation.PushAsync(new ControlPage());
            }
        }
    }
}
