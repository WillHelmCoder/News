using Dal.ApiControllers;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace News.ApiControllers
{
    public class SliderController : BaseApiController
    {
        public SliderController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
            : base(userService, magazineService, resourceService, influencerService, galeryService, listService) { }

        private static void EnableCrossSiteRequests(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*");
            config.EnableCors(cors);
        }

        [AcceptVerbs("GET")]
        [Route("api/{sguid}/Slider/GetSlider")]
        public HttpResponseMessage GetListOfItemsBySlider(String sguid)
        {
            var list = magazineService.GetSlider(sguid);

            if (list.Count == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lo sentimos, no hay listas para mostrar.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(list));
        }
    }
}
