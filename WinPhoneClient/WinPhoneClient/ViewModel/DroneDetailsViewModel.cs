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
        private VideoPlayStatus _videoStatus = VideoPlayStatus.Stoped;
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
        private RelayCommand<MediaElement> _playVideoCommand;
        private RelayCommand<MediaElement> _pauseVideoCommand;
        private RelayCommand<MediaElement> _stopVideoCommand;
        #endregion

        #region Properties

        public VideoPlayStatus VideoStatus
        {
            get { return _videoStatus;}
            set
            {
                Set(ref _videoStatus, value);
                RaiseCanExecuteCommands();
            }
        }
        public DroneInfo Model { get; set; }

        public string DroneDescription => $"{Model.Name} - '{Model.Id}'";

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

        public RelayCommand<MediaElement> PauseVideoCommand
        {
            get
            {
                return _pauseVideoCommand ?? (_pauseVideoCommand = new RelayCommand<MediaElement>(video =>
                {
                    if (video != null)
                    {
                        video.Pause();
                        VideoStatus = VideoPlayStatus.Paused;
                    }
                }, video => VideoStatus == VideoPlayStatus.Playing));
            }
        }

        public RelayCommand<MediaElement> PlayVideoCommand
        {
            get
            {
                return _playVideoCommand ?? (_playVideoCommand = new RelayCommand<MediaElement>(video =>
                {
                    if (video != null)
                    {
                            video.Play();
                            VideoStatus = VideoPlayStatus.Playing;
                    }
                }, video => VideoStatus != VideoPlayStatus.Playing ));
            }
        }

        public RelayCommand<MediaElement> StopVideoCommand
        {
            get
            {
                return _stopVideoCommand ?? (_stopVideoCommand = new RelayCommand<MediaElement>(video =>
                {
                    if (video != null)
                    {
                        video.Stop();
                        VideoStatus = VideoPlayStatus.Stoped;
                    }
                }, video => VideoStatus != VideoPlayStatus.Stoped));
            }
        }
        #endregion
        #region Private methods

        private void RaiseCanExecuteCommands()
        {
            PlayVideoCommand.RaiseCanExecuteChanged();
            StopVideoCommand.RaiseCanExecuteChanged();
            PauseVideoCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
