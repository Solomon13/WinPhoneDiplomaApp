using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using WinPhoneClient.Enums;
using WinPhoneClient.Model;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace WinPhoneClient.ViewModel
{
    public class DroneInfo : ObservableObject
    {
        #region Fields
        private readonly Dictionary<DroneType, BitmapImage> DroneIcons = new Dictionary<DroneType, BitmapImage>
        {
            {Enums.DroneType.Quadrocopter, new BitmapImage(new Uri("ms-appx:/Assets/quadcopter.png", UriKind.RelativeOrAbsolute)) },
            {Enums.DroneType.Tank, new BitmapImage(new Uri("ms-appx:/Assets/tank.png", UriKind.RelativeOrAbsolute)) },
            {Enums.DroneType.Uterus, new BitmapImage(new Uri("ms-appx:/Assets/uterus.png", UriKind.RelativeOrAbsolute)) }
        }; 
        private readonly Drone _droneModel;
        private Visibility _detailsVisibility = Visibility.Collapsed;
        private bool _isSelected;
        #endregion
        #region Commands
        private RelayCommand _detailsTappedCommand;
        private RelayCommand _detailsDoubleTappedCommand;
        #endregion
        #region Constructor
        public DroneInfo(Drone drone)
        {
            if(drone == null)
                throw new ArgumentException("drone");
            _droneModel = drone;
        }
        #endregion

        #region Properties

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
        public Drone Model
        {
            get { return _droneModel; }
        }
        public string Id
        {
            get { return _droneModel.Id; }
        }
        public string FormatedPossition
        {
            get { return $"{_droneModel.DroneGeopoint.Position.Latitude:##.0000 °}, {_droneModel.DroneGeopoint.Position.Longitude:##.0000 °}"; }
        }

        public string DroneType
        {
            get { return _droneModel.DroneType.ToString(); }
        }

        public string Task
        {
            get { return _droneModel.CurrecntTask; }
        }

        public BitmapImage DroneIcon
        {
            get { return DroneIcons[_droneModel.DroneType]; }
        }

        public Color IconColor
        {
            get
            {
                switch (_droneModel.DroneType)
                {
                    case Enums.DroneType.Quadrocopter:
                        return Colors.CornflowerBlue;
                    case Enums.DroneType.Tank:
                        return Colors.Gray;
                    case Enums.DroneType.Uterus:
                        return Colors.Yellow;
                }

                return Colors.White;
            }
        }

        public Color BorderColor
        {
            get { return IsSelected ? Colors.Red : IconColor; }
        }

        public Geopoint DroneGeopoint
        {
            get { return _droneModel.DroneGeopoint; }
            set
            {
                _droneModel.DroneGeopoint = value;
                RaisePropertyChanged(nameof(DroneGeopoint));
                RaisePropertyChanged(nameof(FormatedPossition));
            }
        }

        public Visibility DetailsVisibility
        {
            get { return IsSelected ? Visibility.Visible : Visibility.Collapsed; }
        }

        public double PolutionLevel
        {
            get { return _droneModel.PolutionLevel; }
        }

        public List<DroneVideo> VideoList
        {
            get { return _droneModel.VideosList; }
        }

        public Point Anchor
        {
            get { return new Point(0.5, 1); }
        }
        #endregion
        #region Command Handlers

        public RelayCommand DetailsTappedCommand
        {
            get
            {
                return _detailsTappedCommand ?? (_detailsTappedCommand = new RelayCommand(() =>
                {
                    var mainViewModel = Application.Current.Resources["Main"] as MainViewModel;
                    mainViewModel?.SelectDroneOnMapCommand.Execute(new KeyValuePair<string, bool>(Id, !IsSelected));
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
    }
}
