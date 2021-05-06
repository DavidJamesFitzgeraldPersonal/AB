using Plugin.BLE;
using System.Collections.Generic;
namespace UniProject.ViewModels
{
    public static class ViewModel_BLE
    {
        private static List<Models.Model_BleConnection> _BleDevices = new List<Models.Model_BleConnection>();

        public static async void dosomethingwithble()
        {
            Plugin.BLE.Abstractions.Contracts.IBluetoothLE bleInterface = CrossBluetoothLE.Current;
            
            Plugin.BLE.Abstractions.Contracts.IAdapter bleHW = CrossBluetoothLE.Current.Adapter;

            bleHW.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowLatency;
            bleHW.ScanTimeout = 5000;
            bleHW.DeviceDiscovered += BleHW_DeviceDiscovered;
            /*
            ble.StateChanged += (s, e) =>
            {
                i++;
                this.display.Text = "The bluetooth state changed, iteration  " + i;
                Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
            };
            */

            /*
            adapter.DeviceAdvertised += (s, a) =>
            {
                Debug.WriteLine("Device advertised: " + a.Device);
            };
            */
            await bleHW.StartScanningForDevicesAsync();
        }

        #region Events
        private static void BleHW_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            Models.Model_BleConnection newDevice = new Models.Model_BleConnection(e.Device);
            _BleDevices.Add(newDevice);
        }
        #endregion
    }
}
