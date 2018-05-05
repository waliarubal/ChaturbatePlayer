using ChaturbatePlayer.Base;
using System;
using System.Windows;

namespace ChaturbatePlayer.Views
{
    /// <summary>
    /// Interaction logic for SplashWindowView.xaml
    /// </summary>
    public partial class SplashWindowView: IMediator
    {
        public SplashWindowView()
        {
            InitializeComponent();
            Mediator.Instance.RegisterColleague(this);
        }

        public void NotificationReceived(NotificationType type, params object[] data)
        {
            switch(type)
            {
                case NotificationType.Initialized:
                    var action = (Action)(() => Close());
                    Application.Current.Dispatcher.BeginInvoke(action);
                    break;
            }
        }
    }
}
