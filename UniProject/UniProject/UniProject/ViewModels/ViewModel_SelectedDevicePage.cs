using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;
namespace PED_Gen_2_Debug_App.ViewModels
{
    public class ViewModel_SelectedDevicePage : BindableObject
    {
        #region Privates
        private INavigation _navigation;
        private static System.Timers.Timer _backgroundTimer = new System.Timers.Timer(500);
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

        private string _prevRate = "1.00";
        private string _requestedRate = "1.00";
        public string _RequestedRate
        {
            get { return _requestedRate; }
            set
            {
                if (value == _requestedRate)
                    return;
                _requestedRate = value;
                // Must notify of change to state
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public ViewModel_SelectedDevicePage(INavigation nav, Models.Model_Device dev)
        {
            _navigation = nav;
            _selectedDevice = dev;

            _backgroundTimer.Elapsed += _backgroundTimer_Elapsed;
            _backgroundTimer.Start();
        }
        #endregion

        #region Events
        private async void _backgroundTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _backgroundTimer.Stop();

            if (_SelectedDevice._Connection._Old)
            {
                // Connection lost
                _InfoString = "Connection to device lost! Press the back button to rescan.";
            }
            else
            {
                if(Models.Model_Device.DataSource.FAKE == _SelectedDevice._DataSource)
                {
                    _InfoString = "Device in Remote Control Mode.";

                    if(_prevRate!=_requestedRate)
                    {
                        AttemptRemoteControlEnable();
                    }
                }
                else
                {
                    // Not in FAKE mode
                    if (Models.Model_Device.DataSource.FAKE == _SelectedDevice._CommandedState)
                    {
                        _InfoString = "Attempting Remote Control...";
                    }
                    else 
                    {
                        _InfoString = "";
                    }
                }
            }
            _backgroundTimer.Start();
        }
        #endregion

        public void AttemptRemoteControlEnable()
        {

            _SelectedDevice._CommandedState = Models.Model_Device.DataSource.FAKE;

            if (!_SelectedDevice._HasDataToSend && (null == _SelectedDevice._DataToSend))
            {
                float requested = 0.00f;
                if (float.TryParse(_requestedRate, out requested))
                {
                    UInt32 rate10nSvh = (UInt32)(100000000 * requested);
                    List<byte> Data = new List<byte>();

                    Data.Add(0xDF);
                    Data.Add(0x00);
                    Data.Add(0x01);
                    Data.Add((byte)((rate10nSvh >> 24) & 0xFF));
                    Data.Add((byte)((rate10nSvh >> 16) & 0xFF));
                    Data.Add((byte)((rate10nSvh >> 8) & 0xFF));
                    Data.Add((byte)((rate10nSvh >> 0) & 0xFF));

                    _SelectedDevice._DataToSend = Models.Model_Device.EncodeMessage(Data.ToArray());
                    _SelectedDevice._HasDataToSend = true;
                }
            }
        }
        public void AttemptRemoteControlDisable()
        {

            _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;

            if (!_SelectedDevice._HasDataToSend && (null == _SelectedDevice._DataToSend))
            {
                byte[] Data;
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
                _SelectedDevice._DataToSend = Data;
                _SelectedDevice._HasDataToSend = true;
            }
        }
    }
}
