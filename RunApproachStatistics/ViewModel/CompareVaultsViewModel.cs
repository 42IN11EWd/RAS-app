using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RunApproachStatistics.ViewModel
{
    public class CompareVaultsViewModel : AbstractViewModel
    {
        private IApplicationController _app;

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
        private String leftVideoPath;
        private VideoViewModel leftVideoView;

        // Right sided variables
        private String rightFullName;
        private String rightDate;
        private String rightVaultNumber;
        private String rightTotalScore;
        private Boolean rightIsEnabled;
        private String rightVideoPath;
        private VideoViewModel rightVideoView;

        // Universal sided variables 
        private GraphViewModel distanceGraphView;
        private GraphViewModel speedGraphView;

        private Double currentPosition;
        private Double maximum;
        private Double playbackSpeed;
        private String playbackSpeedString;
        private Boolean dragging = false;
        private Boolean sliderEnabled;
        private Double leftPlaybackDelay = 0;
        private Double rightPlaybackDelay = 0;
        private Boolean selectionEnabled;

        private BitmapImage playButtonImage;
        private BitmapImage pauseImage = new BitmapImage(new Uri(@"/Images/videoControl_pause_b.png", UriKind.Relative));
        private BitmapImage playImage = new BitmapImage(new Uri(@"/Images/videoControl_play_b.png", UriKind.Relative));
        #endregion

        #region DataBinding
        public RelayCommand LeftSelectionCommand { get; private set; }
        public RelayCommand RightSelectionCommand { get; private set; }

        public RelayCommand PlayClickCommand { get; private set; }
        public RelayCommand StopClickCommand { get; private set; }
        public RelayCommand ForwardClickCommand { get; private set; }
        public RelayCommand BackwardClickCommand { get; private set; }
        public RelayCommand MouseUpCommand { get; private set; }
        public RelayCommand MouseDownCommand { get; private set; }

        #region Vault info
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
                setSliderEnabled();
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
                setSliderEnabled();
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
        #endregion

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
                if (dragging)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(value);
                    if (rightIsEnabled)
                    {
                        rightVideoView.Video.Position = ts.Subtract(TimeSpan.FromMilliseconds(rightPlaybackDelay));
                    }
                    else
                    {
                        rightPlaybackDelay = value - rightVideoView.CurrentPosition;
                    }

                    if (leftIsEnabled)
                    {
                        leftVideoView.Video.Position = ts.Subtract(TimeSpan.FromMilliseconds(leftPlaybackDelay));
                    }
                    else
                    {
                        leftPlaybackDelay = value - leftVideoView.CurrentPosition;
                    }
                }
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

        public BitmapImage PlayButtonImage
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
                LeftVideoView.PlaybackSpeed = value;
                RightVideoView.PlaybackSpeed = value;
                PlaybackSpeedString = Math.Round(value, 2).ToString("0.00", CultureInfo.InvariantCulture);
                OnPropertyChanged("PlaybackSpeed");
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

        public Boolean SliderEnabled
        {
            get { return sliderEnabled; }
            set
            {
                sliderEnabled = value;
                OnPropertyChanged("SliderEnabled");
            }
        }

        public Boolean SelectionEnabled
        {
            get { return selectionEnabled; }
            set
            {
                selectionEnabled = value;
                OnPropertyChanged("SelectionEnabled");
            }
        }
        #endregion

        #region Command Methodes
        public void ToggleLeftSelection(object commandParam)
        {
            LeftIsEnabled = !LeftIsEnabled;
        }

        public void ToggleRightSelection(object commandParam)
        {
            RightIsEnabled = !RightIsEnabled;
        }

        public void MouseDown(object commandParam)
        {
            if (rightIsEnabled || leftIsEnabled)
            {
                dragging = true;
                if (rightIsEnabled)
                {
                    rightVideoView.MouseDown(null);
                }
                if (leftIsEnabled)
                {
                    leftVideoView.MouseDown(null);
                }
            }
        }

        public void MouseUp(object commandParam)
        {
            if (rightIsEnabled || leftIsEnabled)
            {
                dragging = false;
                if (rightIsEnabled)
                {
                    rightVideoView.MouseUp(null);
                }
                if (leftIsEnabled)
                {
                    leftVideoView.MouseUp(null);
                }
            }
        }
        #endregion

        public CompareVaultsViewModel(IApplicationController app)
            : base()
        {
            _app = app;

            // Set menu
            MenuViewModel menuViewModel     = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser   = true;
            Menu                            = menuViewModel;

            PlayButtonImage     = playImage;
            SelectionEnabled    = true;
            rightIsEnabled      = true;
            LeftIsEnabled       = true;

            DistanceGraphView   = new GraphViewModel(_app, this, false, 1000);
            SpeedGraphView      = new GraphViewModel(_app, this, false, 1000);
        }

        public void setVaultsToCompare(List<vault> vaults)
        {
            // Set left vault
            setVaultLabels(vaults[0], "Left");
            leftVideoPath = vaults[0].videopath;
            leftVideoView = new VideoViewModel(_app, null, this, vaults[0].videopath);
            leftVideoView.ToggleVideoControls(false);

            // set Right vault
            setVaultLabels(vaults[1], "Right");
            rightVideoPath = vaults[1].videopath;
            rightVideoView = new VideoViewModel(_app, null, this, vaults[1].videopath);
            rightVideoView.ToggleVideoControls(false);

            String[][][] vaultsSpecialized = new String[vaults.Count][][]; // [vaultnumber][typenumber][measurementnumber], VN = 0 || 1. TN = 0 (Distance} || 1 (Speed). MN = Numeric (Aantal measurements)


            for (int i = 0; i < vaults.Count; i++)
            {

                String[] measurements = vaults[i].graphdata.Split(',');
                Boolean hasSpeed = !String.IsNullOrWhiteSpace(measurements[0].Split(' ')[1]);

                vaultsSpecialized[i] = new String[2][];
                for (int j = 0; j < 2; j++)
                {
                    vaultsSpecialized[i][j] = new String[measurements.Length];
                }

                if (hasSpeed)
                {
                    for (int j = 0; j < measurements.Length; j++)
                    {
                        if (!String.IsNullOrWhiteSpace(measurements[j]))
                        {
                            String[] splitString = measurements[j].Split(' ');

                            vaultsSpecialized[i][0][j] = splitString[0];
                            vaultsSpecialized[i][1][j] = splitString[1];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < measurements.Length; j++)
                    {
                        if (!String.IsNullOrWhiteSpace(measurements[j]))
                        {
                            vaultsSpecialized[i][0][j] = measurements[j];
                        }
                    }
                }
            }

            SpeedGraphView.setupSpecializedGraph("Speed", vaultsSpecialized[0][0], vaultsSpecialized[1][0]);
            DistanceGraphView.setupSpecializedGraph("Distance", vaultsSpecialized[0][1], vaultsSpecialized[1][1]);

            /*
            DistanceGraphView.setupSpecializedGraph("Distance", vaultsSpecialized[0][0], vaultsSpecialized[1][0]);
            SpeedGraphView.setupSpecializedGraph("Speed", vaultsSpecialized[0][1], vaultsSpecialized[1][1]);
             */
        }

        public void setVideoInfo()
        {
            // Set video settings
            double leftMax = leftVideoView.Maximum;
            double rightMax = rightVideoView.Maximum;

            if (leftMax < rightMax)
            {
                Maximum = rightMax;
            }
            else
            {
                Maximum = leftMax;
            }

            CurrentPosition = 0;
            PlaybackSpeed   = 1.0;
            setSliderEnabled();
        }

        /// <summary>
        /// Convert milliseconds to a time string
        /// </summary>
        /// <param name="ms"></param>
        /// <returns>00:00:000 formatted string</returns>
        private String MillisecondsToTimespan(double ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}:{1:D2}:{2:D3}",
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
        }

        private void setVaultLabels(vault setVault, String side)
        {
            Type classType = this.GetType();

            String fullName = "Unknown gymnast";
            if (setVault.gymnast != null)
            {
                fullName = setVault.gymnast.name + (!String.IsNullOrWhiteSpace(setVault.gymnast.surname_prefix) ? " " +
                                                    setVault.gymnast.surname_prefix + " " : " ") + setVault.gymnast.surname;
            }
            classType.GetProperty(side + "FullName").SetValue(this, fullName);

            String date = "Unknown date";
            if (setVault.timestamp != null && !String.IsNullOrWhiteSpace(setVault.timestamp.ToShortDateString()))
            {
                date = setVault.timestamp.ToShortDateString();
            }
            classType.GetProperty(side + "Date").SetValue(this, date);

            String vaultnumber = "Vault Number undefined";
            if (setVault.vaultnumber != null && setVault.vaultnumber.code != null)
            {
                vaultnumber = setVault.vaultnumber.code;
            }
            classType.GetProperty(side + "VaultNumber").SetValue(this, vaultnumber);

            decimal totalScore = setVault.rating_official_E.GetValueOrDefault() + setVault.rating_official_D.GetValueOrDefault() - setVault.penalty.GetValueOrDefault();
            classType.GetProperty(side + "TotalScore").SetValue(this, totalScore.ToString("0.000"));
        }

        public void updateSeconds(float duration)
        {
            // langste gebruiken
            // graph  .updateSpecializedGraphLength(duration);
            DistanceGraphView.updateSpecializedGraphLength(duration);
            SpeedGraphView.updateSpecializedGraphLength(duration);
        }

        public void updateCurrentPosition(double seconds, String filePath)
        {
            if (!dragging)    
            {
                double milliseconds = (seconds * 1000);

                VideoViewModel viewmodel    = null;
                Boolean otherVideoPlaying   = false;

                if (rightVideoPath.Equals(Path.GetFileName(filePath)))
                {
                    viewmodel = RightVideoView;
                    otherVideoPlaying = LeftVideoView.IsPlaying;

                    DistanceGraphView.updateRightVideoSlider(seconds);
                    SpeedGraphView.updateRightVideoSlider(seconds);
                }
                else
                {
                    viewmodel = LeftVideoView;
                    otherVideoPlaying = RightVideoView.IsPlaying;

                    DistanceGraphView.updateLeftVideoSlider(seconds);
                    SpeedGraphView.updateLeftVideoSlider(seconds);
                }

                if ((viewmodel.IsPlaying && !otherVideoPlaying) || (viewmodel.IsPlaying && currentPosition < milliseconds))
                {
                    CurrentPosition = milliseconds;
                }
            }
        }

        private void setSliderEnabled()
        {
            if (rightIsEnabled || leftIsEnabled)
            {
                SliderEnabled = true;
            }
            else 
            {
                SliderEnabled = false;
            }
        }

        public void forwardVideo(object commandParam)
        {
            if (rightIsEnabled)
            {
                rightVideoView.ForwardMedia(commandParam);
            }

            if (leftIsEnabled)
            {
                leftVideoView.ForwardMedia(commandParam);
            }
        }

        public void rewindVideo(object commandParam)
        {
            if (rightIsEnabled)
            {
                rightVideoView.BackwardMedia(commandParam);
            }

            if (leftIsEnabled)
            {
                leftVideoView.BackwardMedia(commandParam);
            }
        }

        public void playVideo(object commandParam)
        {
            if (rightIsEnabled)
            {
                rightVideoView.PlayMedia(null);
            }

            if (leftIsEnabled)
            {
                leftVideoView.PlayMedia(null);
            }

            if (leftVideoView.IsPlaying || rightVideoView.IsPlaying)
            {
                PlayButtonImage = pauseImage;
                SelectionEnabled = false;
            }
            else
            {
                PlayButtonImage = playImage;
                SelectionEnabled = true;
            }
        }

        public void stopVideo(object commandParam)
        {
            if (rightIsEnabled)
            {
                rightVideoView.StopMedia(null);
            }

            if (leftIsEnabled)
            {
                leftVideoView.StopMedia(null);
            }

            SelectionEnabled = true;

            CurrentPosition = 0;
            PlayButtonImage = playImage;
        }

        protected override void initRelayCommands()
        {
            PlayClickCommand = new RelayCommand(playVideo);
            StopClickCommand = new RelayCommand(stopVideo);
            ForwardClickCommand = new RelayCommand(forwardVideo);
            BackwardClickCommand = new RelayCommand(rewindVideo);
            LeftSelectionCommand = new RelayCommand(ToggleLeftSelection);
            RightSelectionCommand = new RelayCommand(ToggleRightSelection);

            MouseUpCommand = new RelayCommand(MouseUp);
            MouseDownCommand = new RelayCommand(MouseDown);
        }
    }
}
