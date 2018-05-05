using ChaturbatePlayer.Models;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace ChaturbatePlayer.ViewModels
{
    public class SettingsViewModel: ViewModelBase
    {
        ICommand _selectDirectory;

        IEnumerable<string> _mediaPlayers;
        IEnumerable<int> _refreshIntervals;

        #region properties

        public IEnumerable<string> MediaPlayers
        {
            get
            {
                if (_mediaPlayers == null)
                    _mediaPlayers = new List<string> { "VLC", "Windows" };

                return _mediaPlayers;
            }
        }

        public IEnumerable<int> RefreshIntervals
        {
            get
            {
                if (_refreshIntervals == null)
                {
                    var refreshIntervals = new List<int>();
                    for (var interval = 2; interval <= 15; interval += 2)
                        refreshIntervals.Add(interval);
                    _refreshIntervals = refreshIntervals;
                }

                return _refreshIntervals;
            }
            
        }

        public string ProductName
        {
            get { return Shared.Instance.Information.ProductTitle; }
        }

        public string ProductDescription
        {
            get { return Shared.Instance.Information.Description; }
        }

        public string ProductCopyright
        {
            get { return Shared.Instance.Information.Copyright; }
        }

        public Version ProductVersion
        {
            get { return Shared.Instance.Information.Version; }
        }

        public SettingsModel Settings
        {
            get { return Shared.Instance.Settings; }
        }

        public LicenseModel License
        {
           get { return Shared.Instance.License; }
        }

        #endregion

        #region commands

        public ICommand SelectDirectoryCommand
        {
            get
            {
                if (_selectDirectory == null)
                    _selectDirectory = new RelayCommand<string>(SelectDirectory) { IsSynchronous = true };

                return _selectDirectory;

            }
        }

        #endregion
       
        void SelectDirectory(string dialogDescription)
        {
            if (dialogDescription == null)
                dialogDescription = "Please select a directory below and click OK.";

            var dialog = new FolderBrowserDialog();
            dialog.Description = dialogDescription;
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == DialogResult.OK)
                Settings.AutoSaveDirectory = dialog.SelectedPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? dialog.SelectedPath : string.Format("{0}{1}", dialog.SelectedPath, Path.DirectorySeparatorChar);
        }
    }
}
