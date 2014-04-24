using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.Controllers
{
    public interface IApplicationController
    {
        /// <summary>
        /// Show the main screen
        /// </summary>
        void ShowMainScreen();

        void ShowHomeView();

        void ShowCameraView();

        void ShowCompareVaultsView();

        void ShowLoginView();

        void ShowMeasurementView();

        void ShowPostMeasurementView();

        void ShowProfileView();

        void ShowSettingsView();

        void ShowVaultSelectorView();

        void CloseLoginWindow();

        void CloseSettingsWindow();
    }
}
