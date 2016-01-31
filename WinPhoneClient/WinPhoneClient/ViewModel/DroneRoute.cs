using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Json;
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
        public int DroneId { get; }
        public int AddedTime { get; set; }
        #endregion
        #region Constructor
        public DroneRoute(int droneId, IEnumerable<BasicGeoposition> points, Color routeColor)
        {
            if(droneId < 0)
                throw new ArgumentException("droneId");
            if (points == null)
                throw new ArgumentException("points");

            Route = new Geopath(points);
            _color = routeColor;
            DroneId = droneId;
        }
        #endregion
        #region Public methods

        public void AddPoints(IEnumerable<BasicGeoposition> points)
        {
            var basicGeopositions = points as BasicGeoposition[] ?? points.ToArray();
            if (points != null && basicGeopositions.Any())
            {
                var routePoints = Route.Positions.ToList();
                routePoints.AddRange(basicGeopositions);
                Route = new Geopath(routePoints);
            }
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
                    mainViewModel?.SelectDroneOnMapCommand.Execute(new KeyValuePair<int, bool>(DroneId, !IsSelected));
                }));
            }
        }
        #endregion
    }
}
