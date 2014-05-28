using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public abstract class AbstractViewModel : PropertyChangedBase
    {
        private PropertyChangedBase menu;

        public PropertyChangedBase Menu
        {
            get { return menu; }
            set
            {
                menu = value;
                OnPropertyChanged("Menu");
            }
        }
        public AbstractViewModel()
        {
            initRelayCommands();
        }

        protected abstract void initRelayCommands();
    }
}
