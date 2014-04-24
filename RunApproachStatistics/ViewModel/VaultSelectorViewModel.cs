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
    public class VaultSelectorViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        private PropertyChangedBase ratingControl;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

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

        public PropertyChangedBase RatingControl
        {
            get { return ratingControl; }
            set
            {
                ratingControl = value;
                OnPropertyChanged("RatingControl");
            }
        }

        #endregion

        public VaultSelectorViewModel(IApplicationController app) : base()
        {
            _app = app;

            // Set menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = false;
            Menu = menuViewModel;

            RatingViewModel ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
