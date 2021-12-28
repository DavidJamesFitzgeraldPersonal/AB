using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PED_Gen_2_Debug_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedDevicePage : ContentPage
    {
        private ViewModels.ViewModel_SelectedDevicePage _selectedDeviceVM;
        public SelectedDevicePage(Models.Model_Device selectedDev)
        {
            InitializeComponent();
            BindingContext = new ViewModels.ViewModel_SelectedDevicePage(Navigation, selectedDev);
            MessagingCenter.Subscribe<ViewModels.ViewModel_SelectedDevicePage, string>(this, "Exception", async (sender, arg) =>
            {
                await DisplayAlert("Exception", arg, "OK");
            });

            remoteControlSwitch.IsToggled = selectedDev._IsFakeData;

            _selectedDeviceVM = (ViewModels.ViewModel_SelectedDevicePage)BindingContext;
        }

        private void remoteControlSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if(true == e.Value)
            {
                if(null != _selectedDeviceVM)
                {
                    _selectedDeviceVM.AttemptRemoteControlEnable();
                }
            }
            else
            {
                if (null != _selectedDeviceVM)
                {
                    _selectedDeviceVM.AttemptRemoteControlDisable();
                }
            }
        }
    }
}