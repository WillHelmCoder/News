using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dal.Attributes;
using Dal.Classes;

namespace Dal.AuthControllers
{
    [CustomAuthorize]
    [UserAuthorize]
    public abstract class UserAuthController : BasicController
    {
        public UserAuthController() { }
       

        protected String UserEmail
        {
            get
            {
                return EncryptionService.Decrypt(Request.Cookies[Configuration.UserCookie]["Email"]);
            }
        }

        protected String UserCode
        {
            get
            {
                return EncryptionService.Decrypt(Request.Cookies[Configuration.UserCookie]["Code"]);
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.UserEmail = UserEmail;
            ViewBag.UserCode = UserCode;

            base.OnActionExecuting(filterContext);
        }
    }
}