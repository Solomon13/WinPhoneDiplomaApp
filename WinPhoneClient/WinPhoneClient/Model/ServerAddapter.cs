using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using WinPhoneClient.Enums;
using WinPhoneClient.ViewModel;

namespace WinPhoneClient.Model
{
    class ServerAddapter
    {
        private Settings _settings = new Settings();
        private List<Drone> _dronesList = new List<Drone>()
        {
            new Drone (DroneType.Uterus){
                Id = "1",
                CurrecntTask = "Uterus test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1311, Altitude = 120},
                AltitudeReferenceSystem.Terrain),
                Locations = new List<BasicGeoposition>
                {
                    new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1311 },
                    new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1411 },
                    new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1511 },
                },
                PolutionLevel = 5
            },
            new Drone (DroneType.Tank){
                Id="2",
                CurrecntTask = "Tank test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1411, Altitude = 120},
                AltitudeReferenceSystem.Terrain),
                Locations = new List<BasicGeoposition>
                {
                    new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1311 },
                    new BasicGeoposition {Latitude = 47.6886, Longitude = -122.1411 },
                    new BasicGeoposition {Latitude = 47.6986, Longitude = -122.1511 },
                },
                PolutionLevel = 10
            },
            new Drone (DroneType.Quadrocopter){
                Id="3",
                CurrecntTask = "Quadrocopter test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1511, Altitude = 120},
                AltitudeReferenceSystem.Terrain),
                PolutionLevel = 15,
                VideosList = new List<DroneVideo> {new DroneVideo(), new DroneVideo(), new DroneVideo() }
            }
        };

        public Settings Settings
        {
            get { return _settings; }
        }

        public List<Drone> DroneList
        {
            get { return _dronesList; }
        } 
    }
}
