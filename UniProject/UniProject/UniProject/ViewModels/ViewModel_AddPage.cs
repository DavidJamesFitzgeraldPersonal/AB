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
        private Models.Model_BleConnection _selectedDevice;
        public Models.Model_BleConnection _SelectedDevice
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

        private bool _allowScan;
        public bool _AllowScan
        {
            get { return _allowScan; }
            set
            {
                if (value == _allowScan)
                    return;
                _allowScan = value;
                OnPropertyChanged();

                if (_AllowScan)
                {
                    _ScanState = "Scan";
                }
                else
                {
                    _ScanState = "Scanning...";
                }
            }
        }

        private string _scanState = "";
        public string _ScanState
        {
            get { return _scanState; }
            set
            {
                if (value == _scanState)
                    return;
                _scanState = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand _DeviceSelected { get; }
        private void DoDeviceSelected()
        {
            // connect to device
            if()
        }

        public ICommand _ScanSelected { get; }
        private void DoScanSelected()
        {
            _AllowScan = false;
            _AvailableBleDevices.Clear();
            DoBLEScan();
        }
        #endregion

        #region Constructor
        public ViewModel_AddPage()
        {
            _AvailableBleDevices = new ObservableCollection<Models.Model_BleConnection>();

            // Bind Commands
            _DeviceSelected = new Command(DoDeviceSelected);
            _ScanSelected = new Command(DoScanSelected);

            // Start a scan on creation for lulz
            DoScanSelected();
        }
        #endregion

        public async void DoBLEScan()
        {
            try
            {
                Plugin.BLE.Abstractions.Contracts.IAdapter bleHW = CrossBluetoothLE.Current.Adapter;

                if (false == bleHW.IsScanning)
                {
                    bleHW.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowLatency;
                    bleHW.ScanTimeout = 5000;
                    bleHW.DeviceDiscovered += (sender, events) =>
                    {
                        _AvailableBleDevices.Add(new Models.Model_BleConnection
                                                (events.Device.Id.ToString(),
                                                events.Device.Name,
                                                events.Device));   
                    };
                    bleHW.ScanTimeoutElapsed += (sender, events) =>
                    {
                        _AllowScan = true;
                        bleHW.StopScanningForDevicesAsync();
                    };

                    _AllowScan = false;
                    await bleHW.StartScanningForDevicesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}