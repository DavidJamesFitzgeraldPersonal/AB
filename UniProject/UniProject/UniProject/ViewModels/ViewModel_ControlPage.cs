using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Exceptions;
namespace UniProject.ViewModels
{
    public class ViewModel_ControlPage : BindableObject
    {
        #region Privates
        private static System.Timers.Timer _backgroundTimer = new System.Timers.Timer(200);

        private static Plugin.BLE.Abstractions.Contracts.IAdapter _bleHW;
        #endregion

        #region Properties
        private Models.Model_Device _selectedDevice;
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
            }
        }

        private ObservableCollection<Models.Model_Device> _devices;
        public ObservableCollection<Models.Model_Device> _Devices
        {
            get { return _devices; }
            set
            {
                if (value == _devices)
                    return;
                _devices = value;
                OnPropertyChanged();
            }
        }

        private string _scanningString = "";
        public string _ScanningString
        {
            get { return _scanningString; }
            set
            {
                if (value == _scanningString)
                    return;
                _scanningString = value;
                OnPropertyChanged();
            }
        }

        private string _infoString = "";
        public string _InfoString
        {
            get { return _infoString; }
            set
            {
                if (value == _infoString)
                    return;
                _infoString = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand _DeviceSelected { get; }
        private void DoDeviceSelected()
        {
            if (null != _SelectedDevice)
            {
                try
                {
                    if (Models.Model_Device.LockState.LOCKED == _SelectedDevice._LockState)
                    {
                        _SelectedDevice._CommandedState = Models.Model_Device.LockState.UNLOCKED;
                    }
                    if (Models.Model_Device.LockState.UNLOCKED == _SelectedDevice._LockState)
                    {
                        _SelectedDevice._CommandedState = Models.Model_Device.LockState.LOCKED;
                    }
                    if (Models.Model_Device.LockState.UNKNOWN == _SelectedDevice._LockState)
                    {
                        _SelectedDevice._CommandedState = Models.Model_Device.LockState.LOCKED;
                    }
                    _SelectedDevice = null;
                }
                catch (Exception e)
                {
                    // ... could not connect to device
                    int i = 0;
                }
            }
        }
        #endregion

        #region Events
        private void _backgroundTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _backgroundTimer.Stop();
            DoBLEScan();
        }
        #endregion

        #region Constructor
        public ViewModel_ControlPage()
        {
            // Bind Commands
            _DeviceSelected = new Command(DoDeviceSelected);

            _Devices = new ObservableCollection<Models.Model_Device>();

            _bleHW = CrossBluetoothLE.Current.Adapter;
            _bleHW.ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode.LowLatency;
            _bleHW.ScanTimeout = 100;
            #region Callbacks
            _bleHW.DeviceDiscovered += (sender, events) =>
            {
                DoDeviceFound(events.Device);
            };
            _bleHW.ScanTimeoutElapsed += (sender, events) =>
            {
                DoScanTimeout();
            };
            _bleHW.DeviceConnected += (sender, events) =>
            {
                DoDeviceConnected(events.Device);
            };
            _bleHW.DeviceDisconnected += (sender, events) =>
            {
                foreach (Models.Model_Device existingDev in _devices)
                {
                    if (existingDev._Connection._Dev.Id == events.Device.Id)
                    {
                        existingDev._Connection._Old = true;
                    }
                }
            };
            _bleHW.DeviceConnectionLost += (sender, events) =>
            {
                foreach (Models.Model_Device existingDev in _devices)
                {
                    if (existingDev._Connection._Dev.Id == events.Device.Id)
                    {
                        existingDev._Connection._Old = true;
                    }
                }
            };
            #endregion
            _backgroundTimer.Elapsed += _backgroundTimer_Elapsed;
            DoBLEScan();
        }
        #endregion

        #region BLE Methods
        private void DoBLEScan()
        {
            try
            {
                byte[] key;
                byte[] cipher = Models.Model_AES.AES_Encrypt("123456", out key);
                byte[] message = Models.Model_AES.AES_Decrypt(cipher, key);
                string plain = System.Text.Encoding.ASCII.GetString(message);
                int i = 0;
            }
            catch(Exception ex)
            {
                int i = 0;
            }
            if (false == _bleHW.IsScanning)
            {
                try
                {
                    _ScanningString = "Scanning...";
                    _bleHW.StartScanningForDevicesAsync();
                }
                catch (Exception ex)
                {
                    MessagingCenter.Send(this, "Exception", ex.Message);
                }
            }
        }
        private void DoScanTimeout()
        {
            if (false == _bleHW.IsScanning)
            {
                _bleHW.StopScanningForDevicesAsync();
                _ScanningString = "";
                
                int k = _Devices.Count;
                if (0 == k)
                {
                    _InfoString = "No devices found. Continuing background scan.";
                }
                else
                {
                    _InfoString = "";
                    for (int i = 0; i < k; i++)
                    {
                        if (_Devices[i]._Connection._Old && _Devices[i] != null)
                        {
                            _Devices.RemoveAt(i);
                        }
                    }
                }

                DoStateUpdates();
            }
        }
        private void DoDeviceFound(Plugin.BLE.Abstractions.Contracts.IDevice founddDev)
        {
            if ("SH-HC-08" == founddDev.Name)
            {
                bool found = false;

                foreach (Models.Model_Device existingDev in _devices)
                {
                    if (founddDev.Id == existingDev._Connection._Dev.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (false == found)
                {
                    try
                    {
                        _bleHW.ConnectToDeviceAsync(founddDev);
                    }
                    catch (DeviceConnectionException ex)
                    {
                        MessagingCenter.Send(this, "Exception", ex.Message);
                    }
                    catch (Exception e)
                    {
                        MessagingCenter.Send(this, "Exception", e.Message);
                    }
                }
            }
        }
        private void DoDeviceConnected(Plugin.BLE.Abstractions.Contracts.IDevice founddDev)
        {
            Models.Model_Device Dev = new Models.Model_Device(founddDev.Name);
            Dev._Connection = new Models.Model_BleConnection(founddDev);
            _Devices.Add(Dev);
        }
        private void DoStateUpdates()
        {
            foreach (Models.Model_Device existingDev in _Devices)
            {
                DoUpdate(existingDev);
            }
            _backgroundTimer.Start();
        }
        private void DoUpdate(Models.Model_Device thisDev)
        {
            if (false == _bleHW.IsScanning)
            {
                try
                {
                    if (thisDev._LockState != thisDev._CommandedState && thisDev._CommandedState != Models.Model_Device.LockState.NONE)
                    {
                        byte[] Data = new byte[1];
                        switch (thisDev._CommandedState)
                        {
                            case Models.Model_Device.LockState.LOCKED:
                                Data[0] = 0x31;
                                break;
                            case Models.Model_Device.LockState.UNLOCKED:
                                Data[0] = 0x30;
                                break;
                            default:
                                // Lock for safety
                                Data[0] = 0x31;
                                break;
                        }
                        DoWriteState(thisDev, Data);
                    }
                    else
                    {
                        // Ensure we dont continually set the lock state
                        thisDev._CommandedState = Models.Model_Device.LockState.NONE;
                    }

                    DoReadState(thisDev);
                }
                catch (DeviceConnectionException ex)
                {
                    MessagingCenter.Send(this, "Exception", ex.Message);
                }
                catch (Exception e)
                {
                    MessagingCenter.Send(this, "Exception", e.Message);
                }
            }
        }
        private async void DoReadState(Models.Model_Device connectedDev)
        {
            if (false == _bleHW.IsScanning)
            {
                try
                {
                    var service = await connectedDev._Connection._Dev.GetServiceAsync(Guid.Parse("0000FFE0-0000-1000-8000-00805F9B34FB"));
                    var characteristic = await service.GetCharacteristicAsync(Guid.Parse("0000FFE1-0000-1000-8000-00805F9B34FB"));
                    var bytes = await characteristic.ReadAsync();

                    if (bytes.Length > 1)
                    {
                        connectedDev._LockState = Models.Model_Device.LockState.UNKNOWN;
                    }
                    else
                    {
                        if (bytes[0] == 0x30)
                        {
                            connectedDev._LockState = Models.Model_Device.LockState.UNLOCKED;
                        }
                        else if (bytes[0] == 0x31)
                        {
                            connectedDev._LockState = Models.Model_Device.LockState.LOCKED;
                        }
                        else
                        {
                            connectedDev._LockState = Models.Model_Device.LockState.UNKNOWN;
                        }
                    }
                }
                catch (DeviceConnectionException ex)
                {
                    MessagingCenter.Send(this, "Exception", ex.Message);
                }
                catch (Exception e)
                {
                    MessagingCenter.Send(this, "Exception", e.Message);
                }
            }
        }
        private async void DoWriteState(Models.Model_Device connectedDev, byte[] data)
        {
            if (false == _bleHW.IsScanning)
            {
                try
                {
                    var service = await connectedDev._Connection._Dev.GetServiceAsync(Guid.Parse("0000FFE0-0000-1000-8000-00805F9B34FB"));
                    var characteristic = await service.GetCharacteristicAsync(Guid.Parse("0000FFE1-0000-1000-8000-00805F9B34FB"));
                    var bytes = await characteristic.WriteAsync(data);
                }
                catch (DeviceConnectionException ex)
                {
                    MessagingCenter.Send(this, "Exception", ex.Message);
                }
                catch (Exception e)
                {
                    MessagingCenter.Send(this, "Exception", e.Message);
                }
            }
        }
        #endregion
    }
}