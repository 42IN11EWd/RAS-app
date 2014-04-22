using RunApproachStatistics.Controllers;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RunApproachStatistics.ViewModel
{
    class ThumbnailViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        public ThumbnailViewModel(IApplicationController app)
            : base()
        {
            _app = app;
        }

        private string name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string datetime;
        public String Datetime
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Datetime");
            }
        }

        private string vaultnumber;
        public String VaultNumber
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        private Brush selectionBackground;
        public Brush SelectionBackground
        {
            get { return selectionBackground; }
            set
            {
                selectionBackground = value;
                OnPropertyChanged("SelectionBackground");
            }
        }

        private Visibility liveVaultText;
        public Visibility LiveVaultText
        {
            get { return liveVaultText; }
            set
            {
                liveVaultText = value;
                OnPropertyChanged("LiveVaultText");
            }
        }

        public RelayCommand VaultClickCommand { get; private set; }

        public void setTypeVault(Boolean isLiveVault)
        {
            LiveVaultText = (isLiveVault ? Visibility.Hidden : Visibility.Visible);
        }

        public void toggleSelection(String typeOfSelection)
        {
            if (typeOfSelection.Equals("Missing"))
            {
                SelectionBackground = Brushes.Red;
            }
            else if (typeOfSelection.Equals("Select"))
            {
                SelectionBackground = Brushes.LightBlue;
            }
            else
            {
                SelectionBackground = Brushes.Transparent;
            }
        }

        public void VaultClick(object commandParam)
        {
            toggleSelection("Select");
        }

        protected override void initRelayCommands()
        {
            VaultClickCommand = new RelayCommand(VaultClick);
        }
    }
}