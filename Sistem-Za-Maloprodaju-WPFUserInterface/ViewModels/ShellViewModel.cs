using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Sistem_Za_Maloprodaju_WPFUserInterface.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel _loginVM;
        //Bootsrapper is sending instance through dependency injection 
        //We do not need to create object with "new LoginViewModel" 
        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;

            ActivateItemAsync(_loginVM);
             
                
        }
    }
}
