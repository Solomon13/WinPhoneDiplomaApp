using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;
using WinPhoneClient.Model;

namespace WinPhoneClient.ViewModel
{
    public class DroneInfo : ObservableObject
    {
        #region Fields
        private readonly Drone _droneModel;
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

        public string Possition
        {
            get { return $"Possition: {_droneModel.Possition}"; }
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
        #endregion
    }
}
