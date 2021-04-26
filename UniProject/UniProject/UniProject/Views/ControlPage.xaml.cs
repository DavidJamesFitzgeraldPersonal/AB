using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : TabbedPage
    {
        //private readonly IAdapter _bluetoothAdapter;
        
        public ControlPage()
        {
            InitializeComponent();
            BindingContext = new ViewModel_ControlPage();
            /*
            _bluetoothAdapter = CrossBluetoothLE.Current.Adapter;
            _bluetoothAdapter.DeviceDiscovered += (sender, foundBleDevice) =>
            {
                if (foundBleDevice.Device != null && !string.IsNullOrEmpty(foundBleDevice.Device.Name))
                {

                }
            };
            */

        }
    }
}