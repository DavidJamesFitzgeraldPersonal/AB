using System;
using System.Windows.Input;
using Xamarin.Forms;
namespace UniProject.ViewModels
{
    public class ViewModel_MainPage : BindableObject
    {
        #region Privates
        private INavigation _navigation;
        #endregion
        #region Commands
        public ICommand _LoginClicked { get; }
        private async void DoLoginClicked()
        {
            try
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
                            MessagingCenter.Send(this, "Error", "Ensure at least one finger print is enrolled for user identification.");
                        }
                        else
                        {
                            MessagingCenter.Send(this, "Error", "This device does not support finger print identifaction.");
                        }
                    }
                }

                if (proceed)
                {
                    await _navigation.PushAsync(new HomePage());
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        public ViewModel_MainPage(INavigation nav)
        {
            _navigation = nav;
            _LoginClicked = new Command(DoLoginClicked);
        }
    }

}
