using System.Web;
using System.Web.Mvc;

namespace MT.CSGPortal.Services
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
