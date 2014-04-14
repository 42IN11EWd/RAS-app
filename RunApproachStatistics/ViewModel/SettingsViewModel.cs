using RunApproachStatistics.Controllers;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class SettingsViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase content;

        #region DataBinding

        public PropertyChangedBase Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        #endregion

        public SettingsViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        protected override void initRelayCommands()
        {
        }
    }
}
