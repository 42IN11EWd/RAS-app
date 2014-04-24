using RunApproachStatistics.Controllers;
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

namespace RunApproachStatistics.ViewModel
{
    class HomeViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;

        private CameraViewModel cameraView;
        private CameraWindow cameraWindow;
        private VideoCameraController videoCameraController;
        private IVideoCameraSettingsModule videoCameraSettingsModule;

        public VideoCameraController VideoCameraController
        {
            get { return videoCameraController; }
            set { }
        }

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

        public CameraViewModel CameraView
        {
            get { return cameraView; }
            set
            {
                cameraView = value;
                OnPropertyChanged("CameraView");
            }
        }
        #endregion

        public HomeViewModel(IApplicationController app, VideoCameraController videoCameraController) : base()
        {
            _app = app;

            // Set menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = true;
            Menu = menuViewModel;

            // Set VideoCamera
            CameraView = new CameraViewModel(_app);
            this.videoCameraController = videoCameraController;
            openVideoSource();
        }

        private void openVideoSource()
        {
            cameraWindow = videoCameraController.CameraWindow;

            if (cameraWindow != null)
            {
                setVideoSource(cameraWindow);
            }
        }

        /// <summary>
        /// Pause the video source so it can be used in another window
        /// </summary>
        /// <param name="pause">True if the video should be paused</param>
        public void pauseVideoSource(Boolean pause)
        {
            if (pause)
            {
                System.Windows.Forms.Label pauseCameraLabel = new System.Windows.Forms.Label();
                pauseCameraLabel.Text = "Camera is being used by a different window";
                pauseCameraLabel.ForeColor = Color.White;
                pauseCameraLabel.AutoSize = false;
                pauseCameraLabel.Dock = System.Windows.Forms.DockStyle.Fill;
                pauseCameraLabel.TextAlign = ContentAlignment.MiddleCenter;

                WindowsFormsHost host = new WindowsFormsHost();
                host.Child = pauseCameraLabel;
                cameraView.CameraHost = host;
            }
            else
            {
                setVideoSource(cameraWindow);
            }
        }

        private void setVideoSource(CameraWindow cameraWindow)
        {
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = cameraWindow;
            cameraView.CameraHost = host;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
