using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private BitmapImage pauseImage = new BitmapImage(new Uri(@"/Images/videoControl_pause.png", UriKind.Relative));
        private BitmapImage playImage = new BitmapImage(new Uri(@"/Images/videoControl_play.png", UriKind.Relative));
        private IApplicationController _app;
        private MediaElement _video;

        public MediaElement Video
        {
            get { return _video; }
            set { 
                _video = value;
                OnPropertyChanged("Video");
            }
        }
        DispatcherTimer timer;



        private BitmapImage playButtonImage;

        private Boolean isPlaying;

        public Boolean IsPlaying
        {
            get { return isPlaying; }
            set { 
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
        
        private String currentTime;
        private String totalTime;
        private Double currentPosition;

        public Double CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value;
            if (dragging)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(value);
                Video.Position = ts;               
            }
                OnPropertyChanged("CurrentPosition");
            }
        }
        private Double maximum;

        public Double Maximum
        {
            get { return maximum; }
            set { maximum = value;

            OnPropertyChanged("Maximum");
            }
        }

        bool dragging = false;

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
            set { currentTime = value;
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


        public RelayCommand PlayClickCommand { get; private set; }

        public RelayCommand ForwardClickCommand { get; private set; }

        public RelayCommand BackwardClickCommand { get; private set; }

        public RelayCommand ScrubbingCommand { get; private set; }

        public RelayCommand MouseUpCommand { get; private set; }

        public RelayCommand MouseDownCommand { get; private set; }

        public VideoViewModel(IApplicationController app) : base()
        
{
            Console.WriteLine("VideoViewModel");
            _app = app;
            IsPlaying = false;
            Video = new MediaElement
            {
                Source = new Uri(@"Movie.avi", UriKind.Relative),
                VerticalAlignment = VerticalAlignment.Stretch,
                Stretch = System.Windows.Media.Stretch.Fill,
                ScrubbingEnabled = true,
                LoadedBehavior = MediaState.Manual            
            };
            Video.Loaded += Video_Loaded;            
            
            CurrentTime = MillisecondsToTimespan(0);
        }

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

                
        public void PlayMedia(object commandParam)
        {
            if (IsPlaying)
            {
                Video.SpeedRatio = 1;
                Pause();
            }
            else
            {
                Video.SpeedRatio = 1;
                Play();
            }
            IsPlaying = !IsPlaying;
        }

        public void Play()
        {
            //IsPlaying = true;
            Video.Play();
            //timer.Start();
        }

        public void Pause()
        {
           // IsPlaying = false;
            Video.Pause();
            //timer.Stop();
        }

        public void Stop()
        {
            IsPlaying = false;
            Video.Stop();
            timer.Stop();
        }

        public void ForwardMedia(object commandParam)
        {
            Video.SpeedRatio = 0.5;
            IsPlaying = true;
            Play();            
        }

        public void BackwardMedia(object commandParam)
        {
            //IsPlaying = true;
            //Video.SpeedRatio = -2;
            //Video.Play();
        }

        public void MouseDown(object commandParam)
        {
            Pause();
            dragging = true;
        }

        public void MouseUp(object commandParam)
        {
            dragging = false;
            if (IsPlaying) { Play(); }
        }
        
        private String MillisecondsToTimespan(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}:{1:D2}:{2:D3}",
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
        }

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
            ForwardClickCommand = new RelayCommand(ForwardMedia);
            BackwardClickCommand = new RelayCommand(BackwardMedia);
            MouseUpCommand = new RelayCommand(MouseUp);
            MouseDownCommand = new RelayCommand(MouseDown);
        }

    }
}
