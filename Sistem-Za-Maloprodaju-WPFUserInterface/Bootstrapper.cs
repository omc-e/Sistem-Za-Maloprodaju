using Caliburn.Micro;
using Sistem_Za_Maloprodaju_WPFUserInterface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sistem_Za_Maloprodaju_WPFUserInterface
{
    public class Bootstrapper : BootstrapperBase 
    {

        public Bootstrapper()
        {
            Initialize();
        }


            protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }


    }
    

