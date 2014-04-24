using RunApproachStatistics.Controllers;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    class MenuViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        #region DataBinding

        public String PilotLaserTitle { get; set; }
        public Boolean VisibilityLaser { get; set; }
        public Boolean ToggleLaser { get; set; }
        public RelayCommand SetPilotLaserCommand { get; private set; }
        public RelayCommand StartSessionCommand { get; private set; }
        public RelayCommand GymnastProfilesCommand { get; private set; }
        public RelayCommand SelectVaultCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand HomeCommand { get; private set; }

        #endregion

        public MenuViewModel(IApplicationController app) : base()
        {
            _app = app;
            PilotLaserTitle = "Set Pilot Laser On";
        }

        #region RelayCommands

        public void SetPilotLaser(object commandParam)
        {
            if (!ToggleLaser) 
            {
                PilotLaserTitle = "Set Pilot Laser Off";
            } 
            else 
            {
                PilotLaserTitle = "Set Pilot Laser On";
            }
            ToggleLaser = !ToggleLaser;

            OnPropertyChanged("PilotLaserTitle");
            _app.ShowCompareVaultsView();
        }
        public void StartSession(object commandParam)
        {
            _app.ShowMeasurementView();
        }
        public void LoadGymnastProfiles(object commandParam)
        {
            _app.ShowProfileView();
        }
        public void LoadSelectVault(object commandParam)
        {
            _app.ShowVaultSelectorView();
        }
        public void LoadSettings(object commandParam)
        {
            _app.ShowSettingsView();
        }
        public void LoadHomeScreen(object commandParam)
        {
            _app.ShowHomeView();
        }

        #endregion

        protected override void initRelayCommands()
        {
            SetPilotLaserCommand = new RelayCommand(SetPilotLaser);
            StartSessionCommand = new RelayCommand(StartSession);
            GymnastProfilesCommand = new RelayCommand(LoadGymnastProfiles);
            SelectVaultCommand = new RelayCommand(LoadSelectVault);
            SettingsCommand = new RelayCommand(LoadSettings);
            HomeCommand = new RelayCommand(LoadHomeScreen);
        }
    }
}
