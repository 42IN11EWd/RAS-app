using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace RunApproachStatistics.ViewModel
{
    public class ProfileViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;

        private ImageSource picture;
        private Visibility pictureAvailable;
        private String pictureText;
        private String fullname;
        private String name;
        private String prefix;
        private String surname;
        private String figNumber;
        private String dateOfBirth;
        private String nationality;
        private String gender;
        private String length;
        private String weight;
        private String memos;
        private String filterField;
        private ObservableCollection<String> filterList; // T.B.D. to be deleted
        private String selectedFilterItem;

        #region Modules

        private IUserModule userModule = new UserModule();
        
        #endregion

        #region DataBinding
        public RelayCommand PictureCommand { get; private set; }
        public RelayCommand SaveChangesCommand { get; private set; }
        public RelayCommand CancelChangesCommand { get; private set; }
        public RelayCommand DeleteGymnastCommand { get; private set; }
        public RelayCommand EditGymnastCommand { get; private set; }
        public RelayCommand NewGymnastCommand { get; private set; }
        public RelayCommand ToMainscreenCommand { get; private set; }
        public RelayCommand SeeVaultsCommand { get; private set; }

        public PropertyChangedBase Menu
        {
            get { return menu; }
            set
            {
                menu = value;
                OnPropertyChanged("Menu");
            }
        }

        public ImageSource Picture
        {
            get { return picture; }
            set
            {
                picture = value;
                OnPropertyChanged("Picture");
            }
        }

        public Visibility PictureAvailable
        {
            get { return pictureAvailable; }
            set
            {
                pictureAvailable = value;
                OnPropertyChanged("PictureAvailable");
            }
        }

        public String PictureText
        {
            get { return pictureText; }
            set
            {
                pictureText = value;
                OnPropertyChanged("PictureText");
            }
        }

        public String Fullname
        {
            get { return fullname; }
            set
            {
                fullname = value;
                OnPropertyChanged("Fullname");
            }
        }

        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public String Prefix
        {
            get { return prefix; }
            set
            {
                prefix = value;
                OnPropertyChanged("Prefix");
            }
        }

        public String Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("");
            }
        }

        public String FIGNumber
        {
            get { return figNumber; }
            set
            {
                figNumber = value;
                OnPropertyChanged("FIGNumber");
            }
        }

        public String DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }

        public String Nationality
        {
            get { return nationality; }
            set
            {
                nationality = value;
                OnPropertyChanged("Nationality");
            }
        }

        public String Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public String Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }

        public String Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        public String Memos
        {
            get { return memos; }
            set
            {
                memos = value;
                OnPropertyChanged("Memos");
            }
        }

        public String FilterField
        {
            get { return filterField; }
            set
            {
                filterField = value;
                OnPropertyChanged("FilterField");
            }
        }

        public ObservableCollection<String> FilterList
        {
            get
            {
                //Get filtered list
                return filterList; // to be removed
            }
        }

        public String SelectedFilterItem
        {
            get { return selectedFilterItem; }
            set
            {
                selectedFilterItem = value;
                OnPropertyChanged("SelectedFilterItem");
            }
        }
        /*
        private String filterField;
        private ObservableCollection<String> filterList;
        private String selectedFilterItem;
         */

        #endregion

        public ProfileViewModel(IApplicationController app) : base()
        {
            _app = app;

            // Set the menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = false;
            Menu = menuViewModel;

            // Get the user profile
            //userModule.read();
        }

        #region Command Methodes

        public void InitPictureUpload(object commandParam)
        {
            // Do something?
        }

        public void SaveChanges(object commandParam)
        {
            // Do something?
        }

        public void CancelChanges(object commandParam)
        {
            // Do something?
        }

        public void DeleteGymnast(object commandParam)
        {
            // Do something?
        }

        public void EditGymnast(object commandParam)
        {
            // Do something?
        }

        public void NewGymnast(object commandParam)
        {
            // Do something?
        }

        public void ToMainScreen(object commandParam)
        {
            // Do something?
        }

        public void SeeVaults(object commandParam)
        {
            // Do something?
        }

        #endregion Command Methodes

        protected override void initRelayCommands()
        {
            PictureCommand = new RelayCommand(InitPictureUpload);
            SaveChangesCommand = new RelayCommand(SaveChanges);
            CancelChangesCommand = new RelayCommand(CancelChanges);
            DeleteGymnastCommand = new RelayCommand(DeleteGymnast);
            EditGymnastCommand = new RelayCommand(EditGymnast);
            NewGymnastCommand = new RelayCommand(NewGymnast);
            ToMainscreenCommand = new RelayCommand(ToMainScreen);
            SeeVaultsCommand = new RelayCommand(SeeVaults);
        }
    }
}