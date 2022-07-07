
using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.Library.API
{
    public interface IAPIHelper
    {
        Task<AuthanticatedUser> Authenticate(string username, string password);

         Task GetLoggedInUserInfo(string token);
    }
}