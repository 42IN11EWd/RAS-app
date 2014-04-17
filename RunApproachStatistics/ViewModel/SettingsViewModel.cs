using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
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
        }

        public void openVideoSource(CameraWindow cameraWindow)
        {
            WindowsFormsHost host = new WindowsFormsHost();

            //host.Child = cameraWindow;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
