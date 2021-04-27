using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Forms;

namespace UniProject.ViewModels
{
    public static class VM_UserAuthentication
    {
        #region Static Properties
        private static bool _userAuthenticated = false;
        public static bool _UserAuthenticated { get => _userAuthenticated; set => _userAuthenticated = value; }

        private static bool _fingerPrintAuthenticationPossible = false;
        public static bool _FingerPrintAuthenticationPossible { get => _fingerPrintAuthenticationPossible; set => _fingerPrintAuthenticationPossible = value; }

        private static bool _fingerPrintAuthenticationUsed = false;
        public static bool _FingerPrintAuthenticationUsed { get => _fingerPrintAuthenticationUsed; set => _fingerPrintAuthenticationUsed = value; }
        #endregion

        #region Static Methods
        // Checks which type of authentication is available. If finger print is available, the app will check the user is authenticated via a background task.
        public static async void CheckAuthenticationIsAvailable()
        {
            FingerprintAvailability available = await CrossFingerprint.Current.GetAvailabilityAsync(false);

            switch (available)
            {
                case FingerprintAvailability.Available:
                    _fingerPrintAuthenticationPossible = true;
                    _fingerPrintAuthenticationUsed = true;
                    break;
                case FingerprintAvailability.NoFingerprint:
                    _fingerPrintAuthenticationPossible = true;
                    _fingerPrintAuthenticationUsed = false;
                    break;
                default:
                    _fingerPrintAuthenticationPossible = false;
                    _fingerPrintAuthenticationUsed = false;
                    break;
            }         
        }
        
        // Called when the user attempts to log-in without previously being authenticated
        public static async Task<bool> CheckFingerPrintIsAuthenticated()
        {
            bool authenticated = false;

            if (await CrossFingerprint.Current.IsAvailableAsync(false))
            {
                AuthenticationRequestConfiguration conf =
                    new AuthenticationRequestConfiguration("Authentication",
                    "Authenticate access to your personal data")
                    {
                        AllowAlternativeAuthentication = true,
                        ConfirmationRequired = false
                    };

                var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
                if (authResult.Authenticated)
                {
                    authenticated = true;
                }
            }

            // Update static
            _userAuthenticated = authenticated;
            return _userAuthenticated;
        }
        #endregion
    }
}
