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
        string _name, _profileUrl, _profileImageUrl, _roomTitle, _videoFeedUrl, _camsCount;
        int _height, _width, _age;
        Gender _gender;
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

        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
        }

        public int Age
        {
            get => _age;
            set
            {
                Set(nameof(Age), ref _age, value);
            }
        }

        public Gender Gender
        {
            get => _gender;
            set
            {
                Set(nameof(Gender), ref _gender, value);
            }
        }

        public string ProfileUrl
        {
            get => _profileUrl;
            set
            {
                Set(nameof(ProfileUrl), ref _profileUrl, value);
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                Set(nameof(Height), ref _height, value);
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                Set(nameof(Width), ref _width, value);
            }
        }

        public string ProfileImageUrl
        {
            get => _profileImageUrl;
            set
            {
                Set(nameof(ProfileImageUrl), ref _profileImageUrl, value);
            }
        }

        public string RoomTitle
        {
            get => _roomTitle;
            set
            {
                Set(nameof(RoomTitle), ref _roomTitle, value);
            }
        }

        public string VideoFeedUrl
        {
            get => _videoFeedUrl;
            set
            {
                Set(nameof(VideoFeedUrl), ref _videoFeedUrl, value);
            }
        }

        public bool IsVideoFeedHd
        {
            get => _isVideoFeedHd;
            set
            {
                Set(nameof(IsVideoFeedHd), ref _isVideoFeedHd, value);
            }
        }

        public string CamsCount
        {
            get => _camsCount;
            set
            {
                Set(nameof(CamsCount), ref _camsCount, value);
            }
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
