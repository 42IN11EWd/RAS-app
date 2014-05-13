using RunApproachStatistics.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    class RatingViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private int ratingValue;
        private bool star1Checked;
        private bool star2Checked;
        private bool star3Checked;
        private bool star4Checked;
        private bool star5Checked;
        private bool[] starArray = new bool[6];

        #region Bindings
        public int RatingValue
        {
            get { return ratingValue; }
            set
            {
                ratingValue = value;
                Console.WriteLine(ratingValue);
                for (int i = 1; i < 6; i++)
                {

                    if (i <= ratingValue)
                    {
                        starArray[i] = true;
                    }
                    else
                    {
                        starArray[i] = false;
                    }
                }
                star1Checked = starArray[1];
                star2Checked = starArray[2];
                star3Checked = starArray[3];
                star4Checked = starArray[4];
                star5Checked = starArray[5];
                OnPropertyChanged("Star1Checked");
                OnPropertyChanged("Star2Checked");
                OnPropertyChanged("Star3Checked");
                OnPropertyChanged("Star4Checked");
                OnPropertyChanged("Star5Checked");
                OnPropertyChanged("RatingValue");
            }
        }
        public bool Star1Checked
        {
            get { return star1Checked; }
            set
            {
                star1Checked = value;
                RatingValue = 1;
                OnPropertyChanged("Star1Checked");
            }
        }
        public bool Star2Checked
        {
            get { return star2Checked; }
            set
            {
                star2Checked = value;
                RatingValue = 2;
                OnPropertyChanged("Star2Checked");

            }
        }
        public bool Star3Checked
        {
            get { return star3Checked; }
            set
            {
                star3Checked = value;
                RatingValue = 3;
                OnPropertyChanged("Star3Checked");
            }
        }
        public bool Star4Checked
        {
            get { return star4Checked; }
            set
            {
                star4Checked = value;
                RatingValue = 4;
                OnPropertyChanged("Star4Checked");

            }
        }
        public bool Star5Checked
        {
            get { return star5Checked; }
            set
            {
                star5Checked = value;
                RatingValue = 5;
                OnPropertyChanged("Star5Checked");
            }
        }    

        #endregion

        public RatingViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        protected override void initRelayCommands()
        {
          
        }

        public int getScore()
        {
            if (star5Checked)
            {
                return 5;
            }

            if (star4Checked)
            {
                return 4;
            }

            if (star3Checked)
            {
                return 3;
            }

            if (star2Checked)
            {
                return 2;
            }

            if (star1Checked)
            {
                return 1;
            }

            return 0;
        }
    }
}
