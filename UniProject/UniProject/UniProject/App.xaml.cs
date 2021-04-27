using Xamarin.Forms;
namespace UniProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts

            // Start the fingerprint authenticator in the background.
            ViewModels.VM_UserAuthentication.CheckAuthenticationIsAvailable();

            // Check the permissions the user has allowed. 
            ViewModels.VM_Permissions.CheckAndRequestAllPermissions();
        }
    }
}