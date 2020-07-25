[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Dal.App_Start.NinjectWeb), "Start")]

namespace Dal.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject.Web.Common;
    public class NinjectWeb
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }
    }
}