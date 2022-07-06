using Sistem_Za_Maloprodaju_WPFUserInterface.Models;
using System.Threading.Tasks;

namespace Sistem_Za_Maloprodaju_WPFUserInterface.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthanticatedUser> Authenticate(string username, string password);
    }
}