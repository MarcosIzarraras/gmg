using Microsoft.AspNetCore.Mvc.Rendering;

namespace GMGv2.Extensions
{
    public static class HtmlHelpers
    {
        public static string IsActive(this IHtmlHelper html, string? controller = null, string? action = null)
        {
            var routeData = html.ViewContext.RouteData;

            string? routeAction = routeData?.Values["action"]?.ToString();
            string? routeController = routeData?.Values["controller"]?.ToString();

            bool controllerMatch = controller == null || controller.ToLower() == routeController?.ToLower();
            bool actionMatch = action == null || action.ToLower() == routeAction?.ToLower();

            return controllerMatch && actionMatch ? "active" : "";
        }
    }
}
