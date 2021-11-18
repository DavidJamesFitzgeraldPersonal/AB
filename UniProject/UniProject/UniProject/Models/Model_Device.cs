using Xamarin.Forms;

namespace UniProject.Models
{ 
    public class Model_Device : BindableObject
    {
        #region Enums
        public enum DataSource
        {
            UNKNOWN,
            FAKE,
            REAL,
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

        private byte[] _dataToSend;
        public byte[] _DataToSend;

        private byte[] _response;
        public byte[] _Response;

        private DataSource _dataSource;
        public DataSource _DataSource
        {
            get { return _dataSource; }
            set
            {
                if (value == _dataSource)
                    return;
                _dataSource = value;
                if (DataSource.FAKE == _dataSource)
                {
                    _Image = "baseline_lock_black_36.png";
                }
                else if(DataSource.REAL == _dataSource)
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

        private DataSource _commandedState = DataSource.NONE;
        public DataSource _CommandedState { get; set; }

        public Model_BleConnection _Connection;

        #endregion

        #region Constructor
        public Model_Device(string name)
        {
            _Name = name;
            _dataSource = DataSource.UNKNOWN;
            _image = "baseline_help_center_black_36.png";
            _CommandedState = _dataSource;

            _DataSource = _dataSource;
            _Image = _image;
        }
        #endregion
    }
}

