using NullVoidCreations.WpfHelpers;
using NullVoidCreations.WpfHelpers.Base;
using System;

namespace ChaturbatePlayer.Models
{
    public class LicenseModel : NotificationBase
    {
        bool _isTrial;
        DateTime _issueDate, _expirationDate;
        string _registeredEmail, _licenseKey;

        public LicenseModel()
        {
            Rest();
        }

        internal void Rest()
        {
            IssueDate = new DateTime(1900, 1, 1);
            ExpirationDate = new DateTime(1900, 1, 1);
            IsTrial = true;
        }

        #region properties

        public string LicenseKey
        {
            get { return _licenseKey; }
            set
            {
                Set(nameof(LicenseKey), ref _licenseKey, value);
            }
        }

        public bool IsTrial
        {
            get { return _isTrial; }
            private set
            {
                Set(nameof(IsTrial), ref _isTrial, value);
            }
        }

        public string RegisteredEmail
        {
            get { return _registeredEmail; }
            set
            {
                Set(nameof(RegisteredEmail), ref _registeredEmail, value);
            }
        }

        public DateTime IssueDate
        {
            get { return _issueDate; }
            set
            {
                Set(nameof(IssueDate), ref _issueDate, value);
            }
        }

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            private set
            {
                Set(nameof(ExpirationDate), ref _expirationDate, value);
            }
        }

        #endregion

        #region private methods

        bool IsExpired(LicenseModel license)
        {
            var currentDate = DateTime.Now;
            if (currentDate.Date < license.IssueDate.Date)
                return true;
            if (currentDate.Date > license.ExpirationDate.Date)
                return true;

            return false;
        }

        DateTime ExtractDate(string text, int startIndex)
        {
            var date = new DateTime(
                int.Parse(text.Substring(startIndex, 4)),
                int.Parse(text.Substring(startIndex + 4, 2)),
                int.Parse(text.Substring(startIndex + 6, 2)));
            return date;
        }

        #endregion

        internal void Generate(string registeredEmail, DateTime issueDate, int numberOfDays)
        {
            RegisteredEmail = registeredEmail;
            IssueDate = issueDate;
            ExpirationDate = IssueDate.AddDays(numberOfDays);
            LicenseKey = string.Format("{0:0000}{1:00}{2:00}{3:0000}{4:00}{5:00}{6}",
                IssueDate.Year,
                IssueDate.Month,
                IssueDate.Day,
                ExpirationDate.Year,
                ExpirationDate.Month,
                ExpirationDate.Day,
                RegisteredEmail);
            LicenseKey = LicenseKey.Encrypt(Shared.ENCRYPTION_KEY);
        }

        public bool Validate()
        {
            try
            {
                var key = LicenseKey.Decrypt(Shared.ENCRYPTION_KEY);
                IssueDate = ExtractDate(key, 0);
                ExpirationDate = ExtractDate(key, 8);
                RegisteredEmail = key.Substring(16, key.Length - 16);
                IsTrial = IsExpired(this);
                return true;
            }
            catch
            {
                Rest();
            }

            return false;
        }
    }
}