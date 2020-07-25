using System.Web.Http;
using System.Web.Http.Cors;
using Dal.Repositories;
using Dal.Services;
using System.Net.Http;
using System.Net;
using System;

namespace Dal.ApiControllers
{
    public class GaleriesController : BaseApiController
    {
        #region InjectionAndCors
        public GaleriesController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
            : base(userService, magazineService, resourceService, influencerService, galeryService, listService) { }

        private EFDataBase db = new EFDataBase();

        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*");
            config.EnableCors(cors);
        }
        #endregion

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Galeries/GetGaleries_ByMag")]
        public HttpResponseMessage GetGaleries_ByMag(string mGuid)
        {
            var galeries = galeryService.GetGaleries_ByMGuid(mGuid);

            if (galeries.Count == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lo sentimos, no hay galerías para mostrar.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateGaleries_ResponseModel(galeries));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Galeries/GetGalery_ById/{id:int}")]
        public HttpResponseMessage GetGalery_ByMag(string mGuid, Int32 id)
        {
            var galery = galeryService.GetGalery_ByMGuid(mGuid, id);

            if (galery == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lo sentimos, no hay galería para mostrar.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateGalery_ResponseModel(galery));
        }

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}