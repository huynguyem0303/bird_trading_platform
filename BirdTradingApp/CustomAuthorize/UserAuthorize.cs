using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BirdTradingApp.CustomAuthorize
{
    public class UserAuthorize : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var result = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(result) || result != "Customer")
            {
                context.Result = new RedirectToPageResult("/Login/Logout");
            }
        }
    }
}
