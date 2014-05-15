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
        private Visibility liveLabelVisibility;

        public ThumbnailViewModel(IApplicationController app)
            : base()
        {
            _app = app;
            LiveLabelVisibility = Visibility.Hidden;
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

        public Visibility LiveLabelVisibility
        {
            get { return liveLabelVisibility; }
            set 
            { 
                liveLabelVisibility = value;
                OnPropertyChanged("LiveLabelVisibility");
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
                OnPropertyChanged("NoThumbnailRectangle");
            }
        }

        public Visibility HasThumnailImage
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
            }
            else
            {
                LiveLabelVisibility = Visibility.Hidden;
            }
        }

        protected override void initRelayCommands()
        {

        }
    }
}