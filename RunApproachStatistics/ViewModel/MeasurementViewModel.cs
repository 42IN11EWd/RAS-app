﻿using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using RunApproachStatistics.Services;
using RunApproachStatistics.View;
using System;
using System.Drawing;
using System.Windows.Forms.Integration;


namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase ratingControl;

        private String vaultKind;
        public String[] vaultKindArray;
        private String location;
        private String gymnast;
        private String vaultNumber;
        private String dscore;
        private String escore;
        private String penalty;
        private String totalscore;
        
        private Boolean manualModeChecked;
        private Boolean measuring;
        private String measurementButtonContent;

        private CameraViewModel cameraView;
        private CameraWindow cameraWindow;
        private VideoCameraController videoCameraController;
        private PortController portController;

        private GraphViewModel graphView;

        public VideoCameraController VideoCameraController
        {
            get { return videoCameraController; }
            set
            {
                videoCameraController = value;
                openVideoSource();
            }
        }

        #region Modules

        private IVaultModule vaultModule = new VaultModule();
        private ICameraModule cameraModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand PostMeasurementCommand { get; private set; }
        public RelayCommand StartMeasurementCommand { get; private set; }

        public PropertyChangedBase RatingControl
        {
            get { return ratingControl; }
            set
            {
                ratingControl = value;
                OnPropertyChanged("RatingControl");
            }
        }

        public Boolean Measuring
        {
            get { return measuring; }
            set
            {
                measuring = value;
                if(value == true && ManualModeChecked)
                {
                    MeasurementButtonContent = "Stop Measurement";
                    videoCameraController.Capture(); // --> Doesnt recognize the DLL
                    // portController.startMeasurement();
                }
                else if(value == false && ManualModeChecked) 
                {
                    MeasurementButtonContent = "Start Measurement";
                    if (videoCameraController.IsCapturing)
                    {
                        videoCameraController.StopCapture();
                        cameraModule.createVideoData(null, videoCameraController.RecordedVideo);
                        videoCameraController.Close();
                        // portController.startMeasurement();
                    }
                }
                else
                {
                    MeasurementButtonContent = "";
                }
                
                OnPropertyChanged("Measuring");
            }
        }

        public GraphViewModel GraphViewMeasurement
        {
            get { return graphView; }
            set
            {
                graphView = value;
                OnPropertyChanged("GraphViewMeasurement");
            }
        }

        public Boolean ManualModeChecked
        {
            get { return manualModeChecked; }
            set
            {
                manualModeChecked = value;
                //turn manual mode on
                Measuring = false;
                OnPropertyChanged("ManualModeChecked");
            }
        }

        public String MeasurementButtonContent
        {
            get { return measurementButtonContent; }
            set
            {
                measurementButtonContent = value;
                OnPropertyChanged("MeasurementButtonContent");
            }
        }
        public String SelectedVaultKind
        {
            get
            {
                if (vaultKind == null) return "";
                return vaultKind;
            }
            set
            {
                vaultKind = value;
                OnPropertyChanged("VaultKind");
            }

        }

        
        public String[] VaultKind
        {
            get { return vaultKindArray; }
            set
            {
                vaultKindArray = value;
                OnPropertyChanged("VaultKind");
            }
        }

        public String Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        public String Gymnast
        {
            get { return gymnast; }
            set
            {
                gymnast = value;
                OnPropertyChanged("Gymnast");
            }
        }

        public String VaultNumber
        {
            get { return vaultNumber; }
            set
            {
                vaultNumber = value;
                OnPropertyChanged("VaultNumber");
            }
        }

        public String Dscore
        {
            get { return dscore; }
            set
            {
                dscore = value;
                OnPropertyChanged("Dscore");
            }
        }

        public String Escore
        {
            get { return escore; }
            set
            {
                escore = value;
                OnPropertyChanged("Escore");
            }
        }

        public String Penalty
        {
            get { return penalty; }
            set
            {
                penalty = value;
                OnPropertyChanged("Penalty");
            }
        }

        public String Totalscore
        {
            get { return totalscore; }
            set
            {
                totalscore = value;
                OnPropertyChanged("Totalscore");
            }
        }

        public CameraViewModel CameraView
        {
            get { return cameraView; }
            set
            {
                cameraView = value;
                OnPropertyChanged("CameraView");
            }
        }

        #endregion

        public MeasurementViewModel(IApplicationController app, PortController portController, VideoCameraController videoCameraController) : base()
        {
            _app = app;
            Measuring = false;
            RatingViewModel ratingVM = new RatingViewModel(_app);
            RatingControl = ratingVM;        

            //put data in array for testing
            vaultKindArray = new String[3];
            vaultKindArray[0] = "practice";
            vaultKindArray[1] = "nk";
            vaultKindArray[2] = "ek";

            // Set PortController
            this.portController = portController;

            // Set VideoCamera
            CameraView = new CameraViewModel(_app);
            VideoCameraController = videoCameraController;

            // Set Graph
            GraphViewModel graphVM = new GraphViewModel(_app, this, 0);
            GraphViewMeasurement = graphVM;
        }

        private void openVideoSource()
        {
            cameraWindow = videoCameraController.CameraWindow;

            if (cameraWindow != null)
            {
                setVideoSource(cameraWindow);
            }
        }

        private void setVideoSource(CameraWindow cameraWindow)
        {
            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = cameraWindow;
            cameraView.CameraHost = host;
        }

        #region RelayCommands

        public void LoadPostMeasurementScreen(object commandParam)
        {
            _app.ShowPostMeasurementView();
        }
        public void StartMeasurement(object commandParam)
        {
            //start measurement
            if(Measuring)
            {
                Measuring = false;
            }
            else
            {
                Measuring = true;
            }
            


        }

        #endregion

        protected override void initRelayCommands()
        {
            PostMeasurementCommand = new RelayCommand(LoadPostMeasurementScreen);
            StartMeasurementCommand = new RelayCommand(StartMeasurement);
        }
       
    }
}
