using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dal.Classes;
using Dal.Models;
using Dal.Repositories;
using Ninject;

namespace Dal.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        [Inject]
        public Repository Repository { set; get; }

        protected override Boolean AuthorizeCore(HttpContextBase httpContext)
        {
            if (Configuration.DalEnvironment == DalEnvironmentTypes.Development)
            {
                return Repository.Users().SingleOrDefault(x => x.Email.Equals(httpContext.User.Identity.Name, StringComparison.OrdinalIgnoreCase)) != null;
            }
            else
            {
                return base.AuthorizeCore(httpContext);
            }
        }
    }
}