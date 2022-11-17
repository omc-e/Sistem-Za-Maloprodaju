using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI.Library.Models;
using DesktopUI.ViewModels;
using Sistem_Za_Maloprodaju_WPFUserInterface.EventModels;

namespace DesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {

        public IEventAggregator _events;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;
        //Bootsrapper is sending instance through dependency injection 
        //We do not need to create object with "new LoginViewModel" 
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _user = user;
            _salesVM = salesVM;
            _apiHelper = apiHelper;


            _events.SubscribeOnUIThread(this);

            //IoC allows us to get instance through the cotainer 
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());


        }

        public bool IsAccountVisible
        {
            get {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }
        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task LogOut()
        {
            _user.ResetUser();
            _apiHelper.LogOffUser();
           await ActivateItemAsync(IoC.Get<LoginViewModel>(),new CancellationToken());
            NotifyOfPropertyChange(() => IsAccountVisible);
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        //TODO
        //ADD NOTIFYONPROPERTYCHANGE IN TASK 
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM, cancellationToken);
            NotifyOfPropertyChange(() => IsAccountVisible);
        } 
             
        
        
        

        //public void Handle(LogOnEvent message)
        //{


        //}
    }
}
