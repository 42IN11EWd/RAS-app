using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RunApproachStatistics.View
{
    /// <summary>
    /// Interaction logic for CompareVaultsView.xaml
    /// </summary>
    public partial class CompareVaultsView : UserControl
    {
       
        DispatcherTimer timer;
        bool dragging =false;
        public CompareVaultsView()
        {
            InitializeComponent();
            TimeSlider.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(timeSlider_MouseLeftButtonUp), true);
            
            TimeSlider.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(timeSlider_MouseLeftButtonDown), true);
            IsPlaying(false);

            btnPlay.IsEnabled = true;
        }

        private void timeSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            //VideoControl.Pause();
        }

        private void timeSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (VideoControl.NaturalDuration.HasTimeSpan)
            {
                if (VideoControl.NaturalDuration.TimeSpan.TotalSeconds > 0)
                {
                    // VideoControl.Pause();
                    VideoControl.Position = TimeSpan.FromMilliseconds(TimeSlider.Value);
                }
            }
            dragging = false;
            //VideoControl.Play();
        }

        private void IsPlaying(bool flag)
        {
            btnPlay.IsEnabled = flag;
            btnMoveBack.IsEnabled = flag;
            btnMoveForward.IsEnabled = flag;
        }

       
        void OnMouseDownPlayMedia(object sender, RoutedEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or  
            // resume media if it is paused. This has no effect if the media is 
            // already running.
            if (btnPlay.Content == ">")
            {
                btnPlay.Content = "||";
                VideoControl.Play();
            }
            else
            {
                btnPlay.Content = ">";
                VideoControl.Pause();
            }
            
            // Initialize the MediaElement property values.
            //InitializePropertyValues();

        }
        private void Element_MediaOpened(object sender, EventArgs e)
        {
           
            
            if (VideoControl.NaturalDuration.HasTimeSpan)
            {
                TimeSlider.Maximum = VideoControl.NaturalDuration.TimeSpan.TotalMilliseconds;
                TimeSlider.SmallChange = 100;
                TimeSlider.LargeChange = Math.Min(1000, VideoControl.NaturalDuration.TimeSpan.Milliseconds /10);
                TotalTimetext.Text = MillisecondsToTimespan(TimeSlider.Maximum);
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        // When the media playback is finished. Stop() the media to seek to media start. 
        private void Element_MediaEnded(object sender, EventArgs e)
        {
           //VideoControl.Stop();
        }
        /*// Jump to different parts of the media (seek to).  
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)TimeSlider.Value;

            TimeSpan ts = TimeSpan.FromMilliseconds(SliderValue);
            VideoControl.Position = ts;
        }*/

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
            if (VideoControl.NaturalDuration.HasTimeSpan)
            {
                if (VideoControl.NaturalDuration.TimeSpan.TotalMilliseconds > 0)
                {
                    if (!dragging)
                    {
                        Double position = VideoControl.Position.TotalMilliseconds;
                        // Updating time slider
                        TimeSlider.Value = position;
                    }
                    Timetext.Text = MillisecondsToTimespan(TimeSlider.Value);
                }
            }
        }
    }
}
