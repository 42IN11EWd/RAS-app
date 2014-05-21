using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Threading;

namespace RunApproachStatistics.ViewModel
{
    class VideoPlaybackViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        
        private VideoViewModel videoView;
        private GraphViewModel graphView;

        private vault selectedVault;

        private DispatcherTimer timer;
        private bool dragging = false;

        private string totalTime;
        private string currentTime;
        private double videoPosition;

        #region DataBinding

        public PropertyChangedBase Menu
        {
            get { return menu; }
            set
            {
                menu = value;
                OnPropertyChanged("Menu");
            }
        }

        public VideoViewModel VideoView
        {
            get { return videoView; }
            set
            {
                videoView = value;
                OnPropertyChanged("VideoView");
            }
        }

        public GraphViewModel GraphView
        {
            get { return graphView; }
            set
            {
                graphView = value;
                OnPropertyChanged("GraphView");
            }
        }

        public string CurrentTime
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

        public double VideoPosition
        {
            get { return videoPosition; }
            set 
            {
                videoPosition = value;
                OnPropertyChanged("VideoPosition");
            }
        }
        #endregion

        public VideoPlaybackViewModel(IApplicationController app)
            : base()
        {
            _app = app;
            
            // Set menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = true;
            Menu = menuViewModel;

            
            // Set Graph
            GraphViewModel graphVM = new GraphViewModel(_app, this, 10,2000);
            GraphView = graphVM;
        }

        public void setVaultToPlay(vault selectedVault)
        {
            this.selectedVault = selectedVault;
            // Set ReplayVideo
            VideoView = new VideoViewModel(_app, selectedVault.videopath);
        }

        protected override void initRelayCommands()
        {
        }
    }
}
