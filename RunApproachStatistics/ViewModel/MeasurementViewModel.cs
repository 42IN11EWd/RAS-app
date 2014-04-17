using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class MeasurementViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase ratingValue;

        #region Modules

        private ICameraModule cameraModule = new VaultModule();

        #endregion

        #region DataBinding

        public RelayCommand PostMeasurementCommand { get; private set; }
        public RelayCommand ClickRatingCommand { get; private set; }

        public PropertyChangedBase RatingValue
        {
            get { return ratingValue; }
            set
            {
                ratingValue = value;
                OnPropertyChanged("RatingValue");
            }
        }

        #endregion

        public MeasurementViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        #region RelayCommands

        public void LoadPostMeasurementScreen(object commandParam)
        {
            _app.ShowPostMeasurementView();
        }
        public void ClickRating(object commandParam)
        {

            //Console.WriteLine("clicked: " + commandParam.ToString());
            Console.WriteLine("RatingValue: " + RatingValue);
        }

        #endregion

        protected override void initRelayCommands()
        {
            PostMeasurementCommand = new RelayCommand(LoadPostMeasurementScreen);
            ClickRatingCommand = new RelayCommand(ClickRating);
        }
    }
}
