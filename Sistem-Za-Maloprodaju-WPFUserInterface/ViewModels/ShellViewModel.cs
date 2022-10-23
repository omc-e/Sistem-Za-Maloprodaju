using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Sistem_Za_Maloprodaju_WPFUserInterface.EventModels;

namespace Sistem_Za_Maloprodaju_WPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        
        public IEventAggregator _events;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;
        //Bootsrapper is sending instance through dependency injection 
        //We do not need to create object with "new LoginViewModel" 
        public ShellViewModel( IEventAggregator events, SalesViewModel salesVM)
        {
            _events = events;
           
            _salesVM = salesVM;
            

            _events.SubscribeOnUIThread(this);

            //IoC allows us to get instance through the cotainer 
            ActivateItemAsync(IoC.Get<LoginViewModel>());
             
                
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken) => ActivateItemAsync(_salesVM);

        //public void Handle(LogOnEvent message)
        //{


        //}
    }
}
