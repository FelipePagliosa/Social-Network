using System.Web;
using System.Web.Mvc;

namespace Felipe_Pagliosa_PB {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
