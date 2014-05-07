using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RunApproachStatistics.ViewModel
{
    public class ProfileViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase menu;
        private UserModule uModule;

        private ImageSource picture;
        private Visibility pictureAvailable;
        private Visibility pictureCommandVisible;

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

        private String editName;
        private String editPrefix;
        private String editSurname;
        private String editFIGNumber;
        private String editDateOfBirth;
        private String editNationality;
        private String editGender;
        private String editLength;
        private String editWeight;
        private Boolean editingMemos;

        private String filterField;
        private List<gymnast> gymnastList; // To store all the gymnasts
        private ObservableCollection<gymnast> filterList; // T.B.D. to be deleted or converted
        private gymnast selectedFilterItem;
        private Boolean enableFilter;

        private Boolean inEditingMode;
        private Boolean creatingNewGymnast;

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

        public Visibility PictureCommandVisible
        {
            get { return pictureCommandVisible; }
            set
            {
                pictureCommandVisible = value;
                OnPropertyChanged("PictureCommandVisible");
            }
        }

        public String Fullname
        {
            get
            {
                return !inEditingMode ? Name + (!String.IsNullOrWhiteSpace(Prefix) ? " " + Prefix + " " : " ") + Surname : EditName + (!String.IsNullOrWhiteSpace(EditPrefix) ? " " + EditPrefix + " " : " ") + EditSurname;
            }
        }

        // Regular fields (displaying)

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
                OnPropertyChanged("Surname");
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

        // Edit fields (when editing)

        public String EditName
        {
            get { return editName; }
            set
            {
                editName = value;
                OnPropertyChanged("EditName");
                OnPropertyChanged("Fullname");
            }
        }

        public String EditPrefix
        {
            get { return editPrefix; }
            set
            {
                editPrefix = value;
                OnPropertyChanged("EditPrefix");
                OnPropertyChanged("Fullname");
            }
        }

        public String EditSurname
        {
            get { return editSurname; }
            set
            {
                editSurname = value;
                OnPropertyChanged("EditSurname");
                OnPropertyChanged("Fullname");
            }
        }

        public String EditFIGNumber
        {
            get { return editFIGNumber; }
            set
            {
                editFIGNumber = value;
                OnPropertyChanged("EditFIGNumber");
            }
        }

        public String EditDateOfBirth
        {
            get { return editDateOfBirth; }
            set
            {
                editDateOfBirth = value;
                OnPropertyChanged("EditDateOfBirth");
            }
        }

        public String EditNationality
        {
            get { return editNationality; }
            set
            {
                editNationality = value;
                OnPropertyChanged("EditNationality");
            }
        }

        public String EditGender
        {
            get { return editGender; }
            set
            {
                editGender = value;
                OnPropertyChanged("EditGender");
            }
        }

        public String EditLength
        {
            get { return editLength; }
            set
            {
                editLength = value;
                OnPropertyChanged("EditLength");
            }
        }

        public String EditWeight
        {
            get { return editWeight; }
            set
            {
                editWeight = value;
                OnPropertyChanged("EditWeight");
            }
        }

        public Boolean EditingMemos
        {
            get { return editingMemos; }
            set
            {
                editingMemos = value;
                OnPropertyChanged("EditingMemos");
            }
        }

        public Visibility IsEditing
        {
            get { return inEditingMode ? Visibility.Visible : Visibility.Hidden; }
        }

        public Visibility IsNotEditing
        {
            get { return !inEditingMode ? Visibility.Visible : Visibility.Hidden; }
        }

        public String FilterField
        {
            get { return filterField; }
            set
            {
                filterField = value;
                OnPropertyChanged("FilterField");

                filterList.Clear();
                foreach (gymnast gymnast in gymnastList)
                {
                    String tempFullname = gymnast.name + (!String.IsNullOrWhiteSpace(gymnast.surname_prefix) ? " " + gymnast.surname_prefix + " " : " ") + gymnast.surname;
                    if (tempFullname.Contains(value))
                    {
                        filterList.Add(gymnast);
                    }
                }
                OnPropertyChanged("FilterList");
            }
        }

        public ObservableCollection<gymnast> FilterList
        {
            get
            {
                //Get filtered list
                return filterList; // to be removed
            }
        }

        public gymnast SelectedFilterItem
        {
            get { return selectedFilterItem; }
            set
            {
                if (value != null)
                {
                    selectedFilterItem = value;

                    Picture = null; // read bitmap from g.picture
                    PictureAvailable = Picture != null ? Visibility.Visible : Visibility.Hidden;
                    Name = value.name;
                    Prefix = value.surname_prefix;
                    Surname = value.surname;
                    FIGNumber = value.turnbondID.ToString();
                    DateOfBirth = value.birthdate.ToString();
                    Nationality = value.nationality;
                    Gender = value.gender;
                    Length = value.length.ToString();
                    Weight = value.weight.ToString();
                    Memos = value.note;

                    OnPropertyChanged("Fullname");
                    OnPropertyChanged("SelectedFilterItem");
                }

            }
        }

        public Boolean EnableFilter
        {
            get { return enableFilter; }
            set
            {
                enableFilter = value;
                OnPropertyChanged("EnableFilter");
            }
        }
        #endregion

        public ProfileViewModel(IApplicationController app)
            : base()
        {
            _app = app;
            uModule = new UserModule();
            filterList = new ObservableCollection<gymnast>();

            // Set the menu
            MenuViewModel menuViewModel = new MenuViewModel(_app);
            menuViewModel.VisibilityLaser = false;
            Menu = menuViewModel;

            // Get the user profile
            gymnastList = uModule.getGymnastCollection();
            foreach (gymnast g in gymnastList)
            {
                filterList.Add(g);
            }
            OnPropertyChanged("FilterList");

            //Other misc stuff
            PictureCommandVisible = Visibility.Hidden;
            EnableFilter = true;
            inEditingMode = false;
            creatingNewGymnast = false;
        }

        #region Command Methodes

        public void InitPictureUpload(object commandParam)
        {
            // Do something? Dialog first?
        }

        public void SaveChanges(object commandParam)
        {
            decimal tmpLength;
            decimal tmpWeight;
            DateTime tmpDoB;

            gymnast gymnast = creatingNewGymnast ? new gymnast() : SelectedFilterItem;

            gymnast.name = EditName;
            gymnast.surname_prefix = EditPrefix;
            gymnast.surname = EditSurname;
            gymnast.turnbondID = long.Parse(EditFIGNumber);
            gymnast.birthdate = DateTime.TryParse(EditLength, out tmpDoB) ? tmpDoB : (DateTime?)null;
            gymnast.nationality = EditNationality;
            gymnast.gender = EditGender;
            gymnast.length = decimal.TryParse(EditLength, out tmpLength) ? tmpLength : (decimal?)null;
            gymnast.weight = decimal.TryParse(EditWeight, out tmpWeight) ? tmpWeight : (decimal?)null;

            if (creatingNewGymnast)
            { // Create a new Gymnast
                uModule.create(gymnast);
            }
            else
            { // update gymnast
                uModule.update(gymnast);
            }

            CancelChanges(null); // returns to non editing mode

            gymnastList = uModule.getGymnastCollection();
            foreach (gymnast g in gymnastList)
            {
                filterList.Add(g);
            }
        }

        public Boolean CanSaveChanges()
        {
            return inEditingMode && madeChanges();
        }

        public void CancelChanges(object commandParam)
        {
            EditingMemos = false;
            PictureCommandVisible = Visibility.Hidden;
            EnableFilter = true;

            inEditingMode = false;
            creatingNewGymnast = false;
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
            OnPropertyChanged("Fullname");
        }

        public Boolean CanCancelChanges()
        {
            return inEditingMode;
        }

        public void DeleteGymnast(object commandParam)
        {
            if (!_app.IsLoggedIn)
            {
                _app.ShowLoginView();

                while (_app.IsLoginWindowOpen)
                {
                    // waiting for the login window to close
                }
            }

            if (_app.IsLoggedIn && SelectedFilterItem != null)
            {
                uModule.delete(SelectedFilterItem.gymnast_id);
            }
        }

        public Boolean CanDeleteGymnast()
        {
            return SelectedFilterItem != null;
        }

        public void EditGymnast(object commandParam)
        {
            EditName = Name;
            EditPrefix = Prefix;
            EditSurname = Surname;
            EditFIGNumber = figNumber;
            EditDateOfBirth = DateOfBirth;
            EditNationality = Nationality;
            EditGender = Gender;
            EditLength = Length;
            EditWeight = Weight;
            EditingMemos = true;
            PictureCommandVisible = Visibility.Visible;
            EnableFilter = false;

            inEditingMode = true;
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
        }

        public Boolean CanEditGymnast()
        {
            return SelectedFilterItem != null && !inEditingMode && !creatingNewGymnast;
        }

        public void NewGymnast(object commandParam)
        {
            selectedFilterItem = null;
            OnPropertyChanged("SelectedFilterItem");

            EditName = "";
            EditPrefix = "";
            EditSurname = "";
            EditFIGNumber = "";
            EditDateOfBirth = "";
            EditNationality = "";
            EditGender = "";
            EditLength = "";
            EditWeight = "";
            EditingMemos = true;
            PictureCommandVisible = Visibility.Visible;
            EnableFilter = false;

            inEditingMode = true;
            creatingNewGymnast = true;
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
            OnPropertyChanged("Fullname");
        }

        public Boolean CanNewGymnast()
        {
            return !inEditingMode && !creatingNewGymnast;
        }

        public void ToMainScreen(object commandParam)
        {
            _app.ShowHomeView();
        }

        public void SeeVaults(object commandParam)
        {
            _app.ShowVaultSelectorView(SelectedFilterItem);
        }

        public Boolean CanSeeVaults()
        {
            return SelectedFilterItem != null && !inEditingMode && !creatingNewGymnast;
        }

        #endregion Command Methodes

        protected override void initRelayCommands()
        {
            PictureCommand = new RelayCommand(InitPictureUpload);
            SaveChangesCommand = new RelayCommand(SaveChanges, e => CanSaveChanges());
            CancelChangesCommand = new RelayCommand(CancelChanges, e => CanCancelChanges());
            DeleteGymnastCommand = new RelayCommand(DeleteGymnast, e => CanDeleteGymnast());
            EditGymnastCommand = new RelayCommand(EditGymnast, e => CanEditGymnast());
            NewGymnastCommand = new RelayCommand(NewGymnast, e => CanNewGymnast());
            ToMainscreenCommand = new RelayCommand(ToMainScreen);
            SeeVaultsCommand = new RelayCommand(SeeVaults, e => CanSeeVaults());
        }

        private Boolean madeChanges()
        {
            return true;
        }
    }
}