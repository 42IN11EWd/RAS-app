using RunApproachStatistics.Controllers;
using RunApproachStatistics.Modules;
using RunApproachStatistics.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunApproachStatistics.ViewModel
{
    public class LoginViewModel : AbstractViewModel
    {
        private IApplicationController _app;
        private PropertyChangedBase content;
        private string username;
        private string password;
        private string errorMessage;


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

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
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
            if(!usermodule.login(username, password))
            {
                ErrorMessage = "Logincredentials are wrong";
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
