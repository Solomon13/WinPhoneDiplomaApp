using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight.Messaging;
using WinPhoneClient.Common;
using WinPhoneClient.Enums;
using WinPhoneClient.ViewModel;

namespace WinPhoneClient.Model
{
    class ServerAddapter
    {
        private Task _workingTask;
        private CancellationTokenSource _cancellationToken;
        private Settings _settings = new Settings();
        private List<Drone> _dronesList = new List<Drone>
        {
            new Drone (DroneType.Uterus){
                Id = "1",
                CurrecntTask = "Uterus test",
                DroneGeopoint = new Geopoint(
                new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1311, Altitude = 120},
                AltitudeReferenceSystem.Terrain),
                Locations = new LimitedQueue<BasicGeoposition>
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
                Locations = new LimitedQueue<BasicGeoposition>
                {
                    new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1411 },
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
        #region Methods

        public void StartServerListening()
        {
            _cancellationToken = new CancellationTokenSource();
            _workingTask = Task.Factory.StartNew(RouteTest, _cancellationToken);
        }

        public void StopServerListening()
        {
            if(!_workingTask.IsCompleted)
                _cancellationToken.Token.ThrowIfCancellationRequested();

        }

        private async void RouteTest(object o)
        {
            var cancelationTokenSource = o as CancellationTokenSource;
            if (cancelationTokenSource != null)
            {
                var drone = DroneList[0];
                int rectangleSide = 0;
                int steptedCount = 0;
                var basicPosition = drone.DroneGeopoint.Position;

                while (!cancelationTokenSource.IsCancellationRequested)
                {
                    steptedCount++;
                    switch (rectangleSide)
                    {
                        case 0:
                            basicPosition.Longitude += 0.01;
                            break;
                        case 1:
                            basicPosition.Latitude -= 0.01;
                            break;
                        case 2:
                            basicPosition.Longitude -= 0.01;
                            break;
                        case 3:
                            basicPosition.Latitude += 0.01;
                            break;
                    }

                    Messenger.Default.Send(new DronePossitionChangedMessage(drone, new Geopoint(basicPosition)));

                    if (steptedCount >= 4)
                    {
                        rectangleSide = rectangleSide + 1 > 3 ? 0 : rectangleSide + 1;
                        steptedCount = 0;
                    }

                    await Task.Delay(2000);
                }
            }
        }
        #endregion
    }
}
