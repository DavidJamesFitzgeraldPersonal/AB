using Xamarin.Forms;

namespace UniProject.Models
{ 
    public class Model_Device : BindableObject
    {
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

        private bool _isLocked;
        public bool _IsLocked
        {
            get { return _isLocked; }
            set
            {
                if (value == _isLocked)
                    return;
                _isLocked = value;
                if (_isLocked)
                {
                    _Image = "baseline_lock_black_36.png";
                }
                else
                {
                    _Image = "baseline_lock_open_black_36.png";
                }
                // Must notify of change to state
                OnPropertyChanged();
            }

        }
        #endregion

        #region Constructor
        public Model_Device(string name, bool initial)
        {
            _Name = name;
            _IsLocked = initial;
        }
        #endregion
    }
}

