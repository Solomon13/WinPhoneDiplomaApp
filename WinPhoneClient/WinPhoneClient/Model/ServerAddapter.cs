using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using WinPhoneClient.Enums;
using WinPhoneClient.ViewModel;

namespace WinPhoneClient.Model
{
    class ServerAddapter
    {
        private Settings _settings = new Settings();
        private ObservableCollection<DroneInfo> _droneList = new ObservableCollection<DroneInfo>()
        {
            new DroneInfo(new Drone (DroneType.Uterus){
                Id = "1",
                CurrecntTask = "Uterus test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1311, Altitude = 120},
                AltitudeReferenceSystem.Terrain) }),
            new DroneInfo(new Drone (DroneType.Tank){
                Id="2",
                CurrecntTask = "Tank test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1411, Altitude = 120},
                AltitudeReferenceSystem.Terrain) }),
            new DroneInfo(new Drone (DroneType.Quadrocopter){
                Id="3",
                CurrecntTask = "Quadrocopter test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1511, Altitude = 120},
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
