using Dal.Classes;
using Dal.Repositories;
using Dal.Services;
using Ninject.Modules;
using Ninject.Web.Common;
using Dal.Interfaces;

namespace Dal.App_Start
{
    public class ProductionModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IDataBase>().To<EFDataBase>().InRequestScope();
            Kernel.Bind<IMembershipService>().To<MembershipService>().InRequestScope();
            Kernel.Bind<IRoleService>().To<Dal.Services.RoleServicecs>().InRequestScope();
            Kernel.Bind<ILog>().To<Logger>().InRequestScope();

            Kernel.Bind<Repository>().ToSelf().InRequestScope();
            Kernel.Bind<UserService>().ToSelf().InRequestScope();
            Kernel.Bind<InfluencerService>().ToSelf().InRequestScope();
            Kernel.Bind<MagazineService>().ToSelf().InRequestScope();
            Kernel.Bind<ResourceService>().ToSelf().InRequestScope();
        }
    }
}