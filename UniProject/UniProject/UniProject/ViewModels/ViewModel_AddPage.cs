using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE;
namespace UniProject.ViewModels
{
    public class ViewModel_AddPage : BindableObject
    {
        #region Properties
        private Models.Model_Device _selectedDevice = null;
        public Models.Model_Device _SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                if (value == _selectedDevice)
                    return;
                _selectedDevice = value;
                OnPropertyChanged();

                // Catch the change in item
                DoDeviceSelected();

                // Set to null to deselect the device
                _selectedDevice = null;
            }
        }

        private ObservableCollection<Models.Model_BleConnection> _availableBleDevices;
        public ObservableCollection<Models.Model_BleConnection> _AvailableBleDevices
        {
            get { return _availableBleDevices; }
            set
            {
                if (value == _availableBleDevices)
                    return;
                _availableBleDevices = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand _DeviceSelected { get; }
        private void DoDeviceSelected()
        {
            // connect to device
        }
        #endregion

        #region Constructor
        public ViewModel_AddPage()
        {
            _AvailableBleDevices = new ObservableCollection<Models.Model_BleConnection>();

            // Bind Commands
            _DeviceSelected = new Command(DoDeviceSelected);

            DoBLEScan();
        }
        #endregion

        public async void DoBLEScan()
        {
            try
            {
                Plugin.BLE.Abstractions.Contracts.IAdapter bleHW = CrossBluetoothLE.Current.Adapter;

                bleHW.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowLatency;
                bleHW.ScanTimeout = 5000;
                bleHW.DeviceDiscovered += (sender, foundDev) =>
                {
                    _AvailableBleDevices.Add(new Models.Model_BleConnection
                                            (foundDev.Device.Id.ToString(),
                                            foundDev.Device.Name,
                                            foundDev.Device));
                };
                await bleHW.StartScanningForDevicesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}