using System.Collections.Generic;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight;
using WinPhoneClient.Enums;

namespace WinPhoneClient.Model
{
    public class Drone
    {
        #region Constructor
        public Drone(DroneType type)
        {
            DroneType = type;
        }
        #endregion
        #region Properties
        public Geopoint DroneGeopoint { get; set; }

        public DroneType DroneType { get;}

        public string CurrecntTask { get; set; }

        public string Id { get; set; }

        public List<BasicGeoposition> Locations { get; set; } = new List<BasicGeoposition>();

        public double PolutionLevel { get; set; }

        public List<Sensor> SensorsList { get; set; } = new List<Sensor>(); 

        public List<DroneVideo> VideosList { get; set; } = new List<DroneVideo>(); 
        #endregion
    }
}
