using NullVoidCreations.WpfHelpers.Base;

namespace ChaturbatePlayer.Models
{
    public enum Gender : byte
    {
        Male,
        Female,
        Couple,
        Trans,
        NotSpecified
    }

    public class ChatRoomModel : NotificationBase
    {
        string _camsCount, _videoFeedUrl;
        bool _isVideoFeedHd;

        public ChatRoomModel(
            string name,
            int age,
            Gender gender,
            string roomTitle,
            string profileUrl,
            string profileImageUrl,
            int height,
            int width)
        {
            Name = name;
            Age = age;
            Gender = gender;
            RoomTitle = roomTitle;
            ProfileUrl = profileUrl;
            ProfileImageUrl = profileImageUrl;
            Height = height;
            Width = width;
        }

        #region properties

        public string Name { get; }

        public int Age { get; }

        public Gender Gender { get; }

        public string ProfileUrl { get; }

        public int Height { get; }

        public int Width { get; }

        public string ProfileImageUrl { get; }

        public string RoomTitle { get; }

        public string VideoFeedUrl
        {
            get => _videoFeedUrl;
            set => Set(nameof(VideoFeedUrl), ref _videoFeedUrl, value);
        }

        public bool IsVideoFeedHd
        {
            get => _isVideoFeedHd;
            set =>  Set(nameof(IsVideoFeedHd), ref _isVideoFeedHd, value);
        }

        public string CamsCount
        {
            get => _camsCount;
            set =>  Set(nameof(CamsCount), ref _camsCount, value);
        }

        #endregion

        #region methods

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var compareWith = obj as ChatRoomModel;
            if (compareWith == null)
                return false;

            return compareWith.Name.Equals(Name);
        }

        #endregion
    }
}
