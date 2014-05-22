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
        public String LogName { get; set; }
        public Boolean ToggleLaser { get; set; }
        public RelayCommand SetPilotLaserCommand { get; private set; }
        public RelayCommand StartSessionCommand { get; private set; }
        public RelayCommand GymnastProfilesCommand { get; private set; }
        public RelayCommand SelectVaultCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand HomeCommand { get; private set; }
        public RelayCommand LogCommand { get; private set; }

        #endregion

        public MenuViewModel(IApplicationController app) : base()
        {
            _app = app;
            PilotLaserTitle = "Set Pilot Laser On";
            LogName = "Login";
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

        public void LoginoutCommand(object commandParam)
        {
            if (!_app.IsLoggedIn)
            {
                _app.ShowLoginView();
                while(_app.IsLoginWindowOpen)
                {
                    //wait while window is open
                }

                if (_app.IsLoggedIn)
                {
                    LogName = "Logout";
                    OnPropertyChanged("LogName");
                }
            }
            else
            {
                LogName = "Login";
                OnPropertyChanged("LogName");
                _app.IsLoggedIn = false;
            }
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
            LogCommand = new RelayCommand(LoginoutCommand);
        }
    }
}
