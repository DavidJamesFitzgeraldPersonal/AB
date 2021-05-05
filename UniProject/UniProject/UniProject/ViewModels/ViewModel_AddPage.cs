using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Exceptions;

namespace UniProject.ViewModels
{
    public class ViewModel_AddPage : BindableObject
    {
        private Plugin.BLE.Abstractions.Contracts.IAdapter _bleHW;

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

        private ObservableCollection<Models.Model_BleConnection> _connectedBleDevices;
        public ObservableCollection<Models.Model_BleConnection> _ConnectedBleDevices
        {
            get { return _connectedBleDevices; }
            set
            {
                if (value == _connectedBleDevices)
                    return;
                _connectedBleDevices = value;
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
        private async void DoDeviceSelected()
        {
            await _bleHW.StopScanningForDevicesAsync();

            if (false == _bleHW.IsScanning)
            {
                // connect to device
                try
                {
                    _bleHW.DeviceConnected += (sender, events) =>
                    {
                        _ConnectedBleDevices = new ObservableCollection<Models.Model_BleConnection>();
                        _ConnectedBleDevices.Add(new Models.Model_BleConnection(events.Device));
                    };
                    await _bleHW.ConnectToDeviceAsync(_SelectedDevice._Dev);
                }
                catch(DeviceConnectionException ex)
                {
                    int i = 0;
                }
                catch (Exception e)
                {
                    // ... could not connect to device
                    int i = 0;
                }
            }
        }

        public ICommand _ScanSelected { get; }
        private void DoScanSelected()
        {
            _AvailableBleDevices = new ObservableCollection<Models.Model_BleConnection>();
            DoBLEScan();
        }
        #endregion

        #region Constructor
        public ViewModel_AddPage()
        {
            // Bind Commands
            _DeviceSelected = new Command(DoDeviceSelected);
            _ScanSelected = new Command(DoScanSelected);

            _bleHW = CrossBluetoothLE.Current.Adapter;

            // Start a scan on creation for lulz
            DoScanSelected();
        }
        #endregion

        public async void DoBLEScan()
        {
            try
            {
                var systemDevices = _bleHW.GetSystemConnectedOrPairedDevices();

                if (false == _bleHW.IsScanning)
                {
                    _AllowScan = false;

                    _bleHW.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowLatency;
                    _bleHW.ScanTimeout = 5000;
                    _bleHW.DeviceDiscovered += (sender, events) =>
                    {
                        if ("SH-HC-08" == events.Device.Name)
                        {
                            _AvailableBleDevices.Add(new Models.Model_BleConnection(events.Device));
                        }
                    };
                    _bleHW.ScanTimeoutElapsed += (sender, events) =>
                    {
                        _AllowScan = true;
                        _bleHW.StopScanningForDevicesAsync();
                    };

                    _AllowScan = false;
                    await _bleHW.StartScanningForDevicesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}