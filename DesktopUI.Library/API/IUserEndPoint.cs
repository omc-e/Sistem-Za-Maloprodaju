using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopUI.Library.API
{
    public interface IUserEndPoint
    {
        Task<List<UserModel>> GetAll();
    }
}