using Caliburn.Micro;
using DesktopUI.Library.API;
using DesktopUI.Library.Models;
using Sistem_Za_Maloprodaju_WPFUserInterface.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sistem_Za_Maloprodaju_WPFUserInterface.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IUserEndPoint _userEndPoint;

        BindingList<UserModel> _users;
        public BindingList<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IUserEndPoint userEndPoint)
        {
            _status = status;
            _window = window;
            _userEndPoint = userEndPoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {

                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                //var info = IoC.Get<StatusInfoViewModel>();

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales Form");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exceptiom", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }

                await TryCloseAsync();
            }
        }
        private async Task LoadUsers()
        {
            var userList = await _userEndPoint.GetAll();
            Users = new BindingList<UserModel>(userList);
            
        }
    }
}
