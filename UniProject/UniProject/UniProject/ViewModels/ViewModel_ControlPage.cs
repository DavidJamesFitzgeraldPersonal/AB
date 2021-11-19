using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Exceptions;
namespace PED_Gen_2_Debug_App.ViewModels
{
    public class ViewModel_ControlPage : BindableObject
    {
        #region Privates
        private static System.Timers.Timer _backgroundTimer = new System.Timers.Timer(2000);

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
                    if (Models.Model_Device.DataSource.FAKE == _SelectedDevice._DataSource)
                    {
                        _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;
                    }
                    if (Models.Model_Device.DataSource.REAL == _SelectedDevice._DataSource)
                    {
                        _SelectedDevice._CommandedState = Models.Model_Device.DataSource.FAKE;
                    }
                    if (Models.Model_Device.DataSource.UNKNOWN == _SelectedDevice._DataSource)
                    {
                        _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;
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
            _bleHW.ScanTimeout = 1000;

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
            /*
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
            */
            if (false == _bleHW.IsScanning)
            {
                try
                {
                    _InfoString = "Scanning.";
                    _bleHW.StartScanningForDevicesAsync();
                }
                catch (Exception ex)
                {
                    MessagingCenter.Send(this, "Exception", ex.Message);
                }
                _backgroundTimer.Start();
            }
        }
        private void DoScanTimeout()
        {
            if (false == _bleHW.IsScanning)
            {
                _bleHW.StopScanningForDevicesAsync();

                _InfoString = "";
                
                int k = _Devices.Count;
                if (0 == k)
                {
                    _InfoString = "No devices found.";
                }
                else
                {
                    _InfoString = "Devices found. Cleaning list.";

                    List<string> names = new List<string>();

                    for (int i = 0; i < k; i++)
                    {
                        if (_Devices[i]._Connection._Old && _Devices[i] != null)
                        {
                            _Devices.RemoveAt(i);
                        }
                        else 
                        {
                            if(names.Contains(_Devices[i]._Name))
                            {
                                _Devices.RemoveAt(i);
                            }
                            else
                            { names.Add(_Devices[i]._Name); }
                        }
                    }

                    _InfoString = "Updating Devices.";
                    DoStateUpdates();
                    _InfoString = "";
                }
            }
        }
        private void DoDeviceFound(Plugin.BLE.Abstractions.Contracts.IDevice foundDev)
        {
            if (foundDev.Name != null)
            {
                if (foundDev.Name.Contains("Tracerco-PED") && foundDev.Name != "")
                {
                    bool found = false;

                    foreach (Models.Model_Device existingDev in _devices)
                    {
                        if (foundDev.Id == existingDev._Connection._Dev.Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (false == found)
                    {
                        try
                        {
                            _bleHW.ConnectToDeviceAsync(foundDev);
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
        }
        private async void DoDeviceConnected(Plugin.BLE.Abstractions.Contracts.IDevice foundDev)
        {
            Models.Model_Device Dev = new Models.Model_Device(foundDev.Name);
            Dev._Connection = new Models.Model_BleConnection(foundDev);
            bool addDev = true;

            int k = _Devices.Count;
            for(int i=0; i < k; i++)
            {
                if(_Devices[i]._Name == Dev._Name)
                {
                    addDev = false;
                    break;
                }
            }

            if (addDev)
            {
                _Devices.Add(Dev);
            }
        }
        private void DoStateUpdates()
        {
            foreach (Models.Model_Device existingDev in _Devices)
            {
                DoUpdate(existingDev);
            }
        }
        bool sent = false;
        private void DoUpdate(Models.Model_Device thisDev)
        {
            if (false == _bleHW.IsScanning)
            {
                try
                {
                    if (thisDev._DataSource != thisDev._CommandedState && thisDev._CommandedState != Models.Model_Device.DataSource.NONE)
                    {
                        byte[] Data;
                        switch (thisDev._CommandedState)
                        {
                            case Models.Model_Device.DataSource.FAKE:
                                // Send command for 1sv/h
                                Data = new byte[12];
                                Data[0] = 0x55;
                                Data[1] = 0x55;
                                Data[2] = 0xDF;
                                Data[3] = 0x00;
                                Data[4] = 0x01;
                                Data[5] = 0x05;
                                Data[6] = 0x05;
                                Data[7] = 0xF5;
                                Data[8] = 0xE1;
                                Data[9] = 0x00;
                                Data[10] = 0x45;
                                Data[11] = 0x04;
 
                                break;

                            case Models.Model_Device.DataSource.REAL:
                            default:
                                // Send command to remove debug mode
                                Data = new byte[11];
                                Data[0] = 0x55;
                                Data[1] = 0x55;
                                Data[2] = 0xDF;
                                Data[3] = 0x00;
                                Data[4] = 0x00;
                                Data[5] = 0x00;
                                Data[6] = 0x00;
                                Data[7] = 0x00;
                                Data[8] = 0x00;
                                Data[9] = 0x21;
                                Data[10] = 0x04;
                               
                                break;
                        }
                        thisDev._DataToSend = Data;
                        if (!sent)
                        {
                            //sent = true;
                            DoDataTransfer(thisDev);
                        }
                    }
                    else
                    {
                        // Ensure we dont continually set the lock state
                        thisDev._CommandedState = Models.Model_Device.DataSource.NONE;
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

        private async void DoDataTransfer(Models.Model_Device connectedDev)
        {
            if (false == _bleHW.IsScanning)
            {
                /*
                 * Proprietary Service UUID: 49535343-FE7D-4AE5-8FA9-9FAFD205E455
                 * UART TX UUID: 49535343-1E4D-4BD9-BA61-23C647249616 notify write write-no-response
                 * UART RX UUID: 49535343-8841-43F4-A8D4-ECBE34729BB3 write write with-no-response
                 */
                try
                {
                    if (false == connectedDev._ExpectingResponse)
                    {
                        connectedDev.Send();
                    }
                }
                catch (Exception ex)
                {
                    MessagingCenter.Send(this, "Exception", ex.Message);
                }
            }
        }
        #endregion
    }
}