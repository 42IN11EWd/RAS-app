using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;

namespace RunApproachStatistics.ViewModel
{
    public class CameraViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        private WindowsFormsHost cameraHost;
        public WindowsFormsHost CameraHost
        {
            get { return cameraHost; }
            set
            {
                cameraHost = value;
                OnPropertyChanged("CameraHost");
            }
        }

        #region Modules

        private ICameraModule cameraModule = new VaultModule();

        #endregion

        public CameraViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
