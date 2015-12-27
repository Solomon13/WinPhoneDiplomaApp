using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
        private ObservableCollection<DroneInfo> _droneCollection = new ObservableCollection<DroneInfo>(); 
        #endregion
        #region Constructor

        public MainViewModel()
        {
            foreach (var drone in _model.DroneList)
                _droneCollection.Add(new DroneInfo(drone));
        }

        public ObservableCollection<DroneInfo> Drones
        {
            get { return _droneCollection; }
            set { Set(ref _droneCollection, value); }

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
        #endregion
    }
}