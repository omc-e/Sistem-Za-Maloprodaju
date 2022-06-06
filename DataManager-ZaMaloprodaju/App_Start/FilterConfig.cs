using System.Web;
using System.Web.Mvc;

namespace DataManager_ZaMaloprodaju
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
