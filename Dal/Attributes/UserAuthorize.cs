using System;
using System.Web;
using System.Web.Mvc;
using Dal.Classes;

namespace Dal.Attributes
{
    public class UserAuthorize : AuthorizeAttribute
    {
        protected override Boolean AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Request.Cookies[Configuration.UserCookie] != null;
        }

    }
}