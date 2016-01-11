using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WinPhoneClient.Enums;
using WinPhoneClient.Model;

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
            get { return _detailsVisibility; }
            set
            {
                Set(ref _detailsVisibility, value);
                RaisePropertyChanged(nameof(PointVisibility));
            }
        }

        public Visibility PointVisibility
        {
            get { return DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }
        #endregion
        #region Command Handlers

        public RelayCommand DetailsTappedCommand
        {
            get
            {
                return _detailsTappedCommand ?? (_detailsTappedCommand = new RelayCommand(() =>
                {
                    DetailsVisibility = DetailsVisibility == Visibility.Collapsed
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }));
            }
        }

        public RelayCommand DetailsDoubleTappedCommand
        {
            get
            {
                return _detailsDoubleTappedCommand?? (_detailsDoubleTappedCommand = new RelayCommand(() =>
                {
                    //todo Show details page for drone
                }));
            }
        }
        #endregion
    }
}
