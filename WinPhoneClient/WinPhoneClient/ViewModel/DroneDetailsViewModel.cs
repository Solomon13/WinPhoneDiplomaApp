using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;

namespace WinPhoneClient.ViewModel
{
    public class DroneDetailsViewModel : ViewModelBase
    {
        #region Fields

        #endregion

        #region Properties

        public DroneInfo Model { get; set; }

        public string DroneDescription
        {
            get { return $"{Model.DroneType} - '{Model.Id}'"; }
        }

        public string CurrentTask
        {
            get { return Model.Task; }
        }

        public Color ItemColor
        {
            get { return Model.IconColor; }
        }

        public string Possition
        {
            get { return Model.FormatedPossition; }
        }

        public string PolutionLevel
        {
            get { return $"{Model.PolutionLevel} %";}
        }

        public BitmapImage DroneIcon
        {
            get { return Model.DroneIcon; }
        }
        #endregion
    }
}
