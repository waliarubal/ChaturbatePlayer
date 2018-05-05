namespace ChaturbatePlayer.Base
{
    public enum NotificationType: byte
    {
        ChatRoomSelected,
        LicenseChanged,
        TabPageChanged,
        Initialized
    }

    interface IMediator
    {
        void NotificationReceived(NotificationType type, params object[] data);
    }
}
