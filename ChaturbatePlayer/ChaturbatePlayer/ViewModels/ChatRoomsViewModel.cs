using ChaturbatePlayer.Base;
using ChaturbatePlayer.Models;
using HtmlAgilityPack;
using NullVoidCreations.WpfHelpers.Base;
using NullVoidCreations.WpfHelpers.Commands;
using NullVoidCreations.WpfHelpers.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace ChaturbatePlayer.ViewModels
{
    public enum NavigationDirection: byte
    {
        Forward,
        Backward,
        Refresh
    }

    public class ChatRoomsViewModel: ViewModelBase, IMediator
    {
        ICommand _getChatRooms, _getVideoFeed;

        Gender _gender;
        IList<ChatRoomModel> _chatRooms;
        ChatRoomModel _selectedRoom;
        Timer _refreshTimer;
        volatile int _page;

        const string CHATURBATE = "https://www.chaturbate.com";
        const string HOST = "chaturbate.com";

        #region constructor/destructor

        public ChatRoomsViewModel()
        {
            _refreshTimer = new Timer();
            _refreshTimer.Elapsed += RefreshTimer_Elapsed;

            Settings.PropertyChanged += Settings_Changed;

            ResetRefreshTimer();
        }

        ~ChatRoomsViewModel()
        {
            _refreshTimer.Elapsed -= RefreshTimer_Elapsed;
            _refreshTimer.Dispose();

            Settings.PropertyChanged -= Settings_Changed;
        }

        #endregion

        #region properties

        public int Page
        {
            get { return _page; }
            private set
            {
                _page = value;
                RaisePropertyChanged(nameof(Page));
            }
        }

        public SettingsModel Settings
        {
            get { return Shared.Instance.Settings; }
        }

        public IList<ChatRoomModel> ChatRooms
        {
            get { return _chatRooms; }
            set
            {
                _chatRooms = value;
                RaisePropertyChanged(nameof(ChatRooms));
            }
        }

        public Gender RoomsGender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(RoomsGender));
            }
        }

        public ChatRoomModel SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                RaisePropertyChanged(nameof(SelectedRoom));
                Mediator.Instance.RaiseNotification(this, NotificationType.ChatRoomSelected, SelectedRoom);
            }
        }

        #endregion

        #region commands

        public ICommand GetChatRoomsCommand
        {
            get
            {
                if (_getChatRooms == null)
                    _getChatRooms = new RelayCommand<NavigationDirection, IList<ChatRoomModel>>(GetChatRooms, GetChatRoomsCallback);

                return _getChatRooms;
            }
        }

        public ICommand GetVideoFeedUrlCommand
        {
            get
            {
                if (_getVideoFeed == null)
                    _getVideoFeed = new RelayCommand<ChatRoomModel>(GetVideoFeedUrl);

                return _getVideoFeed;
            }
        }

        #endregion

        void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => GetChatRoomsCommand.Execute(NavigationDirection.Refresh)));
        }

        void ResetRefreshTimer()
        {
            if (_refreshTimer.Enabled)
                _refreshTimer.Stop();

            if (Shared.Instance.Settings.IsAutoRefreshEnabled)
            {
                _refreshTimer.Interval = Shared.Instance.Settings.RefreshInterval * 60 * 1000;
                _refreshTimer.Start();
            }
        }

        void Settings_Changed(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "RefreshInterval":
                case "IsAutoRefreshEnabled":
                    ResetRefreshTimer();
                    break;
            }

        }

        void GetChatRoomsCallback(IList<ChatRoomModel> chatRooms)
        {
            ChatRooms = chatRooms;
            RaisePropertyChanged(nameof(Page));
        }

        IList<ChatRoomModel> GetChatRooms(NavigationDirection parameter)
        {
            var chatRooms = new List<ChatRoomModel>();

            switch (parameter)
            {
                case NavigationDirection.Forward:
                    Page += 1;
                    break;

                case NavigationDirection.Backward:
                    if (Page > 1)
                        Page -= 1;
                    break;

                case NavigationDirection.Refresh:
                default:
                    if (Page == 0)
                        Page = 1;
                    break;
            }

            var url = string.Format("{0}/?page={1}", CHATURBATE, Page);
            switch (RoomsGender)
            {
                case Gender.Male:
                    url = string.Format("{0}/male-cams/?page={1}", CHATURBATE, Page);
                    break;

                case Gender.Female:
                    url = string.Format("{0}/female-cams/?page={1}", CHATURBATE, Page);
                    break;

                case Gender.Couple:
                    url = string.Format("{0}/couple-cams/?page={1}", CHATURBATE, Page);
                    break;

                case Gender.Trans:
                    url = string.Format("{0}/trans-cams/?page={1}", CHATURBATE, Page);
                    break;
            }

            string html;
            using (var client = new ExtendedWebClient())
            {
                client.Host = HOST;
                try
                {
                    html = client.DownloadString(new Uri(url, UriKind.Absolute));
                }
                catch (WebException)
                {
                    return chatRooms;
                }
            }

            var parser = new HtmlDocument();
            parser.LoadHtml(html);

            HtmlNode tempNode;
            foreach (var node in parser.DocumentNode.SelectNodes("//ul[@class='list']/li"))
            {
                tempNode = node.SelectSingleNode("./a/img[@class='png']");
                var profileImageUrl = tempNode.Attributes["src"].Value;
                var profileImageHeight = int.Parse(tempNode.Attributes["height"].Value);
                var profileImageWidth = int.Parse(tempNode.Attributes["width"].Value);

                tempNode = node.SelectSingleNode("./div[contains(@class,'thumbnail_label')]");
                var isVideoFeedHd = tempNode.InnerText.Equals("HD", StringComparison.InvariantCultureIgnoreCase);

                tempNode = node.SelectSingleNode("./div/ul[@class='subject']/li");
                var title = tempNode.Attributes["title"].Value;

                tempNode = node.SelectSingleNode("./div[@class='details']/div[@class='title']/a");
                var name = tempNode.InnerText.Trim();
                var profileUrl = string.Format("{0}{1}", CHATURBATE, tempNode.Attributes["href"].Value);

                tempNode = node.SelectSingleNode("./div[@class='details']/div[@class='title']/span[contains(@class,'age')]");
                int.TryParse(tempNode.InnerText, out int age);
                var genderString = tempNode.Attributes["class"].Value;
                Gender gender;
                if (genderString.Contains("genderm"))
                    gender = Gender.Male;
                else if (genderString.Contains("genderf"))
                    gender = Gender.Female;
                else if (genderString.Contains("genderc"))
                    gender = Gender.Couple;
                else
                    gender = Gender.Trans;

                tempNode = node.SelectSingleNode("./div[@class='details']/ul[@class='sub-info']/li[@class='cams']");
                var cams = tempNode.InnerText;

                var chatRoom = new ChatRoomModel(name, age, gender, title, profileUrl, profileImageUrl, profileImageHeight, profileImageWidth);
                chatRoom.IsVideoFeedHd = isVideoFeedHd;
                chatRoom.CamsCount = cams;
                chatRooms.Add(chatRoom);
            }

            return chatRooms;
        }

        void GetVideoFeedUrl(ChatRoomModel parameter)
        {
            if (parameter == null)
                return;

            if (string.IsNullOrEmpty(parameter.VideoFeedUrl))
            {
                const string VIDEO_URL_START = "initHlsPlayer(jsplayer, '";
                const string VIDEO_URL_END = "');";

                using (var webClient = new ExtendedWebClient())
                {
                    string html;
                    try
                    {
                        html = webClient.DownloadString(new Uri(parameter.ProfileUrl, UriKind.Absolute));
                    }
                    catch (WebException)
                    {
                        return;
                    }

                    if (string.IsNullOrEmpty(html))
                        return;

                    var start = html.IndexOf(VIDEO_URL_START);
                    if (start > -1)
                    {
                        var end = html.IndexOf(VIDEO_URL_END, start + VIDEO_URL_START.Length);
                        var url = html.Substring(start + VIDEO_URL_START.Length, end - start - VIDEO_URL_START.Length);
                        parameter.VideoFeedUrl = url;
                        SelectedRoom = parameter;
                    }
                }
            }
            else
                SelectedRoom = parameter;
            
        }

        public void NotificationReceived(NotificationType type, params object[] data)
        {
            // do nothing
        }
    }
}
