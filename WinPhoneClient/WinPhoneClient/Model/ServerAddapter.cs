using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using WinPhoneClient.Enums;
using WinPhoneClient.ViewModel;

namespace WinPhoneClient.Model
{
    class ServerAddapter
    {
        private Settings _settings = new Settings();
        private ObservableCollection<DroneInfo> _droneList = new ObservableCollection<DroneInfo>()
        {
            new DroneInfo(new Drone (DroneType.Uterus){CurrecntTask = "Collect info", DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1311, Altitude = 120},
                AltitudeReferenceSystem.Terrain) })
        };

        public Settings Settings
        {
            get { return _settings; }
        }

        public ObservableCollection<DroneInfo> DroneList
        {
            get { return _droneList; }
        } 
    }
}
