using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            // Add loading label
            Label loadingLabel      = new Label();
            loadingLabel.Text       = "Camera is being loaded...";
            loadingLabel.ForeColor  = Color.White;
            loadingLabel.AutoSize   = false;
            loadingLabel.Dock       = DockStyle.Fill;
            loadingLabel.TextAlign  = ContentAlignment.MiddleCenter;

            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = loadingLabel;
            CameraHost = host;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
