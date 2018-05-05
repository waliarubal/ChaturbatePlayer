using ChaturbatePlayer.Base;
using ChaturbatePlayer.Models;
using Microsoft.Win32;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace ChaturbatePlayer.ViewModels
{
    public enum LiveStreamPlayerState : byte
    {
        Play,
        Playing,
        Pause,
        Paused,
        Stop,
        Stopped,
        ExternalMediaPlayer
    }

    public class LiveStreamPlayerViewModel : ViewModelBase, IMediator
    {
        ICommand _toggleMute, _playerCommand;
        LiveStreamPlayerState _state;
        ChatRoomModel _selectedRoom;
        bool _isMuted;

        public LiveStreamPlayerViewModel()
        {
            Mediator.Instance.RegisterColleague(this);
        }

        #region commands

        public ICommand PlayerCommand
        {
            get
            {
                if (_playerCommand == null)
                    _playerCommand = new RelayCommand<LiveStreamPlayerState>(Player);

                return _playerCommand;
            }
        }

        public ICommand ToggleMuteCommand
        {
            get
            {
                if (_toggleMute == null)
                    _toggleMute = new RelayCommand(ToggleMute);

                return _toggleMute;
            }
        }

        #endregion

        #region properties

        public LiveStreamPlayerState State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged(nameof(State));
            }
        }

        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                _isMuted = value;
                RaisePropertyChanged(nameof(IsMuted));
            }
        }

        public SettingsModel Settings
        {
            get { return Shared.Instance.Settings; }
        }

        public ChatRoomModel SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                if (value == _selectedRoom)
                    return;

                _selectedRoom = value;
                RaisePropertyChanged(nameof(SelectedRoom));
                State = LiveStreamPlayerState.Play;
            }
        }

        #endregion

        string GetWmpExecutablePath()
        {
            using (var view = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                using (var key = view.OpenSubKey(@"Software\Microsoft\Active Setup\Installed Components\{22d6f312-b0f6-11d0-94ab-0080c74c7e95}", false))
                {
                    if ((int)key.GetValue("IsInstalled") == 1)
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Media Player\wmplayer.exe");
                    else
                        return string.Empty;
                }
            }
        }

        string GetVlcExecutablePath()
        {
            using (var view = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                using (var key = view.OpenSubKey(@"Software\VideoLAN\VLC\", false))
                {
                    if (key == null)
                        return Path.Combine(Shared.Instance.LibVlcPath, "vlc.exe");
                    else
                        return key.GetValue(string.Empty).ToString();
                }
            }
        }

        void PlayVideoFeed(ChatRoomModel chatRoom)
        {
            string playerPath, arguments;

            var useVlc = "VLC".Equals(Settings.MediaPlayer);
            if (useVlc)
            {
                playerPath = GetVlcExecutablePath();

                var VlcArgumentsBuilder = new StringBuilder();

                // hide controls
                if (Settings.IsExternalMediaPlayerControlsHidden)
                    VlcArgumentsBuilder.Append(" --qt-minimal-view");

                // full screen
                if (Settings.IsExternalMediaPlayerFullScreen)
                    VlcArgumentsBuilder.Append(" --fullscreen");

                // show URL in title bar
                if (!Settings.IsUrlShown)
                    VlcArgumentsBuilder.AppendFormat(" --meta-title=\"{0}\"", chatRoom.Name);

                // video feed URL
                VlcArgumentsBuilder.AppendFormat(" {0}", chatRoom.VideoFeedUrl);

                // recording options
                if (Settings.IsAutoSaveVideosEnabled)
                {
                    var videoDirectory = Settings.AutoSaveDirectory;
                    if (string.IsNullOrEmpty(videoDirectory))
                        videoDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                    if (Directory.Exists(videoDirectory))
                        VlcArgumentsBuilder.AppendFormat(" --sout=\"#duplicate{{dst=std{{access=file,dst='{0}{1}.mp4'}},dst=display}}\"", videoDirectory, chatRoom.Name);
                }

                arguments = VlcArgumentsBuilder.ToString().Trim();
            }
            else
            {
                playerPath = GetWmpExecutablePath();
                arguments = string.Format("\"{0}\"", chatRoom.VideoFeedUrl, chatRoom.Name);
            }

            if (string.IsNullOrEmpty(playerPath) || string.IsNullOrEmpty(arguments))
                return;

            var process = new Process();
            process.StartInfo.FileName = playerPath;
            process.StartInfo.Arguments = arguments;
            process.Start();
        }

        void Player(LiveStreamPlayerState state)
        {
            if (SelectedRoom == null)
                return;

            switch (state)
            {
                case LiveStreamPlayerState.Play:
                    State = LiveStreamPlayerState.Play;
                    break;

                case LiveStreamPlayerState.Pause:
                    State = LiveStreamPlayerState.Pause;
                    break;

                case LiveStreamPlayerState.Stop:
                    State = LiveStreamPlayerState.Stop;
                    break;

                case LiveStreamPlayerState.ExternalMediaPlayer:
                    State = LiveStreamPlayerState.ExternalMediaPlayer;
                    PlayVideoFeed(SelectedRoom);
                    break;
            }
        }

        void ToggleMute()
        {
            IsMuted = !IsMuted;
        }

        public void NotificationReceived(NotificationType type, params object[] data)
        {
            switch(type)
            {
                case NotificationType.TabPageChanged:
                    if (Shared.Instance.Settings.IsAutoPauseEnabled)
                    {
                        var index = (int)data[0];
                        if (index == 5 && (State == LiveStreamPlayerState.Paused || State == LiveStreamPlayerState.Pause))
                            State = LiveStreamPlayerState.Play;
                        else if (index != 5 && State == LiveStreamPlayerState.Playing)
                            State = LiveStreamPlayerState.Pause;
                    }
                    break;

                case NotificationType.ChatRoomSelected:
                    SelectedRoom = (ChatRoomModel)data[0];
                    break;
            }
        }
    }
}
