using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace UniProject
{ 
    public class ViewModel_ControlPage : BindableObject
    {
        #region Properties
        private Model_Device _selectedDevice = null;

        public Model_Device _SelectedDevice
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
                _SelectedDevice = null;
            }
        }

        private ObservableCollection<Model_Device> _devices;
        public ObservableCollection<Model_Device> _Devices
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
        #endregion

        #region Commands
        public ICommand _DeviceSelected { get; }
        private void DoDeviceSelected()
        {
            if (null != _SelectedDevice)
            {
                //TODO just toggle is locked for now
                _SelectedDevice._IsLocked = !_SelectedDevice._IsLocked;
            }
        }
        #endregion

        #region Constructor
        public ViewModel_ControlPage()
        {
              _Devices = new ObservableCollection<Model_Device>();
              _Devices.Add(new Model_Device("front door", "12:34:56:78", true));
              _Devices.Add(new Model_Device("back door", "AB:CD:EF:00", true));

            // Bind Commands
            _DeviceSelected = new Command(DoDeviceSelected);
        }
        #endregion
    }
}
