using ChaturbatePlayer.Models;
using NullVoidCreations.WpfHelpers;
using System;
using System.IO;
using System.Reflection;

namespace ChaturbatePlayer
{
    class Shared
    {
        public const string ENCRYPTION_KEY = "QFByb3Blcl9QYXRvbGEhMjAxNQ==";

        static object _syncRoot;
        static Shared _instance;

        SettingsManager _settingsManager;
        string _settingsFile, _libVlcPath, _dataPath;
        AssemblyInformation _info;
        SettingsModel _settings;

        #region constructor

        static Shared()
        {
            _syncRoot = new object();
        }

        private Shared()
        {
            
        }

        #endregion

        #region properties

        public static Shared Instance
        {
            get
            {
                lock(_syncRoot)
                {
                    if (_instance == null)
                        _instance = new Shared();

                    return _instance;
                }
            }
        }

        public LicenseModel License { get; set; }

        public SettingsModel Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settingsManager = new SettingsManager();
                    _settingsManager.Load(SettingsFile, ENCRYPTION_KEY);

                    _settings = new SettingsModel(ref _settingsManager);
                }

                return _settings;
            }
        }

        public AssemblyInformation Information
        {
            get
            {
                if (_info == null)
                    _info = new AssemblyInformation(Assembly.GetExecutingAssembly());

                return _info;
            }
        }

        string SettingsFile
        {
            get
            {
                if (_settingsFile == null)
                    _settingsFile = Path.Combine(DataPath, string.Format("{0}.setting", Information.Product));

                return _settingsFile;
            }
        }

        public string LibVlcPath
        {
            get
            {
                if (_libVlcPath == null)
                    _libVlcPath = Path.Combine(DataPath, "VLC");

                return _libVlcPath;
            }
        }

        public string DataPath
        {
            get
            {
                if (_dataPath == null)
                    _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Information.Product);

                return _dataPath;
            }
        }

        #endregion

        public void SaveSettings()
        {
            _settingsManager.Save(SettingsFile, ENCRYPTION_KEY);
        }
    }
}
