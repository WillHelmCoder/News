using System.Web.Http;
using Dal.Models;
using Dal.Services;

namespace Dal.ApiControllers
{
    public class BaseApiController : ApiController
    {
        protected readonly UserService userService;
        protected readonly MagazineService magazineService;
        protected readonly ResourceService resourceService;
        protected readonly InfluencerService influencerService;
        protected readonly GaleryService galeryService;
        protected readonly ListService listService;

        private ModelFactory modelFactory;
        
        public BaseApiController( UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
        {
            this.userService = userService;
            this.magazineService = magazineService;
            this.resourceService = resourceService;
            this.influencerService = influencerService;
            this.galeryService = galeryService;
            this.listService = listService;
        }        

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (this.modelFactory == null)
                {
                    this.modelFactory = new ModelFactory();
                }
                return this.modelFactory;
            }
        }
    }
}
