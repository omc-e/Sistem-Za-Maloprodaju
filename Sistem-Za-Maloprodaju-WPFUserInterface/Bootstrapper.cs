﻿using AutoMapper;
using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI.Library.Models;
using Sistem_Za_Maloprodaju_WPFUserInterface.Helpers;
using DesktopUI.Models;
using DesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Sistem_Za_Maloprodaju_WPFUserInterface
{
    //Caliburn.Micro Bootsrapper class
    public class Bootstrapper : BootstrapperBase 
    {
        //Simple container object to hold dependency mapping for use later in an app via Dependency Injection
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
                        
            ConventionManager.AddElementConvention<PasswordBox>(PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");

        }
        private IMapper ConfigureAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });

            var output = config.CreateMapper();
            return output;
        }

        protected override void Configure()
        {
           

            _container.Instance(ConfigureAutomapper());

            _container.Instance(_container)
                .PerRequest<IProductEndPoint, ProductEndPoint>()
                .PerRequest<IUserEndPoint, UserEndPoint>()
                .PerRequest<ISaleEndpoint, SaleEndpoint>();
                

            //based in calibrum micro
            //Window manager
            //EventAggergator, passing event messages  through eventAggregator
            //Singleton creating one instance for scope of this container

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()

                .Singleton<IAPIHelper,APIHelper>();

            //reflection
            //If you need to use multiple times it is better to use String Builder 
            //View models to connect to Views
            //Small performance hit on startup of application
            //Limit just to get class and name ends with viewmodel 
            GetType().Assembly.GetTypes()
                .Where (type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(),viewModelType));
            

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }


    }
    

