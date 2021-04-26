using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace UniProject.ViewModels
{
    // The following dangerous permissions are required, which must be specifically checked for at run time.
    // Android
    //< uses - permission android: name = "android.permission.ACCESS_COARSE_LOCATION" />
    //< uses - permission android: name = "android.permission.ACCESS_FINE_LOCATION" />
    public static class VM_Permissions
    {
        #region Static Methods
        public static async void CheckAndRequestAllPermissions()
        {
            await CheckAndRequestLocationPermission();
        }

        public static async Task<bool> CheckAndRequestLocationPermission()
        {
            bool hasPermission = false;
            PermissionStatus status = PermissionStatus.Unknown;

            try
            {
                status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status == PermissionStatus.Granted)
                {
                    hasPermission = true;
                }
            }
            catch (Exception ex)
            {
                //TODO handle exceptions
            }

            return hasPermission;
        }
        #endregion
    }
}
