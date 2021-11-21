using Xamarin.Forms;
using System;
using System.Collections.Generic;

namespace PED_Gen_2_Debug_App.Models
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

        private bool _expectingResponse;
        public bool _ExpectingResponse
        {
            get { return _expectingResponse; }
            set
            {
                if (value == _expectingResponse)
                    return;
                _expectingResponse = value;
            }
        }

        private byte[] _response;
        public byte[] _Response
         {
            get { return _response; }
            set
            {
                if (value == _response)
                    return;
                _response = value;
                CheckForResponse();
            }
        }

        private byte[] _decodedResponse;

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
        public async void Send()
        {
            var service = await _Connection._Dev.GetServiceAsync(Guid.Parse("49535343-FE7D-4AE5-8FA9-9FAFD205E455"));
            var writeCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("49535343-1E4D-4BD9-BA61-23C647249616"));
            var bytesToWrite = await writeCharacteristic.WriteAsync(_DataToSend);

            _ExpectingResponse = true;

            writeCharacteristic.ValueUpdated += (o, args) =>
            {
                _Response = args.Characteristic.Value;
            };

            await writeCharacteristic.StartUpdatesAsync();
        }

        private async void CheckForResponse()
        {
            var service = await _Connection._Dev.GetServiceAsync(Guid.Parse("49535343-FE7D-4AE5-8FA9-9FAFD205E455"));
            var writeCharacteristic = await service.GetCharacteristicAsync(Guid.Parse("49535343-1E4D-4BD9-BA61-23C647249616"));

            List<byte> bytes = new List<byte>(_Response);
            List<byte> decodedBytes = new List<byte>();
            bool escaped = false;
            bool error = false;
            bool store = false;
            bool fullMessage = false;
            int startCharCount = 0;
            byte checksum = 0x00;

            int startsAt = bytes.IndexOf(0x55);

            for (int i = startsAt; (i < _Response.Length) && (!fullMessage) && (!error); i++)
            {
                const int _DOSEVISION_EXPECTED_START_COUNT = 2;
                const int _DOSEVISION_START_CHAR    = 0x55;
                const int _DOSEVISION_BREAK_CHAR    = 0x05;
                const int _DOSEVISION_END_CHAR      = 0x04;

                error = false;
                store = false;

                if (startCharCount < _DOSEVISION_EXPECTED_START_COUNT)
                {
                    if(_DOSEVISION_START_CHAR == bytes[i])
                    {
                        startCharCount++;
                    }
                }
                else
                {
                    if (false == escaped)
                    {
                        if (bytes[i] == _DOSEVISION_START_CHAR)
                        {
                            error = true;
                        }
                        else if (bytes[i] == _DOSEVISION_BREAK_CHAR)
                        {
                            escaped = true;
                        }
                        else if (bytes[i] == _DOSEVISION_END_CHAR)
                        {
                            if(decodedBytes.Count > 1)
                            {
                                fullMessage = true;
                            }
                            else
                            {
                                error = true;
                            }
                        }
                        else
                        {
                            store = true;
                        }
                    }
                    else
                    {
                        escaped = false;
                        store = true;
                    }

                    if (store)
                    {
                        decodedBytes.Add(bytes[i]);
                    }
                }
            }

            if(fullMessage)
            {

            }
            _expectingResponse = false;
        }
    }
}

