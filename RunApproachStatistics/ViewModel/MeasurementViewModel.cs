﻿using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        #region Modules

        private ICameraModule cameraModule = new CameraModule();
        private IMeasurementModule measurementModule = new MeasurementModule();

        #endregion

        #region DataBinding

        public RelayCommand HomeCommand { get; private set; }

        #endregion

        public MeasurementViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        #region RelayCommands

        public void LoadHomeScreen(object commandParam)
        {
            _app.ShowHomeView();
        }

        #endregion

        protected override void initRelayCommands()
        {
            HomeCommand = new RelayCommand(LoadHomeScreen);
        }
    }
}
