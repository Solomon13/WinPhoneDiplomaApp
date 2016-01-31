using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using WinPhoneClient.CommandExecuter;
using WinPhoneClient.Common;
using WinPhoneClient.Helpers;
using WinPhoneClient.JSON;
using WinPhoneClient.Model;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace WinPhoneClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private Task _workingTask;
        private CancellationTokenSource _cancellationToken;
        private readonly ServerAddapter _serverAddapter = new ServerAddapter();
        private Geopoint _userPossition;
        private Geopoint _mapCenterGeopoint;
        private double _zoomLavel = 13.0;
        private Visibility _userPointVisibility = Visibility.Collapsed;
        private Visibility _settingsVisibility = Visibility.Collapsed;
        private string _selectedMapItem = "";
        private string _serverHost;
        private string _userLogin;
        private string _userPassword;
        private bool _showRoutes = true;
        private bool _isLoading;
        #endregion
        #region Commands

        private RelayCommand _moveToCenterCommand;
        private RelayCommand _showSettingsCommand;
        private RelayCommand<ItemClickEventArgs> _navigateToDroneDetailsPageCommand;
        private RelayCommand _saveSettingsCommand;
        private RelayCommand<KeyValuePair<int, bool>> _selectDroneOnMapCommand;
        private RelayCommand _connectToServerCommand;
        private ObservableCollection<DroneInfo> _availableDronesCollection = new ObservableCollection<DroneInfo>(); 
        #endregion
        #region Constructor
        public MainViewModel()
        {
            _serverHost = _serverAddapter.Settings.Host;
            _userLogin = _serverAddapter.Settings.Login;
            _userPassword = _serverAddapter.Settings.Password;
            UpdateCurrentLocation();
            ConnectToServerCommand.Execute(null);

            #region Message Handlers
            Messenger.Default.Register<DroneAvailableChangedMessage>(this, async arg =>
            {
                if (arg?.Drone != null)
                {
                    var updateDroneJson = new UpdateDroneJson();
                    updateDroneJson.Json = updateDroneJson.CreateEmptyJsonObject();
                    updateDroneJson.Name = arg.Drone.Name;
                    updateDroneJson.Available = arg.Drone.IsAvailable ? 1.0 : 0.0;
                    updateDroneJson.DroneType = arg.Drone.DroneType;
                    updateDroneJson.Status = arg.Drone.Status;

                    if (await Addapter.UpdateDroneAsync(new UpdateDroneInfoCommandExecuter(
                        Addapter.Settings.Host,
                        Addapter.Settings.Token.FormatedToken,
                        arg.Drone.Id,
                        updateDroneJson)))
                        UpdateDroneAvailable(arg.Drone);
                }
            });
            //Messenger.Default.Register<DronePossitionChangedMessage>(this,async arg =>
            //{
                //if (arg != null)
                //{
                //    var droneInfo = Drones.FirstOrDefault(d => d.Model == arg.ChangedDrone);
                //    if (droneInfo != null)
                //    {
                //        await AnimatedChangePossitionAsync(droneInfo, droneInfo.DroneGeopoint, arg.NewDronePosition);
                //        await DispatcherHelper.UIDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                //        {
                //            if (arg.NewRoutePoints == null)
                //                arg.ChangedDrone.Locations.Add(arg.NewDronePosition.Position);
                //            else
                //                arg.ChangedDrone.Locations.AddRange(arg.NewRoutePoints);
                //            UpdateDroneRoute(droneInfo.Id, droneInfo.IsSelected);
                //        });
                        
                //    }
                //}
            //});

            Messenger.Default.Register<ErrorMessage>(this, async msg =>
            {
                IAsyncOperation<IUICommand> dialogTask = null;
                var messageBox = new MessageDialog($"Error: {msg.Error}");
                messageBox.Commands.Add(new UICommand("OK", command => dialogTask?.Cancel()));
                dialogTask = messageBox.ShowAsync();
                try
                {
                    await dialogTask;
                }
                catch (TaskCanceledException)
                {
                }
            });
            #endregion
        }
        #endregion
        #region Properties
        public bool IsLoading { get { return _isLoading; }
            set { Set(ref _isLoading, value); } }
        public ServerAddapter Addapter => _serverAddapter;

        public string UserLogin
        {
            get { return _userLogin; }
            set
            {
                Set(ref _userLogin, value);
                RaiseCanEecuteCommandSettingsTab();
            }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                Set(ref _userPassword, value);
                RaiseCanEecuteCommandSettingsTab();
            }
        }
        public string SelectedMapItem
        {
            get { return _selectedMapItem; }
            set
            {
                if (!string.IsNullOrEmpty(value) && string.Compare(_selectedMapItem, value, StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    _selectedMapItem = value;
                    var geopoint = FindCenterGeoposition(_selectedMapItem);
                    if (geopoint != null)
                        MapCenterGeopoint = geopoint;
                    RaisePropertyChanged(nameof(SelectedMapItem));
                }
            }
        }
        public ObservableCollection<string> MapItemNamesCollection { get; } = new ObservableCollection<string>();
        public ObservableCollection<DroneInfo> Drones { get; } = new ObservableCollection<DroneInfo>();
        public ObservableCollection<DroneRoute> DroneRoutes { get; } = new ObservableCollection<DroneRoute>();

        public ObservableCollection<DroneInfo> AvailableDrones
        {
            get { return _availableDronesCollection; }
            set { Set(ref _availableDronesCollection, value); }
        }

        public bool ShowRoutes
        {
            get { return _showRoutes; }
            set
            {
                Set(ref _showRoutes, value);
                if (value)
                {
                    foreach (var availableDrone in AvailableDrones)
                        UpdateRouteSelection(availableDrone.Id, availableDrone.IsSelected);
                }
                else
                    DroneRoutes.Clear();
            }
        }

        public Visibility UserPointVisibility
        {
            get { return _userPointVisibility; }
            set { Set(ref _userPointVisibility, value); }
        }

        public Visibility SettingsVisibility
        {
            get { return _settingsVisibility; }
            set { Set(ref _settingsVisibility, value); }
        }

        public Geopoint MapCenterGeopoint
        {
            get { return _mapCenterGeopoint; }
            set
            {
                Set(ref _mapCenterGeopoint, value);
                MoveToCenterCommand.RaiseCanExecuteChanged();
            }
        }
        public Geopoint UserPossition
        {
            get { return _userPossition; }
            set
            {
                Set(ref _userPossition, value);
                RaisePropertyChanged(nameof(UserPossitionString));
            }
        }

        public string UserPossitionString
        {
            get
            {
                return _userPossition != null
                    ? $"{_userPossition.Position.Latitude:##.0000 °}, {_userPossition.Position.Longitude:##.0000 °}" : string.Empty;
            }
        }

        public string Host
        {
            get { return _serverHost; }
            set
            {
                Set(ref _serverHost, value);
                RaiseCanEecuteCommandSettingsTab();
            }
        }

        public double ZoomLavel
        {
            get { return _zoomLavel; }
            set { Set(ref _zoomLavel, value); }
        }
        #endregion

        #region Public Methods
        public async Task StartServerListening()
        {
            _cancellationToken = new CancellationTokenSource();
            while (!_cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(30000);
                UpdateCurrentLocation();
                await UpdateDroneCollection();
            }
        }

        public void UpdateDroneAvailable(DroneInfo droneInfo)
        {
            if (droneInfo != null )
            {
                if (droneInfo.IsAvailable)
                {
                    if (!AvailableDrones.Contains(droneInfo))
                        AvailableDrones.Add(droneInfo);
                    if (ShowRoutes)
                        UpdateRouteSelection(droneInfo.Id, droneInfo.IsSelected);
                }
                else
                {
                    if (AvailableDrones.Contains(droneInfo))
                        AvailableDrones.Remove(droneInfo);
                    RemoveDroneRoute(droneInfo.Id);
                }
                droneInfo.RaiseProperty("IsAvailable");
                UpdateMapItemNamesList();
            }
        }

        public async void StopServerListening()
        {
            if (_workingTask != null && _cancellationToken != null && !_workingTask.IsCompleted)
            {
                _cancellationToken.Token.ThrowIfCancellationRequested();
                await _workingTask;
            }

        }
        public void SaveLocalSettings()
        {
            ApplicationData.Current.LocalSettings.Values[nameof(Addapter.Settings.Host)] = Addapter.Settings.Host;
            ApplicationData.Current.LocalSettings.Values[nameof(Addapter.Settings.Login)] = Addapter.Settings.Login;
            ApplicationData.Current.LocalSettings.Values[nameof(Addapter.Settings.Password)] = Addapter.Settings.Password;
        }

        public void LoadLocalSttings()
        {
            Host = ApplicationData.Current.LocalSettings.Values[nameof(Addapter.Settings.Host)]?.ToString();
            UserLogin = ApplicationData.Current.LocalSettings.Values[nameof(Addapter.Settings.Login)]?.ToString();
            UserPassword = ApplicationData.Current.LocalSettings.Values[nameof(Addapter.Settings.Password)]?.ToString();

            if (string.IsNullOrEmpty(Host))
                Host = Settings.DefaultHost;
            if (string.IsNullOrEmpty(UserLogin))
                UserLogin = Settings.DefaultUserName;
            if (string.IsNullOrEmpty(UserPassword))
                UserPassword = Settings.DefaultPassword;
            SaveSettingsCommand.Execute(null);
        }
       
        public async Task UpdateDroneRoute(DroneInfo droneInfo, bool bLatestOnly = false)
        {
            if (droneInfo == null)
                return;
            DroneRoute route = null;
            try
            {
                List<RouteJson> routes = null;
                if (bLatestOnly && droneInfo.Route != null && droneInfo.Route.AddedTime > 0)
                {
                    routes = await Addapter.GetDroneRoutesAsync(new GetDroneRoutesCommandExecuter(
                        Addapter.Settings.Host,
                        Addapter.Settings.Token.FormatedToken,
                        droneInfo.Id, 
                        droneInfo.Route.AddedTime,
                        Utils.GetNowUnixTime()));
                }
                else
                {
                    routes = await Addapter.GetDroneRoutesAsync(new GetDroneRoutesCommandExecuter(
                        Addapter.Settings.Host, Addapter.Settings.Token.FormatedToken, droneInfo.Id));
                }
                if (routes != null)
                {
                    var points =
                        routes.Select(
                            r =>
                                new BasicGeoposition
                                {
                                    Longitude = r.Longtitude,
                                    Latitude = r.Latitude,
                                    Altitude = r.Haight
                                }).ToArray();

                    if (points.Any())
                    {
                        if (bLatestOnly && droneInfo.Route != null)
                            droneInfo.Route.AddPoints(points);                      
                        else
                            droneInfo.Route = new DroneRoute(droneInfo.Id, points, droneInfo.DroneColor);

                        droneInfo.Route.AddedTime = routes.Max(r => Utils.ConvertToUnixTime(r.Added));

                        if (ShowRoutes)
                        {
                            route = GetRoute(droneInfo.Id);
                            if (route != null)
                                droneInfo.Route.IsSelected = route.IsSelected;
                            DroneRoutes.Add(droneInfo.Route);
                        }
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                if (route != null)
                    DroneRoutes.Remove(route);
            }
        }

        public async Task UpdateDroneTasks(DroneInfo droneInfo, bool bLatestOnly = false)
        {
            if (droneInfo != null)
            {
                try
                {
                    List<TaskJson> tasks;
                    if (bLatestOnly && droneInfo.Tasks.Any())
                    {
                        tasks = await Addapter.GetDroneCommandsAsync(
                           new GetDroneTasksCommandExecuter(
                               Addapter.Settings.Host,
                               Addapter.Settings.Token.FormatedToken, 
                               droneInfo.Id,
                               droneInfo.Tasks.Max(t => t.AddedTime),
                               Utils.GetNowUnixTime()));
                    }
                    else
                    {
                        tasks = await Addapter.GetDroneCommandsAsync(
                            new GetDroneTasksCommandExecuter(
                                Addapter.Settings.Host,
                                Addapter.Settings.Token.FormatedToken, 
                                droneInfo.Id));
                    }
                    if (tasks != null && tasks.Any())
                    {
                        if(!bLatestOnly)
                            droneInfo.Tasks.Clear();
                        foreach (var taskJson in tasks)
                        {
                            var task = new DroneTask((int)taskJson.CommandId, (int)taskJson.DroneId);
                            task.ApplyJson(taskJson);
                            var commandValues = await Addapter.GetTaskValuesAsync(new GetTaskValuesCommandExecuter(
                                Addapter.Settings.Host, Addapter.Settings.Token.FormatedToken, task.TaskId));
                            if (commandValues != null && commandValues.Any())
                                task.Values = commandValues;
                            droneInfo.Tasks.Add(task);
                        }

                        droneInfo.RaiseProperty("CurrentTask");
                    }
                }
                catch (ArgumentException)
                {
                }
            }
        }

        public async Task UpdateDroneSensors(DroneInfo droneInfo, bool bLatestOnly = false)
        {
            if (droneInfo != null)
            {
                try
                {
                    var sensors = await Addapter.GetDroneSensorsAsync(new GetDroneSensorsCommandExecuter(
                        Addapter.Settings.Host, Addapter.Settings.Token.FormatedToken, droneInfo.Id));
                    if (sensors != null && sensors.Any())
                    {
                        if (bLatestOnly && droneInfo.Sensors.Any())
                        {
                            var removedSensors =
                                droneInfo.Sensors.Where(d => sensors.All(jsonSensor => (int) jsonSensor.Id != d.SensorId)).ToArray();
                            if (removedSensors.Any())
                            {
                                foreach (var removedSensor in removedSensors)
                                    droneInfo.Sensors.Remove(removedSensor);
                            }
                        }
                        else
                        {
                            droneInfo.Sensors.Clear();
                        }
                        foreach (var sensorJson in sensors)
                        {
                            var sensor = (bLatestOnly ? droneInfo.Sensors.FirstOrDefault(s => s.SensorId == (int)sensorJson.Id) : null) ??
                                                new SensorInfo((int)sensorJson.Id, (int)sensorJson.DroneId);
                            sensor.ApplyJson(sensorJson);
                            List<SensorValueJson> sensorValues;
                            if (bLatestOnly && sensor.Values != null && sensor.Values.Any())
                                sensorValues = await Addapter.GetSensorValuesAsync(
                                    new GetSensorValuesCommandExecuter(
                                    Addapter.Settings.Host,
                                    Addapter.Settings.Token.FormatedToken,
                                    (int)sensorJson.Id, 
                                    sensor.Values.Max(v => Utils.ConvertToUnixTime(v.Added)),
                                    Utils.GetNowUnixTime()));
                            
                            else
                                sensorValues =  await Addapter.GetSensorValuesAsync(
                                    new GetSensorValuesCommandExecuter(
                                    Addapter.Settings.Host, 
                                    Addapter.Settings.Token.FormatedToken, 
                                    (int) sensorJson.Id));

                            if (sensorValues != null && sensorValues.Any())
                            {
                                if (bLatestOnly)
                                    sensor.AddValues(sensorValues);
                                else
                                    sensor.Values = new ObservableCollection<SensorValueJson>(sensorValues);
                            }

                            if(!droneInfo.Sensors.Contains(sensor))
                                droneInfo.Sensors.Add(sensor);
                        }
                    }
                }
                catch (ArgumentException)
                {
                }
            }
        }

        public void UpdateRouteSelection(int droneId, bool bSelected)
        {
            if (droneId >= 0)
            {
                DroneRoute route = GetRoute(droneId);
                try
                {
                    var drone = Drones.FirstOrDefault(d => d.Id == droneId);
                    if (drone?.Route != null)
                    {
                        drone.Route.IsSelected = bSelected;
                        DroneRoutes.Add(drone.Route);
                    }
                }
                catch (Exception) { }
                finally
                {
                    if (route != null)
                        DroneRoutes.Remove(route);
                }
            }
        }

        public void RemoveDroneRoute(int droneId)
        {
            if (droneId >= 0 && ShowRoutes)
            {
                var route = GetRoute(droneId);
                if (route != null)
                    DroneRoutes.Remove(route);
            }
        }

        public DroneRoute GetRoute(int droneId)
        {
            if (droneId >= 0)
                return DroneRoutes.FirstOrDefault(r => r.DroneId == droneId);
            return null;
        }

        public async Task CreateDroneCollection()
        {
            if (Addapter.Settings.Token == null)
                ConnectToServerCommand.Execute(null);
            List<DroneJson> dronesJson = null;
            try
            {
                if (Addapter.Settings.Token != null)
                    dronesJson = await
                        Addapter.GetAllDrones(new GetAllDronesCommandExecuter(Addapter.Settings.Host, Addapter.Settings.Token.FormatedToken));
            }
            catch (Exception)
            {
                return;
            }
            if (dronesJson != null && dronesJson.Any())
            {
                Drones.Clear();
                DroneRoutes.Clear();
                AvailableDrones.Clear();
                foreach (var droneJson in dronesJson)
                {
                    try
                    {
                        var droneInfo = new DroneInfo((int)droneJson.Id);
                        droneInfo.ApplyJson(droneJson);
                        Drones.Add(droneInfo);
                        await UpdateDroneRoute(droneInfo);
                        await UpdateDroneTasks(droneInfo);
                        await UpdateDroneSensors(droneInfo);
                        UpdateDroneAvailable(droneInfo);
                    }
                    catch (Exception e)
                    {
                        Messenger.Default.Send(new ErrorMessage {Error = e.Message});
                    }
                }
                UpdateMapItemNamesList();
            }
        }

        public void NavigateToSettingsHub()
        {
            var mainHub = Utils.GetMainHub();
            if (mainHub != null)
            {
                var section = Utils.FindVisualChildren<HubSection>(mainHub).FirstOrDefault(s => s.Name == "SettingsHubSection");
                if (section != null)
                    mainHub.ScrollToSection(section);
            }
        }
        #endregion
        #region Private Methods

        private async Task UpdateDroneCollection()
        {
            try
            {
                if (Addapter.Settings.Token != null)
                {
                    var drones = await
                        Addapter.GetAllDrones(new GetAllDronesCommandExecuter(Addapter.Settings.Host,
                            Addapter.Settings.Token.FormatedToken));
                    if (drones != null && drones.Any())
                    {
                        var removedDrones = Drones.Where(d => drones.All(jsonDrone => (int)jsonDrone.Id != d.Id)).ToArray();
                        if (removedDrones.Any())
                        {
                            foreach (var removedDrone in removedDrones)
                                Drones.Remove(removedDrone);
                        }

                        foreach (var droneJson in drones)
                        {
                            var droneInfo = Drones.FirstOrDefault(d => d.Id == (int) droneJson.Id);
                            bool bNew = false;
                            if (droneInfo == null)
                            {
                                droneInfo = new DroneInfo((int) droneJson.Id);
                                Drones.Add(droneInfo);
                                bNew = true;
                            }
                            droneInfo.ApplyJson(droneJson);
                            await UpdateDroneRoute(droneInfo, !bNew);
                            await UpdateDroneTasks(droneInfo, !bNew);
                            await UpdateDroneSensors(droneInfo, !bNew);
                            UpdateDroneAvailable(droneInfo);
                        }
                       
                    }
                }
            }
            catch (Exception)
            {
                //Messenger.Default.Send(new ErrorMessage {Error = "Can't update drones info. Please check connection to the server"});
            }
        }
        private void RaiseCanEecuteCommandSettingsTab()
        {
            SaveSettingsCommand.RaiseCanExecuteChanged();
            ConnectToServerCommand.RaiseCanExecuteChanged();
        }
        private async void UpdateCurrentLocation()
        {
            UserPointVisibility = Visibility.Collapsed;
            var geolocator = new Geolocator();
            var location = await geolocator.GetGeopositionAsync(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(60));
            UserPossition = location.Coordinate.Point;
            UserPointVisibility = Visibility.Visible;
            if (MapCenterGeopoint == null)
                MapCenterGeopoint = UserPossition;
        }

        private void UpdateMapItemNamesList()
        {
            var selectedItem = SelectedMapItem;
            MapItemNamesCollection.Clear();
            MapItemNamesCollection.Add("User");
            foreach (var droneInfo in AvailableDrones)
                MapItemNamesCollection.Add($"{droneInfo.Name} - {droneInfo.Id}");

            _selectedMapItem = MapItemNamesCollection.Contains(selectedItem) ? selectedItem : MapItemNamesCollection[0];
            RaisePropertyChanged(nameof(SelectedMapItem)); 
        }

        private Geopoint FindCenterGeoposition(string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                if (selectedName == "User")
                    return UserPossition;
                var tokens = SelectedMapItem.Split(' ');
                if (tokens.Any())
                {
                    int id;
                    if (Int32.TryParse(tokens.Last(), out id))
                    {
                        var drone = AvailableDrones.FirstOrDefault(d => d.Id == id);
                        if (drone != null)
                            return drone.DroneGeopoint;
                    }
                }
            }

            return null;
        }
        private async Task AnimatedChangePossitionAsync(DroneInfo drone, Geopoint startPosition, Geopoint endPossition, int stepCount = 25)
        {
            if (drone == null || startPosition == null || endPossition == null || stepCount <= 0)
                return;

            await Task.Factory.StartNew(async () =>
            {
                var startPoint = startPosition.Position;
                var endPoint = endPossition.Position;
                var longitudeStep = (endPoint.Longitude - startPoint.Longitude) / stepCount;
                var latitudeStep = (endPoint.Latitude - startPoint.Latitude) / stepCount;
                for (int step = 0; step < stepCount; step++)
                {
                    startPoint.Longitude += longitudeStep;
                    startPoint.Latitude += latitudeStep;
                    await DispatcherHelper.UIDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => drone.DroneGeopoint = new Geopoint(startPoint));
                }
            }
            );
        }
        #endregion
        #region Command Handlers

        public RelayCommand ConnectToServerCommand
        {
            get
            {
                return _connectToServerCommand ?? (_connectToServerCommand = new RelayCommand(async () =>
                {
                    IsLoading = true;
                    StopServerListening();
                    SaveSettingsCommand.Execute(null);
                    var token = await _serverAddapter.ConnectToServerAsync(new ConnectToServerCommandExecuter(
                        Host, 
                        new TokenRequestJson(UserLogin, UserPassword)));
                    if (token == null)
                    {
                        IAsyncOperation<IUICommand> dialogTask = null;
                        var messageBox = new MessageDialog($"Can't connect to server {Host}");
                        messageBox.Commands.Add(new UICommand("Try again", command => ConnectToServerCommand.Execute(null)));
                        messageBox.Commands.Add(new UICommand("Cancel", command =>
                        {
                            //todo Stop updating data here
                            IsLoading = false;
                            Utils.MainHubNavivateToSection("SettingsHubSection");
                            dialogTask?.Cancel();
                        }));
                        dialogTask = messageBox.ShowAsync();
                        try
                        {
                            await dialogTask;
                        }
                        catch (TaskCanceledException)
                        {
                            return;
                        }
                    }
                    await CreateDroneCollection();
                    RaiseCanEecuteCommandSettingsTab();
                    IsLoading = false;
                    _workingTask = StartServerListening();
                }, () => SaveSettingsCommand.CanExecute(null)));
            }
        }
        public RelayCommand ShowSettingsCommand
        {
            get
            {
                return _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(() =>
                {
                    SettingsVisibility = SettingsVisibility == Visibility.Collapsed
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                    //todo doesn't work(((
                    //var mainHub = Utils.GetMainHub();
                    //if (mainHub != null)
                    //{
                    //    var grid = Utils.FindVisualChildren<Grid>(mainHub).FirstOrDefault(g => g.Name == "SettingsGrid");
                    //    if (grid != null)
                    //    {
                    //        IsLoading = !IsLoading;
                    //        var storyboard = (Storyboard) (IsLoading
                    //            ? grid.Resources["ShowMapSettingsAction"]
                    //            : grid.Resources["HideMapSettingsAction"]);
                    //        storyboard?.Begin();
                    //    }
                    //}
                }));
            }
        }

        public RelayCommand MoveToCenterCommand
        {
            get
            {
                return _moveToCenterCommand ?? (_moveToCenterCommand = new RelayCommand(() =>
                {
                    var geopoint = FindCenterGeoposition(SelectedMapItem);
                    if (geopoint != null)
                        MapCenterGeopoint = geopoint;

                }, () =>
                {
                    var point = FindCenterGeoposition(SelectedMapItem);
                    if (point == null)
                        return false;
                    double differenceLatitude = Math.Abs(point.Position.Latitude * .00001);
                    double differenceLongtitude = Math.Abs(point.Position.Longitude * .00001);

                    return Math.Abs((double)(point.Position.Latitude - MapCenterGeopoint?.Position.Latitude)) >= differenceLatitude ||
                              Math.Abs((double)(point.Position.Longitude - MapCenterGeopoint?.Position.Longitude)) >= differenceLongtitude;
                }));
            }
        }

        public RelayCommand<ItemClickEventArgs> NavigateToDroneDetailsPageCommand
        {
            get
            {
                return _navigateToDroneDetailsPageCommand ?? (_navigateToDroneDetailsPageCommand = new RelayCommand<ItemClickEventArgs>(
                    args =>
                    {
                        var rootItem = Window.Current.Content as Frame;
                        rootItem?.Navigate(typeof (DroneDetailsPage), args.ClickedItem);
                    }));
            }
        }

        public RelayCommand SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(() =>
                {
                    _serverAddapter.Settings.Host = Host;
                    _serverAddapter.Settings.Login = UserLogin;
                    _serverAddapter.Settings.Password = UserPassword;
                    RaiseCanEecuteCommandSettingsTab();
                }, () => !string.IsNullOrEmpty(Host) && !string.IsNullOrEmpty(UserLogin) && !string.IsNullOrEmpty(UserPassword) &&
                (_serverAddapter.Settings.Host != Host 
                || Addapter.Settings.Login != UserLogin 
                || Addapter.Settings.Password != UserPassword
                || Addapter.Settings.Token == null)));
            }
        }

        public RelayCommand<KeyValuePair<int,bool>> SelectDroneOnMapCommand
        {
            get
            {
                return _selectDroneOnMapCommand ??
                       (_selectDroneOnMapCommand = new RelayCommand<KeyValuePair<int, bool>>(arg =>
                       {
                           var droneInfo = AvailableDrones.FirstOrDefault(d => d.Id == arg.Key);
                           if (droneInfo != null)
                           {
                               droneInfo.IsSelected = arg.Value;
                               UpdateRouteSelection(droneInfo.Id, arg.Value);
                           }
                       }));
            }
        }
        #endregion
    }
}