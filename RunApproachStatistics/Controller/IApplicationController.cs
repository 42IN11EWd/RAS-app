﻿using System;
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

        void ShowVideoPlaybackView();

        void ShowLoginView();

        void ShowMeasurementView();

        void ShowPostMeasurementView();

        void ShowProfileView();

        void ShowSettingsView();

        void ShowVaultSelectorView();

        void ShowVaultSelectorView(RunApproachStatistics.Model.Entity.gymnast gymnast);

        void ShowLocationEditorView();

        void ShowVaultNumberEditorView();

        void ToggleLockScreen();

        void CloseLoginWindow(object sender = null, EventArgs e = null);

        void CloseSettingsWindow(Boolean isClosed = false);

        Boolean IsLoggedIn { get; set; }

        Boolean IsLoginWindowOpen { get; set; }
    }
}
