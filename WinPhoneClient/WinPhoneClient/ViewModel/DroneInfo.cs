using System;
using System.Collections.Generic;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using WinPhoneClient.Enums;
using WinPhoneClient.JSON;
using WinPhoneClient.Model;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace WinPhoneClient.ViewModel
{
    public class DroneInfo : ObservableObject, IJsonDependent
    {
        #region Fields
        private readonly Dictionary<DroneType, BitmapImage> _droneIcons = new Dictionary<DroneType, BitmapImage>
        {
            {Enums.DroneType.aircraft, new BitmapImage(new Uri("ms-appx:/Assets/quadcopter.png", UriKind.RelativeOrAbsolute)) },
            {Enums.DroneType.machine, new BitmapImage(new Uri("ms-appx:/Assets/tank.png", UriKind.RelativeOrAbsolute)) },
            //{Enums.DroneType.Uterus, new BitmapImage(new Uri("ms-appx:/Assets/uterus.png", UriKind.RelativeOrAbsolute)) }
        }; 
        private List<DroneTask>  _tasks = new List<DroneTask>();
        private Visibility _detailsVisibility = Visibility.Collapsed;
        private bool _isSelected;
        private DroneType _droneType;
        private DroneStatus _status;
        private bool _isAvailable;
        private string _name;
        #endregion
        #region Commands
        private RelayCommand _detailsTappedCommand;
        private RelayCommand _detailsDoubleTappedCommand;
        #endregion
        #region Constructor
        public DroneInfo(int id)
        {
            Id = id;
        }
        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set { Set(ref _isAvailable, value); }
        }
        public DroneStatus Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                Set(ref _isSelected, value);
                RaisePropertyChanged(nameof(DetailsVisibility));
                RaisePropertyChanged(nameof(BorderColor));
            }
        }
        public int Id { get; }
        public string FormatedPossition
        {
            //get { return $"{_droneModel.DroneGeopoint.Position.Latitude:##.0000 °}, {_droneModel.DroneGeopoint.Position.Longitude:##.0000 °}"; }
            get { return string.Empty; }
        }

        public DroneType DroneType
        {
            get { return _droneType; }
            set { Set(ref _droneType, value); }
        }

        public string Task
        {
            //get { return _droneModel.CurrentTask; }
            get { return string.Empty; }
        }

        public BitmapImage DroneIcon
        {
            get { return _droneIcons[DroneType]; }
        }

        public Color DroneColor
        {
            get
            {
                switch (DroneType)
                {
                    case Enums.DroneType.aircraft:
                        return Colors.CornflowerBlue;
                    case Enums.DroneType.machine:
                        return Colors.Gray;
                    //case Enums.DroneType.Uterus:
                    //    return Colors.Yellow;
                }

                return Colors.White;
            }
        }

        public Color BorderColor
        {
            get { return IsSelected ? Colors.Red : DroneColor; }
        }

        public Geopoint DroneGeopoint
        {
            //get { return _droneModel.DroneGeopoint; }
            //set
            //{
            //    _droneModel.DroneGeopoint = value;
            //    RaisePropertyChanged(nameof(DroneGeopoint));
            //    RaisePropertyChanged(nameof(FormatedPossition));
            //}
            get { return new Geopoint(new BasicGeoposition { Latitude = 47.6786, Longitude = -122.1511, Altitude = 120 });}
        }

        public Visibility DetailsVisibility => IsSelected ? Visibility.Visible : Visibility.Collapsed;

        public List<DroneVideo> VideoList
        {
            get { return null; }
        }

        public Point Anchor => new Point(0.5, 1);

        #endregion
        #region Command Handlers

        public RelayCommand DetailsTappedCommand
        {
            get
            {
                return _detailsTappedCommand ?? (_detailsTappedCommand = new RelayCommand(() =>
                {
                    var mainViewModel = Application.Current.Resources["Main"] as MainViewModel;
                    mainViewModel?.SelectDroneOnMapCommand.Execute(new KeyValuePair<int, bool>(Id, !IsSelected));
                }));
            }
        }

        public RelayCommand DetailsDoubleTappedCommand
        {
            get
            {
                return _detailsDoubleTappedCommand?? (_detailsDoubleTappedCommand = new RelayCommand(() =>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame?.Navigate(typeof (DroneDetailsPage), this);
                }));
            }
        }
        #endregion

        public void ApplyJson(IBaseJsonValue json)
        {
            if (json is DroneJson)
            {
                try
                {
                    var droneJson = json as DroneJson;
                    Status = droneJson.Status;
                    DroneType = droneJson.DroneType;
                    IsAvailable = droneJson.Available;
                    Name = droneJson.Name;
                }
                catch (Exception)
                {
                }
            }
        }

        public IBaseJsonValue GetJsonValue()
        {
            var droneJson = new DroneJson();
            droneJson.Json = droneJson.CreateEmptyJsonObject();
            droneJson.DroneType = DroneType;
            droneJson.Available = IsAvailable;
            droneJson.Name = Name;
            droneJson.Status = droneJson.Status;
            droneJson.Id = Id;

            return droneJson;
        }
    }
}
