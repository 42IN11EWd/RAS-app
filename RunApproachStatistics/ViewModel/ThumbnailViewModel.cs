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
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }

        private string vaultnumber;
        public String VaultNumber
        {
            get { return vaultnumber; }
            set
            {
                vaultnumber = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        private System.Drawing.Brush selectionBackground;
        public System.Drawing.Brush SelectionBackground
        {
            get { return selectionBackground; }
            set
            {
                selectionBackground = value;
                OnPropertyChanged("SelectionBackground");
            }
        }

        private ImageSource thumbnailImage;
        public ImageSource ThumbnailImage
        {
            get { return thumbnailImage; }
            set
            {
                thumbnailImage = value;
                OnPropertyChanged("ThumbnailImage");
                OnPropertyChanged("HasThumnailImage");
            }
        }

        public Visibility HasThumnailImage
        {
            get { return ThumbnailImage != null ? Visibility.Visible : Visibility.Hidden; }
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
                SelectionBackground = System.Drawing.Brushes.Red;
            }
            else if (typeOfSelection.Equals("Select"))
            {
                SelectionBackground = System.Drawing.Brushes.LightBlue;
            }
            else
            {
                SelectionBackground = System.Drawing.Brushes.Transparent;
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