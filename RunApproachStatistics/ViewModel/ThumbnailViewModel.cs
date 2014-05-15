﻿using RunApproachStatistics.Controllers;
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

        public ThumbnailViewModel(IApplicationController app)
            : base()
        {
            _app = app;
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

        public String Gymnast
        {
            get { return vault.gymnast != null ? vault.gymnast.name : ""; }
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

        private void setThumbnail()
        {
            try
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(vault.thumbnail);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                ThumbnailImage = biImg as ImageSource;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void initRelayCommands()
        {

        }
    }
}