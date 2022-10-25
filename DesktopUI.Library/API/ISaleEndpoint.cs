using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.Library.API
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}