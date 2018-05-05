using ChaturbatePlayer.Base;
using ChaturbatePlayer.Models;
using NullVoidCreations.WpfHelpers.Base;

namespace ChaturbatePlayer.ViewModels
{
    public class MainWindowViewModel: ViewModelBase, IMediator
    {
        ChatRoomModel _selectedRoom;
        int _selectedTabIndex;

        public MainWindowViewModel()
        {
            SelectedRoom = null;
            SelectedTabIndex = 0;

            Mediator.Instance.RegisterColleague(this);
        }

        #region properties

        public Gender Featured
        {
            get { return Gender.NotSpecified; }
        }

        public Gender Female
        {
            get { return Gender.Female; }
        }

        public Gender Male
        {
            get { return Gender.Male; }
        }

        public Gender Couple
        {
            get { return Gender.Couple; }
        }

        public Gender Trans
        {
            get { return Gender.Trans; }
        }

        public LicenseModel License
        {
            get { return Shared.Instance.License; }
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
                SelectedTabIndex = 5;
            }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (value == _selectedTabIndex)
                    return;

                _selectedTabIndex = value;
                RaisePropertyChanged(nameof(SelectedTabIndex));
                Mediator.Instance.RaiseNotification(this, NotificationType.TabPageChanged,  _selectedTabIndex);
            }
        }

        #endregion

        public void NotificationReceived(NotificationType type, params object[] data)
        {
           switch(type)
            {
                case NotificationType.ChatRoomSelected:
                    SelectedRoom = (ChatRoomModel)data[0];
                    break;
            }
        }
    }
}
