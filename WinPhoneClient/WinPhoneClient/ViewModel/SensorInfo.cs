using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using WinPhoneClient.Enums;
using WinPhoneClient.JSON;
using WinPhoneClient.Model;

namespace WinPhoneClient.ViewModel
{
    public class SensorInfo : ObservableObject, IJsonDependent
    {
        #region Fields
        private int _droneId;
        private string _sensorName;
        private ObservableCollection<SensorValueJson> _values = new ObservableCollection<SensorValueJson>();
        #endregion
        #region Properties
        public int SensorId { get; }

        public int DroneId
        {
            get { return _droneId; }
            set { Set(ref _droneId, value); }
        }

        public string SensorName
        {
            get { return _sensorName; }
            set { Set(ref _sensorName, value); }
        }

        public ObservableCollection<SensorValueJson> Values
        {
            get { return _values; }
            set { Set(ref _values, value); }
        }

        #endregion
        #region Constructor
        public SensorInfo(int sensorId, int droneId)
        {
            SensorId = sensorId;
            _droneId = droneId;
        }
        #endregion
        #region Public methods

        public void AddValues(IEnumerable<SensorValueJson> values)
        {
            var sensorValueJsons = values as SensorValueJson[] ?? values.ToArray();
            if (sensorValueJsons.Any())
            {
                foreach (var sensorValueJson in sensorValueJsons)
                    Values.Add(sensorValueJson);
            }
        }

        public void ApplyJson(IBaseJsonValue json)
        {
            try
            {
                var sensorJson = json as SensorJson;
                if (sensorJson != null)
                {
                    DroneId = (int) sensorJson.DroneId;
                    SensorName = sensorJson.Name;
                }
            }
            catch (Exception)
            {
            }
        }

        public IBaseJsonValue GetJsonValue()
        {
            var json = new SensorJson();
            json.Json = json.CreateEmptyJsonObject();
            json.DroneId = DroneId;
            json.Name = SensorName;
            json.Id = SensorId;
            return json;
        }
        #endregion
    }
}
