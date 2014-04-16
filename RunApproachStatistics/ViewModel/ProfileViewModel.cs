using RunApproachStatistics.Controllers;
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
    public class ProfileViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;

        #region Modules

        private IUserModule userModule = new UserModule();
        
        #endregion

        #region DataBinding

        public PropertyChangedBase Menu
        {
            get { return menu; }
            set
            {
                menu = value;
                OnPropertyChanged("Menu");
            }
        }

        #endregion

        public ProfileViewModel(IApplicationController app) : base()
        {
            _app = app;

            // Set the menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = false;
            Menu = menuViewModel;

            // Get the user profile
            userModule.read();
        }

        protected override void initRelayCommands()
        {
        }
    }
}
