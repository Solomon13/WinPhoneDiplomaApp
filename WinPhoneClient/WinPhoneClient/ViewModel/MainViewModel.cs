using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
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
        private string _selectedMapItem = String.Empty;
        #endregion
        #region Commands

        private RelayCommand _showSettingsCommand; 
        #endregion
        #region Constructor
        public MainViewModel()
        {
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
                    UpdateCenterGeoposition(_selectedMapItem);
                }
            }
        }
        public ObservableCollection<string> MapItemNamesCollection { get; } = new ObservableCollection<string>();

        public ObservableCollection<DroneInfo> Drones
        {
            get { return _model.DroneList; }
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
            set { Set(ref _mapCenterGeopoint, value); }
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
            get { return _model.Settings.IpAdress; }
            set
            {
                if (string.Compare(_model.Settings.IpAdress, value, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    _model.Settings.IpAdress = value;
                    RaisePropertyChanged(nameof(Ip));
                } 
            }
        }

        public int Port
        {
            get { return _model.Settings.Port; }
            set
            {
                if (_model.Settings.Port != value)
                {
                    _model.Settings.Port = value;
                    RaisePropertyChanged(nameof(Port));
                }
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

        private void UpdateCenterGeoposition(string selectedName)
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                if (selectedName == "User")
                {
                    MapCenterGeopoint = UserPossition;
                    return;
                }

                var regex = new Regex("'.*'");
                var match = regex.Match(_selectedMapItem);
                if (!string.IsNullOrEmpty(match.Value))
                {
                    var value = match.Value.Replace("'", "");
                    var drone = _model.DroneList.FirstOrDefault(d => d.Id == value);
                    if(drone != null)
                        MapCenterGeopoint = drone.DroneGeopoint;
                }
            }
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
        #endregion
    }
}