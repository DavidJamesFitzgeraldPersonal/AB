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
        #endregion

        #region Constructor
        public ViewModel_SelectedDevicePage(INavigation nav, Models.Model_Device dev)
        {
            _navigation = nav;
            _selectedDevice = dev;
        }
        #endregion

        public void AttemptRemoteControlEnable()
        {
            _InfoString = "Attempting Remote Control...";

            //if (Models.Model_Device.DataSource.FAKE == _SelectedDevice._DataSource)
            //{
            //    _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;
            //}
            //if (Models.Model_Device.DataSource.REAL == _SelectedDevice._DataSource)
            //{
            //    _SelectedDevice._CommandedState = Models.Model_Device.DataSource.FAKE;
            //}
            //if (Models.Model_Device.DataSource.UNKNOWN == _SelectedDevice._DataSource)
            //{
            //    _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;
            //}
            //_SelectedDevice = null;
        }
        public void AttemptRemoteControlDisable()
        {
            _InfoString = "";

            //if (Models.Model_Device.DataSource.FAKE == _SelectedDevice._DataSource)
            //{
            //    _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;
            //}
            //if (Models.Model_Device.DataSource.REAL == _SelectedDevice._DataSource)
            //{
            //    _SelectedDevice._CommandedState = Models.Model_Device.DataSource.FAKE;
            //}
            //if (Models.Model_Device.DataSource.UNKNOWN == _SelectedDevice._DataSource)
            //{
            //    _SelectedDevice._CommandedState = Models.Model_Device.DataSource.REAL;
            //}
            //_SelectedDevice = null;
        }

    }
}
