using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace RunApproachStatistics.ViewModel
{
    public class SettingsViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase content;

        private CameraViewModel cameraView;
        public  CameraViewModel CameraView
        {
            get { return cameraView; }
            set
            {
                cameraView = value;
                OnPropertyChanged("CameraView");
            }
        }

        private int selectedCameraIndex;
        public int SelectedCameraIndex
        {
            get { return selectedCameraIndex; }
            set
            {
                if (value > -1)
                {
                    selectedCameraIndex = value;
                    OnPropertyChanged("SelectedCameraIndex");
                    selectedCameraIndexChanged();
                }
            }
        }

        private String[] devices;
        public String[] Devices
        {
            get { return devices; }
            set
            {
                devices = value;
                OnPropertyChanged("Devices");
            }
        }

        private VideocameraController videocameraController;

        #region Modules

        private IVideoCameraSettingsModule videoCameraSettingsModule = new SettingsModule();
        private ILaserCameraSettingsModule laserCameraSettingsModule = new SettingsModule();

        #endregion

        #region DataBinding

        public PropertyChangedBase Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        #endregion

        public SettingsViewModel(IApplicationController app) : base()
        {
            _app = app;

            CameraView = new CameraViewModel(_app);
            videocameraController = new VideocameraController();
            Devices = videocameraController.Devices;
        }

        public void selectedCameraIndexChanged()
        {
            CameraWindow cameraWindow = videocameraController.OpenVideoSource(selectedCameraIndex);
            if (cameraWindow == null) { Console.WriteLine("Camera niet gevonden, popup maken");  return; }
            openVideoSource(cameraWindow);
        }

        public void openVideoSource(CameraWindow cameraWindow)
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
