using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace RunApproachStatistics.ViewModel
{
    public class VideoViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private BitmapImage pauseImage = new BitmapImage(new Uri(@"/Images/videoControl_pause.png", UriKind.Relative));
        private BitmapImage playImage = new BitmapImage(new Uri(@"/Images/videoControl_play.png", UriKind.Relative));
        private BitmapImage playButtonImage; 
        
        private MediaElement _video;
        private DispatcherTimer timer;

        private bool dragging = false;
        private bool isPlaying;
        private Double currentPosition;
        private Double maximum;
        private Double playbackSpeed;
        private String currentTime;
        private String totalTime;
        private String playbackSpeedString;

        public Boolean IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                if (value)
                {
                    PlayButtonImage = pauseImage;
                }
                else
                {
                    PlayButtonImage = playImage;
                }
            }
        }

#region DataBinding
        public RelayCommand PlayClickCommand { get; private set; }

        public RelayCommand StopClickCommand { get; private set; }

        public RelayCommand ForwardClickCommand { get; private set; }

        public RelayCommand BackwardClickCommand { get; private set; }
        
        public RelayCommand MouseUpCommand { get; private set; }

        public RelayCommand MouseDownCommand { get; private set; }

        public MediaElement Video
        {
            get { return _video; }
            set
            {
                _video = value;
                OnPropertyChanged("Video");
            }
        }

        public String PlaybackSpeedString
        {
            get { return playbackSpeedString; }
            set { playbackSpeedString = value;
            OnPropertyChanged("PlaybackSpeedString");
            }
        }       

        public Double PlaybackSpeed
        {
            get { return playbackSpeed; }
            set
            {
                playbackSpeed = value;
                Video.SpeedRatio = value;
                PlaybackSpeedString = Math.Round(playbackSpeed, 2).ToString("0.00", CultureInfo.InvariantCulture);
                OnPropertyChanged("PlaybackSpeed");
            }
        }

        public Double CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                if (dragging)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(value);
                    Video.Position = ts;
                }
                OnPropertyChanged("CurrentPosition");
            }
        }

        public Double Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;

                OnPropertyChanged("Maximum");
            }
        }

        public BitmapImage PlayButtonImage
        {
            get { return playButtonImage; }
            set
            {
                playButtonImage = value;
                OnPropertyChanged("PlayButtonImage");
            }
        }

        public String CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        public String TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }

#endregion

        public VideoViewModel(IApplicationController app, string videoPath)
            : base()
        {
            Console.WriteLine("VideoViewModel");
            _app = app;
            IsPlaying = false;
            Video = new MediaElement
            {
                VerticalAlignment = VerticalAlignment.Stretch,
                Stretch = System.Windows.Media.Stretch.Fill,
                ScrubbingEnabled = true,
                LoadedBehavior = MediaState.Manual
            };
            String desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            Video.Source = new Uri(desktopPath + "\\" +  videoPath);
            Video.Loaded += Video_Loaded;
            Video.MediaEnded += Video_Ended;

            PlaybackSpeed = 1;
            CurrentTime = MillisecondsToTimespan(0);
        }

        /// <summary>
        /// Show first frame from video, init Times and start timer to update slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Video_Loaded(object sender, RoutedEventArgs e)
        {
            // Play and pause to show first frame instead of black screen
            Video.Play();
            Video.Pause();           
            // Set TotalTime
            while (!Video.NaturalDuration.HasTimeSpan)
            {
            }
            Maximum = Video.NaturalDuration.TimeSpan.TotalMilliseconds;
            TotalTime = MillisecondsToTimespan(Maximum);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// Stop video when it has reached its end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Video_Ended(object sender, RoutedEventArgs e)
        {
            Stop();            
        }

        /// <summary>
        /// Play or pause video when play/pause button is pressed
        /// </summary>
        /// <param name="commandParam"></param>
        public void PlayMedia(object commandParam)
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
            IsPlaying = !IsPlaying;
        }

        /// <summary>
        /// Stop video when stop button has been pressed
        /// </summary>
        /// <param name="commandParam"></param>
        public void StopMedia(object commandParam)
        {
            Stop();
        }
             
        /// <summary>
        /// Move to next frame when forward button has been pressed
        /// </summary>
        /// <param name="commandParam"></param>
        public void ForwardMedia(object commandParam)
        {
            IsPlaying = false;
            Pause();
            Double next = CurrentPosition + 40;
            if (next > Maximum)
            {
                next = Maximum;
            }
            dragging = true;
            CurrentPosition = next;
            dragging = false;
        }

        /// <summary>
        /// Move to previous frame when backward button has been pressed
        /// </summary>
        /// <param name="commandParam"></param>
        public void BackwardMedia(object commandParam)
        {
            IsPlaying = false;
            Pause();
            Double prev = CurrentPosition - 40;
            if (prev < 0)
            {
                prev = 0;
            }
            dragging = true;
            CurrentPosition = prev;
            dragging = false;
        }

        /// <summary>
        /// Pause video when slider is 'grabbed'
        /// </summary>
        /// <param name="commandParam"></param>
        public void MouseDown(object commandParam)
        {
            Pause();
            dragging = true;
        }

        /// <summary>
        /// Unpause video if video was playing and slider has been released
        /// </summary>
        /// <param name="commandParam"></param>
        public void MouseUp(object commandParam)
        {
            dragging = false;
            if (IsPlaying) { Play(); }
        }

        public void Play()
        {
            Video.Play();
        }

        public void Pause()
        {
            Video.Pause();
        }

        public void Stop()
        {
            IsPlaying = false;
            Video.Stop();
        }

        /// <summary>
        /// Convert milliseconds to a time string
        /// </summary>
        /// <param name="ms"></param>
        /// <returns>00:00:000 formatted string</returns>
        private String MillisecondsToTimespan(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}:{1:D2}:{2:D3}",
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
        }

        /// <summary>
        /// Update slider while video is playing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            // Check if the movie finished calculate it's total time
            if (Video.NaturalDuration.HasTimeSpan)
            {
                if (Video.NaturalDuration.TimeSpan.TotalMilliseconds > 0)
                {
                    if (!dragging)
                    {
                        Double position = Video.Position.TotalMilliseconds;
                        // Updating time slider
                        CurrentPosition = position;
                    }
                    CurrentTime = MillisecondsToTimespan(CurrentPosition);
                }
            }
        }

        protected override void initRelayCommands()
        {
            PlayClickCommand = new RelayCommand(PlayMedia);
            StopClickCommand = new RelayCommand(StopMedia);
            ForwardClickCommand = new RelayCommand(ForwardMedia);
            BackwardClickCommand = new RelayCommand(BackwardMedia);
            MouseUpCommand = new RelayCommand(MouseUp);
            MouseDownCommand = new RelayCommand(MouseDown);
        }
    }
}
