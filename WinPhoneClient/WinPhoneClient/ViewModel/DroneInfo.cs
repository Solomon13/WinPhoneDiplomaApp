using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WinPhoneClient.Common;
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
            {DroneType.aircraft, new BitmapImage(new Uri("ms-appx:/Assets/quadcopter.png", UriKind.RelativeOrAbsolute)) },
            {DroneType.machine, new BitmapImage(new Uri("ms-appx:/Assets/tank.png", UriKind.RelativeOrAbsolute)) },
            //{Enums.DroneType.Uterus, new BitmapImage(new Uri("ms-appx:/Assets/uterus.png", UriKind.RelativeOrAbsolute)) }
        };
        private readonly Dictionary<int, BitmapImage> _batteryIcons = new Dictionary<int, BitmapImage>
        {
            {0, new BitmapImage(new Uri("ms-appx:/Assets/battery-empty.png", UriKind.RelativeOrAbsolute)) },
            {1, new BitmapImage(new Uri("ms-appx:/Assets/battery-1.png", UriKind.RelativeOrAbsolute)) },
            {2, new BitmapImage(new Uri("ms-appx:/Assets/battery-2.png", UriKind.RelativeOrAbsolute)) },
            {3, new BitmapImage(new Uri("ms-appx:/Assets/battery-half.png", UriKind.RelativeOrAbsolute)) },
            {4, new BitmapImage(new Uri("ms-appx:/Assets/battery-3.png", UriKind.RelativeOrAbsolute)) },
            {5, new BitmapImage(new Uri("ms-appx:/Assets/battery-full.png", UriKind.RelativeOrAbsolute)) },
        };

        private bool _isSelected;
        private DroneType _droneType;
        private DroneStatus _status;
        private bool _isAvailable;
        private string _name;
        private Geopoint _currentPossition;
        private DroneRoute _route;
        private int _batteryLevel;
        private SensorInfo _selectedSensor;
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

        public SensorInfo SelectedSensor
        {
            get
            {
                if (_selectedSensor == null || !Sensors.Contains(_selectedSensor))
                    _selectedSensor = Sensors.FirstOrDefault();

                return _selectedSensor;
            }
            set
            {
                if (Sensors.Contains(value))
                {
                    Set(ref _selectedSensor, value);
                    RaisePropertyChanged(nameof(SelectedSensorValues));
                }
            }
        }

        public ObservableCollection<SensorValueJson> SelectedSensorValues => SelectedSensor?.Values;

        public int BatteryLevel
        {
            get { return _batteryLevel; }
            set
            {
                Set(ref _batteryLevel, value);
                RaisePropertyChanged(nameof(BatteryFormatedString));
                RaisePropertyChanged(nameof(BatteryIcon));
            }
        }

        public BitmapImage BatteryIcon
        {
            get
            {
                if (BatteryLevel <= 5)
                    return _batteryIcons[0];
                if (BatteryLevel > 5 && BatteryLevel <= 15)
                    return _batteryIcons[1];
                if (BatteryLevel > 15 && BatteryLevel <= 35)
                    return _batteryIcons[2];
                if (BatteryLevel > 35 && BatteryLevel <= 60)
                    return _batteryIcons[3];
                if (BatteryLevel > 60 && BatteryLevel <= 85)
                    return _batteryIcons[4];

                return _batteryIcons[5];
            }
        }

        public DroneRoute Route
        {
            get { return _route; }
            set { Set(ref _route, value); }
        }
        public ObservableCollection<DroneTask> Tasks { get; } = new ObservableCollection<DroneTask>();
        public ObservableCollection<SensorInfo> Sensors { get; } = new ObservableCollection<SensorInfo>(); 

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }
        public bool IsAvailable
        {
            get { return _isAvailable; }
            set
            {
                if (value != _isAvailable)
                {
                    _isAvailable = value;
                    Messenger.Default.Send(new DroneAvailableChangedMessage(this));
                    RaisePropertyChanged(nameof(IsAvailable));
                }
            }
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
        public string FormatedPossition => $"{DroneGeopoint.Position.Latitude:##.0000 °}, {DroneGeopoint.Position.Longitude:##.0000 °}";

        public int SensorsCount => Sensors?.Count ?? 0;

        public DroneType DroneType
        {
            get { return _droneType; }
            set { Set(ref _droneType, value); }
        }

        public string CurrentTask
        {
            get
            {
                DroneTask last = null;
                if(Tasks.Any())
                   last = Tasks.Last();
                return last == null ? string.Empty : last.Description;
            }
        }

        public BitmapImage DroneIcon => _droneIcons[DroneType];

        public Color DroneColor
        {
            get
            {
                switch (DroneType)
                {
                    case DroneType.aircraft:
                        return Colors.CornflowerBlue;
                    case DroneType.machine:
                        return Colors.Yellow;
                    //case Enums.DroneType.Uterus:
                    //    return Colors.Yellow;
                }

                return Colors.White;
            }
        }
        public string BatteryFormatedString => $"{BatteryLevel} %";
        public Color BorderColor => IsSelected ? Colors.Red : DroneColor;

        public Geopoint DroneGeopoint
        {
            get { return _currentPossition; }
            set
            {
                Set(ref _currentPossition, value);
                RaisePropertyChanged(nameof(FormatedPossition));
            }
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

        public void RaiseProperty(string propName)
        {
            if(!string.IsNullOrEmpty(propName))
                RaisePropertyChanged(propName);
        }
        public void ApplyJson(IBaseJsonValue json)
        {
            if (json is DroneJson)
            {
                try
                {
                    var droneJson = json as DroneJson;
                    Status = droneJson.Status;
                    DroneType = droneJson.DroneType;
                    _isAvailable = droneJson.Available;
                    Name = droneJson.Name;
                    BatteryLevel = (int)droneJson.Battery;
                    DroneGeopoint = new Geopoint(new BasicGeoposition { Longitude = droneJson.Longtitude, Latitude = droneJson.Latitude });
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
            droneJson.Battery = BatteryLevel;
            droneJson.Latitude = DroneGeopoint.Position.Latitude;
            droneJson.Longtitude = DroneGeopoint.Position.Longitude;

            return droneJson;
        }
    }
}
