using NullVoidCreations.WpfHelpers.Base;
using System.Collections.Generic;

namespace ChaturbatePlayer.Base
{
    sealed class Mediator: NotificationBase
    {
        static Mediator _instance;
        static object _syncLock;
        readonly List<IMediator> _colleagues;

        #region constructor/destructor

        static Mediator()
        {
            _syncLock = new object();
        }
        
        private Mediator()
        {
            _colleagues = new List<IMediator>();
        }

        #endregion

        #region properties

        public static Mediator Instance
        {
            get
            {
                lock(_syncLock)
                {
                    if (_instance == null)
                        _instance = new Mediator();

                    return _instance;
                }
            }
        }

        #endregion

        public bool RegisterColleague(IMediator colleague)
        {
            if (_colleagues.Contains(colleague))
                return false;
            else
            {
                _colleagues.Add(colleague);
                return true;
            }
        }

        public void UnregisterColleague(IMediator colleague)
        {
            var index = _colleagues.IndexOf(colleague);
            if (index > -1)
                _colleagues.RemoveAt(index);
        }

        public void RaiseNotification(IMediator sender, NotificationType type, params object[] data)
        {
            foreach (var colleague in _colleagues)
            {
                if (!colleague.Equals(sender))
                {
                    colleague.NotificationReceived(type, data);
                }
            }   
        }
    }
}
