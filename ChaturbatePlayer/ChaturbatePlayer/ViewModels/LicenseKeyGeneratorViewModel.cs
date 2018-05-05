using ChaturbatePlayer.Models;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using System.Windows.Input;

namespace ChaturbatePlayer.ViewModels
{
    public class LicenseKeyGeneratorViewModel : ViewModelBase
    {
        LicenseModel _license;
        ICommand _generateLicense;

        #region properties

        public LicenseModel License
        {
            get
            {
                if (_license == null)
                    _license = new LicenseModel();

                return _license;
            }
        }

        #endregion

        #region commands

        public ICommand GenerateLicenseCommand
        {
            get
            {
                if (_generateLicense == null)
                    _generateLicense = new RelayCommand<int>(GenerateLicense);

                return _generateLicense;
            }
        }

        #endregion

        void GenerateLicense(int daysToAdd)
        {
            if (License == null)
                return;

            License.Generate(License.RegisteredEmail, License.IssueDate, daysToAdd);
        }
    }
}
