using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using GalaSoft.MvvmLight.Messaging;
using WinPhoneClient.CommandExecuter;
using WinPhoneClient.Common;
using WinPhoneClient.Helpers;
using WinPhoneClient.JSON;

namespace WinPhoneClient.Model
{
    public class ServerAddapter
    {
        //private Task _workingTask;
        //private CancellationTokenSource _cancellationToken;
        //private List<Drone> _dronesList = new List<Drone>()
        //{
        //    new Drone (DroneType.Uterus){
        //        Id = "1",
        //        CurrecntTask = "Uterus test",
        //        DroneGeopoint = new Geopoint(
        //        new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1311, Altitude = 120},
        //        AltitudeReferenceSystem.Terrain),
        //        Locations = new LimitedQueue<BasicGeoposition>
        //        {
        //            new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1311 },
        //            new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1411 },
        //            new BasicGeoposition {Latitude = 47.6686, Longitude = -122.1511 },
        //        },
        //        PolutionLevel = 5
        //    },
        //    new Drone (DroneType.Tank){
        //        Id="2",
        //        CurrecntTask = "Tank test",
        //        DroneGeopoint = new Geopoint(
        //        new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1411, Altitude = 120},
        //        AltitudeReferenceSystem.Terrain),
        //        Locations = new LimitedQueue<BasicGeoposition>
        //        {
        //            new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1411 },
        //            new BasicGeoposition {Latitude = 47.6886, Longitude = -122.1411 },
        //            new BasicGeoposition {Latitude = 47.6986, Longitude = -122.1511 },
        //        },
        //        PolutionLevel = 10
        //    },
        //    new Drone (DroneType.Quadrocopter){
        //        Id="3",
        //        CurrecntTask = "Quadrocopter test",
        //        DroneGeopoint = new Geopoint(
        //        new BasicGeoposition {Latitude = 47.6786, Longitude = -122.1511, Altitude = 120},
        //        AltitudeReferenceSystem.Terrain),
        //        PolutionLevel = 15,
        //        VideosList = new List<DroneVideo> {new DroneVideo(), new DroneVideo(), new DroneVideo() }
        //    }
        //}
        //;
        public Settings Settings { get; } = new Settings();

        #region Methods

        public async void UpdateDroneList()
        {
            if (Settings.Token == null)
                await ConnectToServerAsync();
            if (Settings.Token == null)
            {
                Messenger.Default.Send(new ErrorMessage {Error = "Failed to update drone list"});
                return;
            }

            var drones = await GetAllDrones(new GetAllDronesCommandExecuter(Settings.Host, Settings.Token.FormatedToken));
            if (drones.Any())
            {
                
            }
        }
        public async Task<TokenResponceJson> ConnectToServerAsync(ConnectToServerCommandExecuter executer)
        {
            Settings.Token = null;
            if (executer != null)
                Settings.Token = await executer.ExecuteAsync() as TokenResponceJson;
            return Settings.Token;
        }
        #region GetInfo 
        public async Task<DroneJson> GetDroneByIdAsync(GetDroneCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return new DroneJson {Json = baseJson.Data as JsonObject};

            return null; 
        }

        public async Task<List<DroneJson>> GetAvailableDronesAsync(GetAvailableDronesInfoCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return Utils.GetArrayFromJson<DroneJson>(baseJson);

            return null;
        }

        public async Task<List<DroneJson>> GetAllDrones(GetAllDronesCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return Utils.GetArrayFromJson<DroneJson>(baseJson);

            return null;
        }

        public async Task<List<TaskJson>> GetDroneCommandsAsync(GetDroneTasksCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null && baseJson.Result)
                return Utils.GetArrayFromJson<TaskJson>(baseJson);

            return null;
        }

        public async Task<List<SensorJson>> GetDroneSensorsAsync(GetDroneSensorsCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return Utils.GetArrayFromJson<SensorJson>(baseJson);
            return null;
        }

        public async Task<List<RouteJson>> GetDroneRoutesAsync(GetDroneRoutesCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return Utils.GetArrayFromJson<RouteJson>(baseJson);

            return null;
        }

        public async Task<List<TaskValueJson>> GetTaskValuesAsync(GetTaskValuesCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return Utils.GetArrayFromJson<TaskValueJson>(baseJson);

            return null;
        }

        public async Task<List<SensorValueJson>> GetSensorValuesAsync(GetSensorValuesCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            if (baseJson != null)
                return Utils.GetArrayFromJson<SensorValueJson>(baseJson);

            return null;
        }
        #endregion
        #region Add items

        public async void AddDroneAsync(AddDroneItemCommandExecuter executer)
        {
            if (Settings.Token == null)
                Settings.Token = await ConnectToServerAsync();
            if (Settings.Token != null && executer != null)
                await executer.ExecuteAsync();
        }
        #endregion
        #region Update items

        public async Task<bool> UpdateDroneAsync(UpdateDroneInfoCommandExecuter executer)
        {
            var baseJson = await Execute(executer);
            return baseJson?.Result != null && baseJson.Result;
        }
        #endregion

        private async Task<BaseJson> Execute(ICommandExecuter executer)
        {
            if (Settings.Token == null)
                Settings.Token = await ConnectToServerAsync();
            if (Settings.Token != null && executer != null)
            {
                var baseJson = await executer.ExecuteAsync() as BaseJson;
                if (baseJson != null)
                {
                    if (!baseJson.Result)
                        return null;
                    return baseJson;
                }
            }
            return null;
        }

        private async Task<TokenResponceJson> ConnectToServerAsync()
        {
            return await ConnectToServerAsync(new ConnectToServerCommandExecuter(Settings.Host,
                            new TokenRequestJson(Settings.Login, Settings.Password)));
        } 

        //public void StartServerListening()
        //{
        //    _cancellationToken = new CancellationTokenSource();
        //    _workingTask = Task.Factory.StartNew(RouteTest, _cancellationToken);
        //}

        //public void StopServerListening()
        //{
        //    if(!_workingTask.IsCompleted)
        //        _cancellationToken.Token.ThrowIfCancellationRequested();

        //}

        //private async void RouteTest(object o)
        //{
        //    var cancelationTokenSource = o as CancellationTokenSource;
        //    if (cancelationTokenSource != null)
        //    {
        //        var drone = DroneList[0];
        //        int rectangleSide = 0;
        //        int steptedCount = 0;
        //        var basicPosition = drone.DroneGeopoint.Position;

        //        while (!cancelationTokenSource.IsCancellationRequested)
        //        {
        //            steptedCount++;
        //            switch (rectangleSide)
        //            {
        //                case 0:
        //                    basicPosition.Longitude += 0.01;
        //                    break;
        //                case 1:
        //                    basicPosition.Latitude -= 0.01;
        //                    break;
        //                case 2:
        //                    basicPosition.Longitude -= 0.01;
        //                    break;
        //                case 3:
        //                    basicPosition.Latitude += 0.01;
        //                    break;
        //            }

        //            Messenger.Default.Send(new DronePossitionChangedMessage(drone, new Geopoint(basicPosition)));

        //            if (steptedCount >= 4)
        //            {
        //                rectangleSide = rectangleSide + 1 > 3 ? 0 : rectangleSide + 1;
        //                steptedCount = 0;
        //            }

        //            await Task.Delay(2000);
        //        }
        //    }
        //}
        #endregion
    }
}
