using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using WinPhoneClient.Model;

namespace WinPhoneClient.ViewModel
{
    public class DroneInfo : ObservableObject
    {
        #region Fields
        private readonly Drone _droneModel;
        private Visibility _detailsVisibility = Visibility.Collapsed;
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

        public Color IconColor
        {
            get
            {
                switch (_droneModel.DroneType)
                {
                    case Enums.DroneType.Quadrocopter:
                        return Colors.Blue;
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
            set { Set(ref _detailsVisibility, value); }
        }
        #endregion
    }
}
