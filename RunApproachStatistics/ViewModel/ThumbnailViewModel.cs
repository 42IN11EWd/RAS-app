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
    public class ThumbnailViewModel : AbstractViewModel
    {
        private IApplicationController _app;

        public ThumbnailViewModel(IApplicationController app)
            : base()
        {
            _app = app;
        }

        private int vault_id;
        public int Vault_id
        {
            get { return vault_id; }
            set
            {
                vault_id = value;
                OnPropertyChanged("Vault_id");
            }
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
        private int starrating;
        public int StarRating
        {
            get { return starrating; }
            set
            {
                starrating = value;
                OnPropertyChanged("StarRating");
            }
        }
        private string gymnast;
        public String Gymnast
        {
            get { return gymnast; }
            set
            {
                gymnast = value;
                OnPropertyChanged("Gymnast");
            }
        }
        private string timespan;
        public String Timespan
        {
            get { return timespan; }
            set
            {
                timespan = value;
                OnPropertyChanged("Timespan");
            }
        }
        private string location;
        public String Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }
        private string dScore;
        public String DScore
        {
            get { return dScore; }
            set
            {
                dScore = value;
                OnPropertyChanged("DScore");
            }
        }
        private string eScore;
        public String EScore
        {
            get { return eScore; }
            set
            {
                eScore = value;
                OnPropertyChanged("EScore");
            }
        }
        private string penalty;
        public String Penalty
        {
            get { return penalty; }
            set
            {
                penalty = value;
                OnPropertyChanged("Penalty");
            }
        }
        private string totalscore;
        public String TotalScore
        {
            get { return totalscore; }
            set
            {
                totalscore = value;
                OnPropertyChanged("TotalScore");
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

        protected override void initRelayCommands()
        {

        }
    }
}