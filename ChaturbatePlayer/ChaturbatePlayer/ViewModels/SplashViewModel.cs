using ChaturbatePlayer.Base;
using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Input;

namespace ChaturbatePlayer.ViewModels
{
    public class SplashViewModel: ViewModelBase, IMediator
    {
        ICommand _initialize;
        const string VLC_LIB_ARCHIVE = "VLC.zip";

        #region properties

        public string ProductName
        {
            get { return Shared.Instance.Information.Product; }
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
            get {  return Shared.Instance.Information.Version; }
        }

        #endregion

        #region commands

        public ICommand InitializeCommand
        {
            get
            {
                if (_initialize == null)
                    _initialize = new RelayCommand(Initialize);

                return _initialize;
            }
        }

        #endregion

        void Initialize()
        {
            if (Directory.Exists(Shared.Instance.LibVlcPath))
            {
                Mediator.Instance.RaiseNotification(this, NotificationType.Initialized);
                return;
            }

            var archivePath = Path.Combine(Application.Current.GetStartupDirectory(), VLC_LIB_ARCHIVE);
            if (!File.Exists(archivePath))
            {
                Application.Current.Shutdown(1);
                return;
            }

            if (!Directory.Exists(Shared.Instance.DataPath))
                Directory.CreateDirectory(Shared.Instance.DataPath);

            using (var zip = ZipStorer.Open(archivePath, FileAccess.Read))
            {
                foreach (ZipStorer.ZipFileEntry zipEntry in zip.ReadCentralDir())
                    zip.ExtractFile(zipEntry, Path.Combine(Shared.Instance.DataPath, zipEntry.FilenameInZip));
            }

            Mediator.Instance.RaiseNotification(this, NotificationType.Initialized);
        }

        public void NotificationReceived(NotificationType type, params object[] data)
        {
            // do nothing
        }
    }
}
