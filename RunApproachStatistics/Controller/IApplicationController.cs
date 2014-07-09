using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        void TogglePilotLaser();

        void SetPilotLaserToNull();

        bool isPilotLaserOn();
        void ShowMainScreen();

        void ShowHomeView();

        void ShowCameraView();

        void ShowCompareVaultsView(List<vault> vaults);

        void ShowVideoPlaybackView(vault selectedVault);

        void ShowLoginView();

        void ShowMeasurementView();

        void ShowPostMeasurementView(ObservableCollection<ThumbnailViewModel> newThumbnailCollection);

        void ShowProfileView();

        void ShowSettingsView();

        void ShowVaultSelectorView();

        void ShowVaultSelectorView(String filterItem, String filterType);

        void RefreshThumbnailCollection();

        void ShowLocationEditorView();

        void ShowVaultNumberEditorView();

        void ShowVaultKindEditorView();

        void ToggleLockScreen();

        void CloseLoginWindow(object sender = null, EventArgs e = null);

        void CloseSettingsWindow(Boolean isClosed = false);

        Boolean IsLoggedIn { get; set; }

        Boolean IsLoginWindowOpen { get; set; }
    }
}
