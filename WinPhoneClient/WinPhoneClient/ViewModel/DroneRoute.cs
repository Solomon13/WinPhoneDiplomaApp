using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace WinPhoneClient.ViewModel
{
    public class DroneRoute : ObservableObject
    {
        #region Fields
        private readonly Color _color;
        private RelayCommand _selectCommand;
        #endregion
        #region Properties
        public bool IsSelected { get; set; }
        public Geopath Route { get; set; }
        public Color RouteColor {
            get { return IsSelected ? Colors.Red : _color; } }
        public string DroneId { get; }
        #endregion
        #region Constructor
        public DroneRoute(string droneId, IEnumerable<BasicGeoposition> points, Color routeColor)
        {
            if(string.IsNullOrEmpty(droneId))
                throw new ArgumentException("droneId");
            if (points == null)
                throw new ArgumentException("points");

            Route = new Geopath(points);
            _color = routeColor;
            DroneId = droneId;
        }
        #endregion
        #region Command Handlers
        public RelayCommand SelectCommand
        {
            get
            {
                return _selectCommand ?? (_selectCommand = new RelayCommand(() =>
                {
                    var mainViewModel = Application.Current.Resources["Main"] as MainViewModel;
                    mainViewModel?.SelectDroneOnMapCommand.Execute(new KeyValuePair<string, bool>(DroneId, !IsSelected));
                }));
            }
        }
        #endregion
    }
}
