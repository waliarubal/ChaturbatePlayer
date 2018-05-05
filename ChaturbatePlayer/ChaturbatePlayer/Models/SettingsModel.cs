using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System.IO;

namespace ChaturbatePlayer.Models
{
    public class SettingsModel: NotificationBase
    {
        readonly SettingsManager _manager;
        LicenseModel _license;

        public SettingsModel(ref SettingsManager manager)
        {
            _manager = manager;
        }

        internal void Reset()
        {
            MediaPlayer = "VLC";
            RefreshInterval = 2;
            IsMinutesLiveShown = false;
            IsAutoRefreshEnabled = false;
            IsExternalMediaPlayerAllowed = false;
            IsExternalMediaPlayerControlsHidden = false;
            IsExternalMediaPlayerFullScreen = false;
            IsAutoPauseEnabled = false;
            IsAutoSaveVideosEnabled = false;
            AutoSaveDirectory = Shared.Instance.DataPath;
            IsUrlShown = false;
        }

        #region properties

        public string LicenseKey
        {
            get { return _manager.GetValue<string>(nameof(LicenseKey)); }
            set
            {
                _manager.SetValue(nameof(LicenseKey), value);
                RaisePropertyChanged(nameof(LicenseKey));
            }
        }

        public LicenseModel License
        {
            get { return _license; }
            private set
            {
                _license = value;
                RaisePropertyChanged(nameof(License));
            }
        }

        public string MediaPlayer
        {
            get
            {
                var value = _manager.GetValue<string>(nameof(MediaPlayer));
                if (default(string) == value)
                    value = "VLC";

                return value;
            }
            set
            {
                _manager.SetValue(nameof(MediaPlayer), value);
                RaisePropertyChanged(nameof(MediaPlayer));
            }
        }

        public int RefreshInterval
        {
            get
            {
                var value = _manager.GetValue<int>(nameof(RefreshInterval));
                if (default(int) == value)
                    value = 2;

                return value;
            }
            set
            {
                _manager.SetValue<int>(nameof(RefreshInterval), value);
                RaisePropertyChanged(nameof(RefreshInterval));
            }
        }

        public bool IsMinutesLiveShown
        {
            get { return _manager.GetValue<bool>(nameof(IsMinutesLiveShown)); }
            set
            {
                _manager.SetValue(nameof(IsMinutesLiveShown), value);
                RaisePropertyChanged(nameof(IsMinutesLiveShown));
            }
        }

        public bool IsAutoRefreshEnabled
        {
            get { return _manager.GetValue<bool>(nameof(IsAutoRefreshEnabled)); }
            set
            {
                _manager.SetValue(nameof(IsAutoRefreshEnabled), value);
                RaisePropertyChanged(nameof(IsAutoRefreshEnabled));
            }
        }

        public bool IsExternalMediaPlayerAllowed
        {
            get { return _manager.GetValue<bool>(nameof(IsExternalMediaPlayerAllowed)); }
            set
            {
                _manager.SetValue(nameof(IsExternalMediaPlayerAllowed), value);
                RaisePropertyChanged(nameof(IsExternalMediaPlayerAllowed));
            }
        }

        public bool IsExternalMediaPlayerControlsHidden
        {
            get { return _manager.GetValue<bool>(nameof(IsExternalMediaPlayerControlsHidden)); }
            set
            {
                _manager.SetValue(nameof(IsExternalMediaPlayerControlsHidden), value);
                RaisePropertyChanged(nameof(IsExternalMediaPlayerControlsHidden));
            }
        }

        public bool IsExternalMediaPlayerFullScreen
        {
            get { return _manager.GetValue<bool>(nameof(IsExternalMediaPlayerFullScreen)); }
            set
            {
                _manager.SetValue(nameof(IsExternalMediaPlayerFullScreen), value);
                RaisePropertyChanged(nameof(IsExternalMediaPlayerFullScreen));
            }
        }

        public bool IsAutoPauseEnabled
        {
            get { return _manager.GetValue<bool>(nameof(IsAutoPauseEnabled)); }
            set
            {
                _manager.SetValue(nameof(IsAutoPauseEnabled), value);
                RaisePropertyChanged(nameof(IsAutoPauseEnabled));
            }
        }

        public bool IsAutoSaveVideosEnabled
        {
            get { return _manager.GetValue<bool>(nameof(IsAutoSaveVideosEnabled)); }
            set
            {
                _manager.SetValue(nameof(IsAutoSaveVideosEnabled), value);
                RaisePropertyChanged(nameof(IsAutoSaveVideosEnabled));
            }
        }

        public string AutoSaveDirectory
        {
            get { return _manager.GetValue<string>(nameof(AutoSaveDirectory)); }
            set
            {
                // always end directory name with '\'
                if (!string.IsNullOrEmpty(AutoSaveDirectory) && value[value.Length - 1] != (Path.DirectorySeparatorChar))
                    value = string.Format("{0}{1}", value, Path.DirectorySeparatorChar);

                _manager.SetValue(nameof(AutoSaveDirectory), value);
                RaisePropertyChanged(nameof(AutoSaveDirectory));
            }
        }

        public bool IsUrlShown
        {
            get { return _manager.GetValue<bool>(nameof(IsUrlShown)); }
            set
            {
                _manager.SetValue(nameof(IsUrlShown), value);
                RaisePropertyChanged(nameof(IsUrlShown));
            }
        }

        #endregion
    }
}
