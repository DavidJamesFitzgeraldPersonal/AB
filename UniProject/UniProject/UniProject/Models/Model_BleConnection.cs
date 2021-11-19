using Xamarin.Forms;

namespace PED_Gen_2_Debug_App.Models
{
    public class Model_BleConnection : BindableObject
    {
        #region Properties
        public Plugin.BLE.Abstractions.Contracts.IDevice _Dev { get; set; }

        public bool _Old = false;

        #endregion

        #region Constructor 
        public Model_BleConnection(Plugin.BLE.Abstractions.Contracts.IDevice dev)
        {
            _Dev = dev;
        }
        #endregion
    }
}
