using ChaturbatePlayer.Base;
using ChaturbatePlayer.ViewModels;
using System;
using System.IO;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;

namespace ChaturbatePlayer.Views
{
    public partial class LiveStreamPlayerView: IMediator
    {
        readonly LiveStreamPlayerViewModel _viewModel;
        string _currentVideoUrl;

        public LiveStreamPlayerView()
        {
            InitializeComponent();

            Mediator.Instance.RegisterColleague(this);

            _viewModel = DataContext as LiveStreamPlayerViewModel;
            _viewModel.State = LiveStreamPlayerState.Stopped;
            _viewModel.PropertyChanged += LiveStreamPlayerView_PropertyChanged;
        }

        ~LiveStreamPlayerView()
        {
            //Player.Dispose();

            _viewModel.PropertyChanged -= LiveStreamPlayerView_PropertyChanged;
        }

        void LiveStreamPlayerView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_viewModel.SelectedRoom == null || Player.SourceProvider.MediaPlayer == null)
                return;

            if (e.PropertyName.Equals("State"))
            {
                switch (_viewModel.State)
                {
                    case LiveStreamPlayerState.Play:
                        ChangePlayerState(_viewModel.SelectedRoom.VideoFeedUrl, LiveStreamPlayerState.Play);
                        break;

                    case LiveStreamPlayerState.Pause:
                        ChangePlayerState(_viewModel.SelectedRoom.VideoFeedUrl, LiveStreamPlayerState.Pause);
                        break;

                    case LiveStreamPlayerState.Stop:
                    case LiveStreamPlayerState.ExternalMediaPlayer:
                        ChangePlayerState(_viewModel.SelectedRoom.VideoFeedUrl, LiveStreamPlayerState.Stop);
                        break;
                }
            }
            else if (e.PropertyName.Equals("IsMuted"))
            {
                Player.SourceProvider.MediaPlayer.Audio.IsMute = _viewModel.IsMuted;
            }
        }

        void MediaPlayer_Stopped(object sender, VlcMediaPlayerStoppedEventArgs e)
        {
            _viewModel.State = LiveStreamPlayerState.Stopped;
        }

        void MediaPlayer_Buffering(object sender, VlcMediaPlayerBufferingEventArgs e)
        {
            _viewModel.State = LiveStreamPlayerState.Playing;
        }

        void MediaPlayer_Opening(object sender, VlcMediaPlayerOpeningEventArgs e)
        {
            _viewModel.State = LiveStreamPlayerState.Playing;
        }

        void MediaPlayer_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            _viewModel.State = LiveStreamPlayerState.Playing;
        }

        void MediaPlayer_EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            _viewModel.State = LiveStreamPlayerState.Stopped;
        }

        void MediaPlayer_Paused(object sender, VlcMediaPlayerPausedEventArgs e)
        {
            _viewModel.State = LiveStreamPlayerState.Paused;
        }

        void ChangePlayerState(string url, LiveStreamPlayerState state)
        {
            switch(state)
            {
                case LiveStreamPlayerState.Play:
                    if (string.IsNullOrEmpty(url))
                    {
                        if (Player.SourceProvider.MediaPlayer.State == MediaStates.Playing)
                            Player.SourceProvider.MediaPlayer.Stop();
                        _currentVideoUrl = null;
                    }
                    else
                    {
                        if (_currentVideoUrl == url)
                        {
                            if (Player.SourceProvider.MediaPlayer.State == MediaStates.Paused)
                                Player.SourceProvider.MediaPlayer.Play();
                        }
                        else
                        {
                            if (Player.SourceProvider.MediaPlayer.State == MediaStates.Playing)
                                Player.SourceProvider.MediaPlayer.Stop();

                            Player.SourceProvider.MediaPlayer.Play(new Uri(url, UriKind.Absolute));
                            _currentVideoUrl = url;
                        }
                    }
                        
                    break;

                case LiveStreamPlayerState.ExternalMediaPlayer:
                    if (Player.SourceProvider.MediaPlayer.State == MediaStates.Playing || Player.SourceProvider.MediaPlayer.State == MediaStates.Paused)
                        Player.SourceProvider.MediaPlayer.Stop();
                    break;

                case LiveStreamPlayerState.Stop:
                    if (Player.SourceProvider.MediaPlayer.State == MediaStates.Playing || Player.SourceProvider.MediaPlayer.State == MediaStates.Paused)
                        Player.SourceProvider.MediaPlayer.Stop();
                    _currentVideoUrl = null;
                    break;

                case LiveStreamPlayerState.Pause:
                    if (Player.SourceProvider.MediaPlayer.State == MediaStates.Playing)
                        Player.SourceProvider.MediaPlayer.Pause();
                    break;
            }
        }

        public void NotificationReceived(NotificationType type, params object[] data)
        {
            switch(type)
            {
                case NotificationType.Initialized:
                    Player.SourceProvider.CreatePlayer(new DirectoryInfo(Shared.Instance.LibVlcPath));
                    Player.SourceProvider.MediaPlayer.Opening += MediaPlayer_Opening;
                    Player.SourceProvider.MediaPlayer.Buffering += MediaPlayer_Buffering;
                    Player.SourceProvider.MediaPlayer.Playing += MediaPlayer_Playing;
                    Player.SourceProvider.MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
                    Player.SourceProvider.MediaPlayer.Stopped += MediaPlayer_Stopped;
                    Player.SourceProvider.MediaPlayer.Paused += MediaPlayer_Paused;
                    break;
            }
        }
    }
}
