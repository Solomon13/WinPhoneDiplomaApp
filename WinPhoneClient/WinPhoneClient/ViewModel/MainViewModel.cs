using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
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
        private double _zoomLavel = 15;
        private Visibility _userPointVisibility = Visibility.Collapsed;
        #endregion
        #region Constructor

        public MainViewModel()
        {
            UpdateCurrentLocation();
        }

        public ObservableCollection<DroneInfo> Drones
        {
            get { return _model.DroneList; }
        }

        public Visibility UserPointVisibility
        {
            get { return _userPointVisibility; }
            set { Set(ref _userPointVisibility, value); }
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
        }
        #endregion
    }
}