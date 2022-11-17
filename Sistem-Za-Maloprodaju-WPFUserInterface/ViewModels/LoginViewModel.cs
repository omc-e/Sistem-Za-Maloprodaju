using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI;
using Sistem_Za_Maloprodaju_WPFUserInterface.EventModels;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace DesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "emsar.omic@outlook.com";
        private string _password = "Emsar12345!";
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper aPIHelper, IEventAggregator events)
        {
            _apiHelper = aPIHelper;
            _events = events;
        }
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }



        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

       // private bool _isErrorVisible;

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output;
            }

        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }



        public bool CanLogIn
        {
            get
            {
                bool output = false;
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;

                }
                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);

                //Capture more information about user

                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
                await _events.PublishOnUIThreadAsync(new LogOnEvent(), new CancellationToken());

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }


        }



    }
}
