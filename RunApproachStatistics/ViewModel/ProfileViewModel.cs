﻿using MvvmValidation;
using RunApproachStatistics.Controllers;
using RunApproachStatistics.Model.Entity;
using RunApproachStatistics.Modules;
using RunApproachStatistics.Modules.Interfaces;
using RunApproachStatistics.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RunApproachStatistics.ViewModel
{
    public class ProfileViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private UserModule uModule;

        private Visibility pictureCommandVisible;
        private ImageSource picture;

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

        private ImageSource editPicture;
        private String editName;
        private String editPrefix;
        private String editSurname;
        private String editFIGNumber;
        private String editDateOfBirth;
        private String editNationality;
        private String editGender;
        private String editLength;
        private String editWeight;
        private String editMemos;

        private String filterField;
        private List<gymnast> gymnastList; // To store all the gymnasts
        private ObservableCollection<gymnast> filterList;
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
        public RelayCommand SeeVaultsCommand { get; private set; }

        public Visibility PictureCommandVisible
        {
            get { return pictureCommandVisible; }
            set
            {
                pictureCommandVisible = value;
                OnPropertyChanged("PictureCommandVisible");
            }
        }

        public ImageSource Picture
        {
            get { return picture; }
            set
            {
                picture = value;
                OnPropertyChanged("Picture");
                OnPropertyChanged("PictureAvailable");
            }
        }

        public Visibility PictureAvailable
        {
            get { return Picture != null && !inEditingMode ? Visibility.Visible : Visibility.Hidden; }
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

        public ImageSource EditPicture
        {
            get { return editPicture; }
            set
            {
                editPicture = value;
                OnPropertyChanged("EditPicture");
                OnPropertyChanged("EditPictureAvailable");
            }
        }

        public Visibility EditPictureAvailable
        {
            get { return EditPicture != null && inEditingMode ? Visibility.Visible : Visibility.Hidden; }
        }

        public String EditName
        {
            get { return editName; }
            set
            {
                editName = value;
                Validator.Validate(() => EditName);
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
                Validator.Validate(() => EditPrefix);
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
                Validator.Validate(() => EditSurname);
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
                Validator.Validate(() => EditFIGNumber);
                OnPropertyChanged("EditFIGNumber");
            }
        }

        public String EditDateOfBirth
        {
            get { return editDateOfBirth; }
            set
            {
                editDateOfBirth = value;
                Validator.Validate(() => EditDateOfBirth);
                OnPropertyChanged("EditDateOfBirth");
            }
        }

        public String EditNationality
        {
            get { return editNationality; }
            set
            {
                editNationality = value;
                Validator.Validate(() => EditNationality);
                OnPropertyChanged("EditNationality");
            }
        }

        public String EditGender
        {
            get { return editGender; }
            set
            {
                editGender = value;
                Validator.Validate(() => EditGender);
                OnPropertyChanged("EditGender");
            }
        }

        public String EditLength
        {
            get { return editLength; }
            set
            {
                editLength = value;
                Validator.Validate(() => EditLength);
                OnPropertyChanged("EditLength");
            }
        }

        public String EditWeight
        {
            get { return editWeight; }
            set
            {
                editWeight = value;
                Validator.Validate(() => EditWeight);
                OnPropertyChanged("EditWeight");
            }
        }

        public String EditMemos
        {
            get { return editMemos; }
            set
            {
                editMemos = value;
                OnPropertyChanged("EditMemos");
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
                applyFilter();
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

                    Name = value.name;
                    Prefix = value.surname_prefix;
                    Surname = value.surname;
                    FIGNumber = value.turnbondID.ToString();
                    DateOfBirth = value.birthdate != null ? value.birthdate.Value.ToShortDateString() : "";
                    Nationality = value.nationality;
                    Gender = value.gender;

                    decimal length = (decimal)value.length;
                    decimal weight = (decimal)value.weight;

                    Length = length.ToString();
                    Weight = weight.ToString();
                    Memos = value.note;

                    if (value.picture != null)
                    {
                        using (var ms = new MemoryStream(value.picture))
                        {
                            BitmapImage image = new BitmapImage();
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();

                            Picture = image;
                        }
                    }
                    else
                    {
                        Picture = null;
                    }

                    OnPropertyChanged("Fullname");
                    OnPropertyChanged("PictureAvailable");
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
            FilterField = "";
            gymnastList = uModule.getGymnastCollection();
            applyFilter();

            //Other misc stuff
            PictureCommandVisible = Visibility.Hidden;
            EnableFilter = true;
            inEditingMode = false;
            creatingNewGymnast = false;

            setValidationRules();
        }

        #region RelayCommand

        public void InitPictureUpload(object commandParam)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            // Setup the OpenFileDialog
            ofd.DefaultExt = ".png";
            ofd.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            ofd.Multiselect = false;
            ofd.Title = "Select profile picture for " + Fullname;

            Nullable<bool> result = ofd.ShowDialog();
            if (result.HasValue && result == true)
            {
                BitmapImage bIamge = new BitmapImage();
                bIamge.BeginInit();
                bIamge.UriSource = new Uri(ofd.FileName);
                bIamge.EndInit();
                EditPicture = bIamge;
            }
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
            gymnast.birthdate = DateTime.TryParse(EditDateOfBirth, out tmpDoB) ? tmpDoB : (DateTime?)null;
            gymnast.nationality = EditNationality;
            gymnast.gender = EditGender;
            gymnast.length = decimal.TryParse(EditLength.Replace(",", "."), out tmpLength) ? tmpLength : (decimal?)null;
            gymnast.weight = decimal.TryParse(EditWeight.Replace(",", "."), out tmpWeight) ? tmpWeight : (decimal?)null;
            gymnast.note = EditMemos;
            //gymnast.picture = EditPicture;
            if (EditPicture != null)
            {
                BitmapImage bImage = EditPicture as BitmapImage; 
                byte[] data = null;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bImage));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                    gymnast.picture = data;
                }
            }

            if (creatingNewGymnast)
            { // Create a new Gymnast
                uModule.create(gymnast);

                selectedFilterItem = null;
                Name = "";
                Prefix = "";
                Surname = "";
                FIGNumber = "";
                DateOfBirth = "";
                Nationality = "";
                Gender = "";
                Length = "";
                Weight = "";
                Memos = "";
                Picture = null;
            }
            else
            { // update gymnast
                uModule.update(gymnast);
                SelectedFilterItem = gymnast; // re-setting selected filter. Should update appropriate fields.
            }

            CancelChanges(null); // returns to non editing mode
            EnableFilter = true;
            inEditingMode = false;

            gymnastList = uModule.getGymnastCollection();
            applyFilter();
            OnPropertyChanged("FilterList");
            OnPropertyChanged("Fullname");
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
            OnPropertyChanged("PictureAvailable");
            OnPropertyChanged("SelectedFilterItem");
            OnPropertyChanged("EditPictureAvailable");
        }

        public Boolean CanSaveChanges()
        {
            return inEditingMode && madeChanges();
        }

        public void CancelChanges(object commandParam)
        {
            PictureCommandVisible = Visibility.Hidden;
            EnableFilter = true;

            inEditingMode = false;
            creatingNewGymnast = false;
            OnPropertyChanged("Fullname");
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
            OnPropertyChanged("PictureAvailable");
            OnPropertyChanged("EditPictureAvailable");
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

            }
            if (_app.IsLoggedIn && SelectedFilterItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    uModule.delete(SelectedFilterItem.gymnast_id);

                    selectedFilterItem = null;
                    Name = "";
                    Prefix = "";
                    Surname = "";
                    FIGNumber = "";
                    DateOfBirth = "";
                    Nationality = "";
                    Gender = "";
                    Length = "";
                    Weight = "";
                    Memos = "";
                    Picture = null;
                    EnableFilter = true;
                    inEditingMode = false;

                    gymnastList = uModule.getGymnastCollection();
                    applyFilter();
                    OnPropertyChanged("FilterList");
                    OnPropertyChanged("Fullname");
                    OnPropertyChanged("IsEditing");
                    OnPropertyChanged("IsNotEditing");
                    OnPropertyChanged("PictureAvailable");
                    OnPropertyChanged("SelectedFilterItem");
                    OnPropertyChanged("EditPictureAvailable");
                }
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
            EditMemos = Memos;
            EditPicture = Picture;
            PictureCommandVisible = Visibility.Visible;
            EnableFilter = false;

            inEditingMode = true;
            OnPropertyChanged("Fullname");
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
            OnPropertyChanged("PictureAvailable");
            OnPropertyChanged("SelectedFilterItem");
            OnPropertyChanged("EditPictureAvailable");
        }

        public Boolean CanEditGymnast()
        {
            return SelectedFilterItem != null && !inEditingMode && !creatingNewGymnast;
        }

        public void NewGymnast(object commandParam)
        {
            selectedFilterItem = null;
            EditName = "";
            EditPrefix = "";
            EditSurname = "";
            EditFIGNumber = "";
            EditDateOfBirth = "";
            EditNationality = "";
            EditGender = "";
            EditLength = "";
            EditWeight = "";
            EditMemos = "";
            EditPicture = null;
            PictureCommandVisible = Visibility.Visible;
            EnableFilter = false;

            inEditingMode = true;
            creatingNewGymnast = true;
            OnPropertyChanged("Fullname");
            OnPropertyChanged("IsEditing");
            OnPropertyChanged("IsNotEditing");
            OnPropertyChanged("PictureAvailable");
            OnPropertyChanged("SelectedFilterItem");
            OnPropertyChanged("EditPictureAvailable");
        }

        public Boolean CanNewGymnast()
        {
            return !inEditingMode && !creatingNewGymnast;
        }

        public void SeeVaults(object commandParam)
        {
            String filterItem = selectedFilterItem.name + " "+ selectedFilterItem.surname_prefix + " " + selectedFilterItem.surname;
            String filterType = "Profilefilter: Gymnast";
            _app.ShowVaultSelectorView(filterItem, filterType);
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
            SeeVaultsCommand = new RelayCommand(SeeVaults, e => CanSeeVaults());
        }

        private void applyFilter()
        {
            if (gymnastList == null)
            {
                return;
            }

            filterList.Clear();
            foreach (gymnast gymnast in gymnastList)
            {
                String tempFullname = gymnast.name + (!String.IsNullOrWhiteSpace(gymnast.surname_prefix) ? " " + gymnast.surname_prefix + " " : " ") + gymnast.surname;
                if (tempFullname.ToLower().Contains(FilterField.ToLower()))
                {
                    filterList.Add(gymnast);
                }
            }
            OnPropertyChanged("FilterList");
        }

        private Boolean madeChanges()
        {
            return !(EditPicture == Picture && EditName == Name && EditPrefix == Prefix && EditSurname == Surname && EditFIGNumber == FIGNumber && EditDateOfBirth == DateOfBirth && EditNationality == Nationality && EditGender == Gender && EditLength == Length && EditWeight == Weight && EditMemos == Memos);
        }

        #region Validation Rules
        private void setValidationRules()
        {
            Validator.AddRule(() => EditName,
                              () =>
                              {
                                  if (EditName == null || EditName.Length > 50 || EditName.Length < 1)
                                  {
                                      return RuleResult.Invalid("Name is too short or too long (max 50 characters)");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditPrefix,
                              () =>
                              {
                                  if (EditPrefix != null && EditPrefix.Length > 45)
                                  {
                                      return RuleResult.Invalid("Prefix is too long (max 45 characters)");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditSurname,
                              () =>
                              {
                                  if (EditSurname == null || EditSurname.Length > 50 || EditSurname.Length < 1)
                                  {
                                      return RuleResult.Invalid("Surname is too short or too long (max 50 characters)");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditFIGNumber,
                              () =>
                              {
                                  float temp;
                                  if (EditFIGNumber == null || !float.TryParse(EditFIGNumber, out temp) || EditFIGNumber.Length < 1)
                                  {
                                      return RuleResult.Invalid("FIG Number must be a number and cannot be empty");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditDateOfBirth,
                              () =>
                              {
                                  DateTime result;
                                  String[] formats = { "yyyy-MM-dd", "dd-MM-yyyy" };
                                  if (EditDateOfBirth != null && EditDateOfBirth.Length > 0 && !DateTime.TryParseExact(EditDateOfBirth, formats, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                                  {
                                      return RuleResult.Invalid("Date of birth must be a date");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditNationality,
                              () =>
                              {
                                  if (EditNationality != null && EditNationality.Length > 45)
                                  {
                                      return RuleResult.Invalid("Nationality is too long (max 45 characters)");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditGender,
                              () =>
                              {
                                  if (EditGender != null && (EditGender.Length > 1 || EditGender.Length < 1))
                                  {
                                      return RuleResult.Invalid("Gender is too short or too long (max 1 characters)");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });

            Validator.AddRule(() => EditWeight,
                              () =>
                              {
                                  float temp;
                                  if (EditWeight != null && EditWeight.Length > 0 && (!float.TryParse(EditWeight, out temp) || EditWeight.Contains(",") || EditWeight.Contains(".")))
                                  {
                                      return RuleResult.Invalid("Weight must be a number and may not contain decimals");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });
            
            Validator.AddRule(() => EditLength,
                              () =>
                              {
                                  float temp;
                                  if (EditLength != null && EditLength.Length > 0 && (!float.TryParse(EditLength, out temp) || EditLength.Contains(",") || EditLength.Contains(".")))
                                  {
                                      return RuleResult.Invalid("Length must be a number and may not contain decimals");
                                  }
                                  else
                                  {
                                      return RuleResult.Valid();
                                  }
                              });
        }
        #endregion
    }
}