using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
namespace UniProject.ViewModels
{
    public static class VM_UserAuthentication
    {
        private static bool userAuthenticated = false;
        public static bool _UserAuthenticated { get => userAuthenticated; set => userAuthenticated = value; }
        
        private static bool fingerPrintAuthenticationUsed = false;
        public static bool _FingerPrintAuthenticationUsed { get => fingerPrintAuthenticationUsed; set => fingerPrintAuthenticationUsed = value; }


        // Checks which type of authentication is available. If finger print is available, the app will check the user is authenticated via a background task.
        public static async void CheckAuthenticationIsAvailable()
        {
            AuthenticationType type = await CrossFingerprint.Current.GetAuthenticationTypeAsync();

            switch (type)
            {
                case AuthenticationType.Fingerprint:
                    fingerPrintAuthenticationUsed = true;
                    break;
                case AuthenticationType.Face:
                default:
                    fingerPrintAuthenticationUsed = false;
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
            userAuthenticated = authenticated;
            return userAuthenticated;
        }
    }
}
