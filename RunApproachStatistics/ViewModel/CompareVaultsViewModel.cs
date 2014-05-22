﻿using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RunApproachStatistics.ViewModel
{
    public class CompareVaultsViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;

        #region Modules

        private IVaultModule vaultModule = new VaultModule();

        #endregion

        #region Databinded Variables
        // Left sided variables
        private String leftFullName;
        private String leftDate;
        private String leftVaultNumber;
        private String leftTotalScore;
        private Boolean leftIsEnabled;
        private VideoViewModel leftVideoView;

        // Right sided variables
        private String rightFullName;
        private String rightDate;
        private String rightVaultNumber;
        private String rightTotalScore;
        private Boolean rightIsEnabled;
        private VideoViewModel rightVideoView;

        // Universal sided variables 
        private GraphViewModel distanceGraphView;
        private GraphViewModel speedGraphView;

        private Double currentPosition;
        private Double maximum;
        private ImageSource playButtonImage;
        private Double playbackSpeed;
        private String currentTime;
        private String totalTime;
        private String playbackSpeedString;
        #endregion

        #region DataBinding
        public RelayCommand PlayClickCommand { get; private set; }

        public RelayCommand StopClickCommand { get; private set; }

        public RelayCommand ForwardClickCommand { get; private set; }

        public RelayCommand BackwardClickCommand { get; private set; }

        public RelayCommand MouseUpCommand { get; private set; }

        public RelayCommand MouseDownCommand { get; private set; }

        // Left sided variables
        public String LeftFullName
        {
            get { return leftFullName; }
            set
            {
                leftFullName = value;
                OnPropertyChanged("LeftFullName");
            }
        }

        public String LeftDate
        {
            get { return leftDate; }
            set
            {
                leftDate = value;
                OnPropertyChanged("LeftDate");
            }
        }

        public String LeftVaultNumber
        {
            get { return leftVaultNumber; }
            set
            {
                leftVaultNumber = value;
                OnPropertyChanged("LeftVaultNumber");
            }
        }

        public String LeftTotalScore
        {
            get { return leftTotalScore; }
            set
            {
                leftTotalScore = value;
                OnPropertyChanged("LeftTotalScore");
            }
        }

        public Boolean LeftIsEnabled
        {
            get { return leftIsEnabled; }
            set
            {
                leftIsEnabled = value;
                OnPropertyChanged("LeftIsEnabled");
            }
        }

        public VideoViewModel LeftVideoView
        {
            get { return leftVideoView; }
            set
            {
                leftVideoView = value;
                OnPropertyChanged("LeftVideoView");
            }
        }

        // Right sided variables
        public String RightFullName
        {
            get { return rightFullName; }
            set
            {
                rightFullName = value;
                OnPropertyChanged("RightFullName");
            }
        }

        public String RightDate
        {
            get { return rightDate; }
            set
            {
                rightDate = value;
                OnPropertyChanged("RightDate");
            }
        }

        public String RightVaultNumber
        {
            get { return rightVaultNumber; }
            set
            {
                rightVaultNumber = value;
                OnPropertyChanged("RightVaultNumber");
            }
        }

        public String RightTotalScore
        {
            get { return rightTotalScore; }
            set
            {
                rightTotalScore = value;
                OnPropertyChanged("RightTotalScore");
            }
        }

        public Boolean RightIsEnabled
        {
            get { return rightIsEnabled; }
            set
            {
                rightIsEnabled = value;
                OnPropertyChanged("RightIsEnabled");
            }
        }

        public VideoViewModel RightVideoView
        {
            get { return rightVideoView; }
            set
            {
                rightVideoView = value;
                OnPropertyChanged("RightVideoView");
            }
        }

        // Universal sided variables 
        public GraphViewModel DistanceGraphView
        {
            get { return distanceGraphView; }
            set
            {
                distanceGraphView = value;
                OnPropertyChanged("DistanceGraphView");
            }
        }

        public GraphViewModel SpeedGraphView
        {
            get { return speedGraphView; }
            set
            {
                speedGraphView = value;
                OnPropertyChanged("SpeedGraphView");
            }
        }

        public Double CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }

        public Double Maximum
        {
            get { return maximum; }
            set
            {
                maximum = value;
                OnPropertyChanged("Maximum");
            }
        }

        public ImageSource PlayButtonImage
        {
            get { return playButtonImage; }
            set
            {
                playButtonImage = value;
                OnPropertyChanged("PlayButtonImage");
            }
        }

        public Double PlaybackSpeed
        {
            get { return playbackSpeed; }
            set
            {
                playbackSpeed = value;
                OnPropertyChanged("PlaybackSpeed");
            }
        }

        public String CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        public String TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }

        public String PlaybackSpeedString
        {
            get { return playbackSpeedString; }
            set
            {
                playbackSpeedString = value;
                OnPropertyChanged("PlaybackSpeedString");
            }
        }

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

        public CompareVaultsViewModel(IApplicationController app) : base()
        {
            _app = app;

            // Set menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = true;
            Menu = menuViewModel;
        }

        public void setVaultsToCompare(List<vault> vaults)
        {
            // Set left vault
            setVaultLabels(vaults[0], "Left");

            // set Right vault
            setVaultLabels(vaults[1], "Right");
        }

        private void setVaultLabels(vault setVault, String side)
        {
            Type classType = this.GetType();
            if (setVault.gymnast != null)
            {
                String fullName = setVault.gymnast.name + setVault.gymnast.surname_prefix != null ? " " + 
                    setVault.gymnast.surname_prefix : "" + setVault.gymnast.surname;
                classType.GetProperty(side + "FullName").SetValue(this, fullName);
            }

            classType.GetProperty(side + "Date").SetValue(this, setVault.timestamp);

            if (setVault.vaultnumber != null)
            {
                classType.GetProperty(side + "VaultNumber").SetValue(this, setVault.vaultnumber.code);
            }

            if (setVault.rating_official_E != null && setVault.rating_official_D != null)
            {
                decimal totalScore = (decimal)setVault.rating_official_E + (decimal)setVault.rating_official_D;
                if (setVault.penalty != null)
                {
                    totalScore = totalScore - (decimal)setVault.penalty;
                }
                classType.GetProperty(side + "TotalScore").SetValue(this, totalScore);
            }
        }

        protected override void initRelayCommands()
        {

        }
    }
}
