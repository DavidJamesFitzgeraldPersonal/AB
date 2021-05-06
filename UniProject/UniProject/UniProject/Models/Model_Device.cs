using Xamarin.Forms;

namespace UniProject.Models
{ 
    public class Model_Device : BindableObject
    {
        #region Enums
        public enum LockState
        {
            UNKNOWN,
            UNLOCKED,
            LOCKED,
            NONE,
        }
        #endregion

        #region Properties
        public string _Name { get; }

        private string _image;
        public string _Image
        {
            get { return _image; }
            set
            {
                if (value == _image)
                    return;
                _image = value;
                // Must notify of change to image
                OnPropertyChanged();
            }
        }

        private LockState _lockState;
        public LockState _LockState
        {
            get { return _lockState; }
            set
            {
                if (value == _lockState)
                    return;
                _lockState = value;
                if (LockState.LOCKED == _lockState)
                {
                    _Image = "baseline_lock_black_36.png";
                }
                else if(LockState.UNLOCKED == _lockState)
                {
                    _Image = "baseline_lock_open_black_36.png";
                }
                else
                {
                    _Image = "baseline_help_center_black_36.png";
                }
                // Must notify of change to state
                OnPropertyChanged();
            }

        }

        private LockState _commandedState = LockState.NONE;
        public LockState _CommandedState { get; set; }

        public Model_BleConnection _Connection;

        #endregion

        #region Constructor
        public Model_Device(string name)
        {
            _Name = name;
            _lockState = LockState.UNKNOWN;
            _CommandedState = _lockState;
        }
        #endregion
    }
}

