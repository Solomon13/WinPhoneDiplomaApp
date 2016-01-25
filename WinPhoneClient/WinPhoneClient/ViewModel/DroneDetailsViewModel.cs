using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WinPhoneClient.Enums;
using WinPhoneClient.Helpers;

namespace WinPhoneClient.ViewModel
{
    public class DroneDetailsViewModel : ViewModelBase
    {
        #region Fields

        private string _selectedCommand = string.Empty;
        #endregion

        #region Constructor

        public DroneDetailsViewModel()
        {
            if (CommandsList.Any())
                _selectedCommand = CommandsList.First();
        }
        #endregion

        #region Commands

        private RelayCommand _sendCommand;
        private RelayCommand _openSettingsCommand;
        private RelayCommand<ItemClickEventArgs> _playVideoCommand;
        #endregion

        #region Properties

        public DroneInfo Model { get; set; }

        public string DroneDescription
        {
            get { return $"{Model.DroneType} - '{Model.Id}'"; }
        }

        public List<string> CommandsList
        {
            get { return Enum.GetNames(typeof (Commands)).ToList(); }
        }

        public string SelectedCommand
        {
            get { return _selectedCommand; }
            set { Set(ref _selectedCommand, value); }
        }
        #endregion

        #region Command Hanglers

        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new RelayCommand(() =>
                {
                    //TODO
                }));
            }
        }

        public RelayCommand OpenSettingsCommand
        {
            get
            {
                return _openSettingsCommand ?? (_openSettingsCommand = new RelayCommand(() =>
                {
                    if (Utils.MainHubNavivateToSection("SettingsHubSection"))
                    {
                        var rootFrame = Window.Current.Content as Frame;
                        rootFrame?.Navigate(typeof (MainPage));
                    }
                }));
            }
        }

        public RelayCommand<ItemClickEventArgs> PlayVideoCommand
        {
            get
            {
                return _playVideoCommand ?? (_playVideoCommand = new RelayCommand<ItemClickEventArgs>((args) =>
                {
                    //TODO
                }));
            }
        } 
        #endregion
    }
}
