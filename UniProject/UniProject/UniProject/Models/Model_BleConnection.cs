using Xamarin.Forms;

namespace UniProject.Models
{
    public class Model_BleConnection : BindableObject
    {
        #region Properties
        public Plugin.BLE.Abstractions.Contracts.IDevice _Dev { get; set; }

        private string _id;
        public string _Id
        {
            get { return _id; } 
            set
            {
                if (value == _id)
                    return;
                _id = value;
                OnPropertyChanged();
            }
        }
        private string _name;
        public string _Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                    return;
                _name = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor 
        public Model_BleConnection(string id, string name, Plugin.BLE.Abstractions.Contracts.IDevice dev)
        {
            _Id = id;
            _Name = name;
            _Dev = dev;
        }
        #endregion
    }
}
