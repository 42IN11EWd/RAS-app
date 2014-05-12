using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RunApproachStatistics.ViewModel
{
    public class LoginViewModel : ValidationViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase content;
        private string username;
        private string errorMessage;
        private PasswordBox _passwordBox;


        #region DataBinding

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand LoginCommand { get; private set; }


        public PropertyChangedBase Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        [Required(ErrorMessage="Please fill in a username")]
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                ValidateProperty(value);
                OnPropertyChanged("Username");
            }
        }

        public PasswordBox PasswordBox
        {
            get { return _passwordBox; }
            set
            {
                _passwordBox = value;
            }
        }

        public string Password
        {
            get
            {
                return _passwordBox.Password;
            }
        }

        #endregion

        public LoginViewModel(IApplicationController app) : base()
        {
            _app = app;
        }

        #region RelayCommands

        public void CancelAction(object commandParam)
        {
            _app.CloseLoginWindow();
        }
        public void LoginAction(object commandParam)
        {
            UserModule usermodule = new UserModule();
            Console.WriteLine("pass: " + Password);

            // Validate username, can't bind password
            Validate();
            if (IsValid)
            {
                if (!usermodule.login(username, Password))
                {
                    ErrorMessage = "Invalid credentials";
                }
                else
                {
                    _app.IsLoggedIn = true;
                    _app.CloseLoginWindow();
                }
            }
            else
            {
                ErrorMessage = "Please fill in all fields";
            }
        }

        #endregion

        protected override void initRelayCommands()
        {
            CancelCommand = new RelayCommand(CancelAction);
            LoginCommand = new RelayCommand(LoginAction);
        }
    }
}
