using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
namespace UniProject.ViewModels
{
    public class ViewModel_ControlPage : BindableObject
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

        private ObservableCollection<Models.Model_Device> _connectedDevices;
        public ObservableCollection<Models.Model_Device> _ConnectedDevices
        {
            get { return _connectedDevices; }
            set
            {
                if (value == _connectedDevices)
                    return;
                _connectedDevices = value;
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
            _ConnectedDevices = new ObservableCollection<Models.Model_Device>();

            // Bind Commands
            _DeviceSelected = new Command(DoDeviceSelected);

        }
        #endregion
    }
}
