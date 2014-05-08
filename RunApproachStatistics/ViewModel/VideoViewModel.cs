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
            
            //TimeSlider.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(timeSlider_MouseLeftButtonUp), true);

            //   TimeSlider.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(timeSlider_MouseLeftButtonDown), true);
            //   IsPlaying(false);

            //   btnPlay.IsEnabled = true;
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
             TotalTime = MillisecondsToTimespan(Video.NaturalDuration.TimeSpan.TotalMilliseconds);
            //}
            //else
            //{
            //    TotalTime = MillisecondsToTimespan(0);
            
        }

        private void timeSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
           
            //VideoControl.Pause();
        }

        //private void timeSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (VideoControl.NaturalDuration.HasTimeSpan)
        //    {
        //        if (VideoControl.NaturalDuration.TimeSpan.TotalSeconds > 0)
        //        {
        //            // VideoControl.Pause();
        //            VideoControl.Position = TimeSpan.FromMilliseconds(TimeSlider.Value);
        //        }
        //    }
        //    dragging = false;
        //    //VideoControl.Play();
        //}

        //private void IsPlaying(bool flag)
        //{
        //    btnPlay.IsEnabled = flag;
        //    btnMoveBack.IsEnabled = flag;
        //    btnMoveForward.IsEnabled = flag;
        //}


        public void PlayMedia(object commandParam)
        {
            if (IsPlaying)
            {
                Video.SpeedRatio = 1;
                Video.Pause();
            }
            else
            {

                Video.SpeedRatio = 1;
                Video.Play();
            }
            IsPlaying = !IsPlaying;
        }

        public void ForwardMedia(object commandParam)
        {
            IsPlaying = true;
            Video.SpeedRatio = 0.5;
            Video.Play();
        }

        public void BackwardMedia(object commandParam)
        {
            //IsPlaying = true;
            //Video.SpeedRatio = -2;
            //Video.Play();
        }

            private void Element_MediaOpened(object sender, EventArgs e)
        {


            //if (VideoControl.NaturalDuration.HasTimeSpan)
            //{
            //    TimeSlider.Maximum = VideoControl.NaturalDuration.TimeSpan.TotalMilliseconds;
            //    TimeSlider.SmallChange = 100;
            //    TimeSlider.LargeChange = Math.Min(1000, VideoControl.NaturalDuration.TimeSpan.Milliseconds / 10);
            //    TotalTimetext.Text = MillisecondsToTimespan(TimeSlider.Maximum);
            //}

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            //timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        // When the media playback is finished. Stop() the media to seek to media start. 
        //private void Element_MediaEnded(object sender, EventArgs e)
        //{
        //    //VideoControl.Stop();
        //}
         // Jump to different parts of the media (seek to).  
        //private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        //{
        //    int SliderValue = (int)TimeSlider.Value;

        //    TimeSpan ts = TimeSpan.FromMilliseconds(SliderValue);
        //    VideoControl.Position = ts;
        //}
        
        private String MillisecondsToTimespan(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}:{1:D2}:{2:D3}",
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
        }

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    // Check if the movie finished calculate it's total time
        //    if (VideoControl.NaturalDuration.HasTimeSpan)
        //    {
        //        if (VideoControl.NaturalDuration.TimeSpan.TotalMilliseconds > 0)
        //        {
        //            if (!dragging)
        //            {
        //                Double position = VideoControl.Position.TotalMilliseconds;
        //                // Updating time slider
        //                TimeSlider.Value = position;
        //            }
        //            Timetext.Text = MillisecondsToTimespan(TimeSlider.Value);
        //        }
        //    }
        //}

        

        protected override void initRelayCommands()
        {
            PlayClickCommand = new RelayCommand(PlayMedia);
            ForwardClickCommand = new RelayCommand(ForwardMedia);
            BackwardClickCommand = new RelayCommand(BackwardMedia);
        }

    }
}
