using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Networking;
using Windows.Storage;
using Windows.UI;
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
using WinPhoneClient.Enums;
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
            //CreateMapItemNamesList();
            //CreateRoutesColection();

            //_model.StartServerListening();

            #region Message Handlers
            Messenger.Default.Register<DronePossitionChangedMessage>(this,async arg =>
            {
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
            });

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
                if (string.Compare(_selectedMapItem, value, StringComparison.CurrentCultureIgnoreCase) != 0)
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
                         UpdateDroneRoute(availableDrone.Id, availableDrone.IsSelected);
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

        #region Methods

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
            MapItemNamesCollection.Clear();
            MapItemNamesCollection.Add("User");
            foreach (var droneInfo in AvailableDrones)
                MapItemNamesCollection.Add($"{droneInfo.Name} - {droneInfo.Id}");

            if (!MapItemNamesCollection.Contains(_selectedMapItem))
                SelectedMapItem = MapItemNamesCollection[0];
            else
            {
                RaisePropertyChanged(nameof(SelectedMapItem));
            }
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

        public async Task UpdateDroneRoute(int droneId, bool bIsSelected = false)
        {
            if (droneId >= 0)
            {
                var droneInfo = AvailableDrones.FirstOrDefault(d => d.Id == droneId);
                if(droneInfo == null)
                    return;

                var route = GetRoute(droneId);
                try
                {
                    var routes = await Addapter.GetDroneRoutesAsync(new GetRoutesInfoCommandExecuter(
                        Addapter.Settings.Host, droneId, Addapter.Settings.Token.FormatedToken));
                    var points =
                        routes.Select(
                            r =>
                                new BasicGeoposition
                                {
                                    Longitude = r.Longtitude,
                                    Latitude = r.Latitude,
                                    Altitude = r.Haight
                                }).ToArray();
                    if (points.Any() && ShowRoutes)
                        DroneRoutes.Add(new DroneRoute(droneId, points, droneInfo.DroneColor) { IsSelected = bIsSelected });

                }
                catch (ArgumentException) { }
                finally 
                {
                    if (route != null)
                        DroneRoutes.Remove(route);
                }
            }
        }

        public void UpdateRouteSelection(int droneId, Color color)
        {
            if (droneId >= 0)
            {
                var route = GetRoute(droneId);
                if (route != null)
                {
                    var points = route.Route.Positions;
                    DroneRoutes.Remove(route);
                    DroneRoutes.Add(new DroneRoute(droneId, points, color));
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

        public async void UpdateDroneCollection()
        {
            if (Addapter.Settings.Token == null)
            {
                if (Utils.MainHubNavivateToSection("SettingsHubSection"))
                    Messenger.Default.Send(new ErrorMessage {Error = "Server connection isn't esteblished"});

                return;
            }

            var dronesJson = await 
                Addapter.GetAllDrones(new GetAllDronesInfoCommandExecuter(Addapter.Settings.Host,
                    Addapter.Settings.Token.FormatedToken));
            if (dronesJson.Any())
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
                        try
                        {
                            var commands = await Addapter.GetDroneCommandsAsync( 
                                new GetCommandsInfoCommandExecuter(Addapter.Settings.Host, droneInfo.Id,
                                Addapter.Settings.Token.FormatedToken));
                            if (commands.Any())
                            {
                                droneInfo.Tasks.Clear();
                                foreach (var commandJson in commands)
                                {
                                    var command = new DroneTask((int) commandJson.CommandId, (int) commandJson.DroneId);
                                    command.ApplyJson(commandJson);
                                    droneInfo.Tasks.Add(command);
                                }

                                droneInfo.RaiseProperty("CurrentTask");
                            }
                        }
                        catch (ArgumentException)
                        {
                        }
                        

                        if (droneInfo.IsAvailable)
                        {
                            AvailableDrones.Add(droneInfo);
                            if(ShowRoutes)
                                await UpdateDroneRoute(droneInfo.Id);
                        }
                    }
                    catch (Exception e)
                    {
                        Messenger.Default.Send(new ErrorMessage {Error = e.Message});
                    }
                }
                UpdateMapItemNamesList();
            }

        }

        //private void CreateRoutesColection()
        //{
        //    DroneRoutes.Clear();
        //    foreach (var droneInfo in Drones)
        //    {
        //        if (droneInfo.Model.Locations != null && droneInfo.Model.Locations.Any())
        //            DroneRoutes.Add(new DroneRoute(droneInfo.Id, droneInfo.Model.Locations, droneInfo.DroneColor));
        //    }
        //}

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

        #region Command Handlers

        public RelayCommand ConnectToServerCommand
        {
            get
            {
                return _connectToServerCommand ?? (_connectToServerCommand = new RelayCommand(async () =>
                {
                    SaveSettingsCommand.Execute(null);
                    var token = await _serverAddapter.ConnectToServerAsync(new ConnectToServerCommandExecuter(
                        Host, 
                        new TokenRequestJson(UserLogin, UserPassword)));
                    if (token == null)
                    {
                        IAsyncOperation<IUICommand> dialogTask = null;
                        var messageBox = new MessageDialog($"Can't connect to server {Host}");
                        messageBox.Commands.Add(new UICommand("Try again", command => ConnectToServerCommand.Execute(null)));
                        messageBox.Commands.Add(new UICommand("Cancel", command => dialogTask?.Cancel()));
                        dialogTask = messageBox.ShowAsync();
                        try
                        {
                            await dialogTask;
                        }
                        catch (TaskCanceledException)
                        {
                        }
                    }
                    UpdateDroneCollection();
                    RaiseCanEecuteCommandSettingsTab();
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
                               UpdateRouteSelection(droneInfo.Id, arg.Value ? Colors.Red : droneInfo.DroneColor);
                               var route = GetRoute(droneInfo.Id);
                               if(route != null)
                                  route.IsSelected = arg.Value;
                           }
                       }));
            }
        }
        #endregion
    }
}