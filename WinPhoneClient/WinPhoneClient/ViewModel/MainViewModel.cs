using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Devices.Geolocation;
using Windows.Networking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WinPhoneClient.Model;

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
        private readonly ServerAddapter _model = new ServerAddapter();
        private Geopoint _userPossition;
        private Geopoint _mapCenterGeopoint;
        private double _zoomLavel = 13.0;
        private Visibility _userPointVisibility = Visibility.Collapsed;
        private Visibility _settingsVisibility = Visibility.Collapsed;
        private string _selectedMapItem = "";
        private int _port;
        private string _serverAddress;
        #endregion
        #region Commands

        private RelayCommand _moveToCenterCommand;
        private RelayCommand _showSettingsCommand;
        private RelayCommand<ItemClickEventArgs> _navigateToDroneDetailsPageCommand;
        private RelayCommand _saveSettingsCommand;
        #endregion
        #region Constructor
        public MainViewModel()
        {
            foreach (var drone in _model.DroneList)
                Drones.Add(new DroneInfo(drone));
            _port = _model.Settings.Port;
            _serverAddress = _model.Settings.IpAdress;
            UpdateCurrentLocation();
            CreateMapItemNamesList();
        }
        #endregion
        #region Properties
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
                }
            }
        }
        public ObservableCollection<string> MapItemNamesCollection { get; } = new ObservableCollection<string>();

        public ObservableCollection<DroneInfo> Drones { get; } = new ObservableCollection<DroneInfo>();

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

        public string Ip
        {
            get { return _serverAddress; }
            set
            {
                Set(ref _serverAddress, value);
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public int Port
        {
            get { return _port; }
            set
            {
                Set(ref _port, value);
                SaveSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public double ZoomLavel
        {
            get { return _zoomLavel; }
            set { Set(ref _zoomLavel, value); }
        }
        #endregion

        #region Methods
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

        private void CreateMapItemNamesList()
        {
            MapItemNamesCollection.Clear();
            MapItemNamesCollection.Add("User");
            foreach (var droneInfo in _model.DroneList)
                MapItemNamesCollection.Add($"{droneInfo.DroneType}-'{droneInfo.Id}'");

            if (!MapItemNamesCollection.Contains(_selectedMapItem))
                SelectedMapItem = MapItemNamesCollection[0];
        }

        private Geopoint FindCenterGeoposition(string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                if (selectedName == "User")
                    return UserPossition;

                var regex = new Regex("'.*'");
                var match = regex.Match(_selectedMapItem);
                if (!string.IsNullOrEmpty(match.Value))
                {
                    var value = match.Value.Replace("'", "");
                    var drone = _model.DroneList.FirstOrDefault(d => d.Id == value);
                    if(drone != null)
                        return drone.DroneGeopoint;
                }
            }

            return null;
        }
        #endregion

        #region Command Handlers

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
                    _model.Settings.IpAdress = Ip;
                    _model.Settings.Port = Port;
                    SaveSettingsCommand.RaiseCanExecuteChanged();
                }, () =>
                {
                    try
                    {
                        var ip = new HostName(Ip);
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    return Ip != _model.Settings.IpAdress || (Port != _model.Settings.Port && Port >= 1024 && Port <= Int16.MaxValue * 2);
                }));
            }
        }
        #endregion
    }
}