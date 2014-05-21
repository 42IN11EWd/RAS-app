using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RunApproachStatistics.ViewModel
{
    public class ThumbnailViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private vault vault = new vault();
        private Visibility liveLabelVisibility, gymnastVisibility;
        private Thickness livePadding;

        public ThumbnailViewModel(IApplicationController app)
            : base()
        {
            _app = app;
            LiveLabelVisibility = Visibility.Hidden;
            LivePadding = new Thickness(5, 0, 5, 0);
            GymnastVisibility = Visibility.Hidden;
        }

        public vault Vault
        {
            get { return vault; }
            set
            {
                vault = value;
                OnPropertyChanged("Vault");
                OnPropertyChanged("Gymnast");
                OnPropertyChanged("Datetime");
                OnPropertyChanged("VaultNumber");

                setThumbnail();
            }
        }

        public Thickness LivePadding
        {
            get { return livePadding; }
            set
            {
                livePadding = value;
                OnPropertyChanged("LivePadding");
            }
        }

        public Visibility LiveLabelVisibility
        {
            get { return liveLabelVisibility; }
            set 
            { 
                liveLabelVisibility = value;
                OnPropertyChanged("LiveLabelVisibility");
            }
        }

        public Visibility GymnastVisibility
        {
            get { return gymnastVisibility; }
            set
            {
                gymnastVisibility = value;
                OnPropertyChanged("GymnastVisibility");
            }
        }

        public String Gymnast
        {
            get 
            {
                if (vault.gymnast != null)
                {
                    return vault.gymnast.name + " " + (vault.gymnast.surname_prefix != null ? vault.gymnast.surname_prefix + " " : "") + vault.gymnast.surname;
                }
                else
                {
                    return "";
                }
            }
        }

        public String Datetime
        {
            get { return vault.timestamp.ToString(); }
        }

        public String VaultNumber
        {
            get { return vault.vaultnumber != null ? vault.vaultnumber.code : ""; }
        }

        private System.Windows.Media.Brush selectionBackground;
        public System.Windows.Media.Brush SelectionBackground
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
                OnPropertyChanged("HasThumbnailImage");
                OnPropertyChanged("NoThumbnailRectangle");
            }
        }

        public Visibility HasThumbnailImage
        {
            get { return ThumbnailImage != null ? Visibility.Visible : Visibility.Hidden; }
        }

        public Visibility NoThumbnailRectangle
        {
            get { return ThumbnailImage == null ? Visibility.Visible : Visibility.Hidden; }
        }

        public void toggleSelection(String typeOfSelection)
        {
            if (typeOfSelection.Equals("Missing"))
            {
                SelectionBackground = System.Windows.Media.Brushes.Red; 
            }
            else if (typeOfSelection.Equals("Select"))
            {
                SelectionBackground = System.Windows.Media.Brushes.LightBlue;
            }
            else
            {
                SelectionBackground = System.Windows.Media.Brushes.Transparent;
            }
        }

        private void setThumbnail()
        {
            if (vault.thumbnail != null)
            {
                try
                {
                    using (var ms = new MemoryStream(vault.thumbnail))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;

                        //dimensies van de image in de xaml
                        image.DecodePixelHeight = 133;
                        image.DecodePixelWidth = 256;

                        image.EndInit();

                        ThumbnailImage = image;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void setLive(Boolean isLive)
        {
            if (isLive)
            {
                LiveLabelVisibility = Visibility.Visible;
                Vault.timestamp = DateTime.Now;
                OnPropertyChanged("Datetime");
                LivePadding = new Thickness(15,0,15,0);
            }
            else
            {
                LiveLabelVisibility = Visibility.Hidden;
            }
        }

        public void noGymnast(Boolean hasGymnast)
        {
            if (hasGymnast)
            {
                GymnastVisibility = Visibility.Hidden;
            }
            else 
            {
                GymnastVisibility = Visibility.Visible;
            }
        }

        protected override void initRelayCommands()
        {

        }
    }
}