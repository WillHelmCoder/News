using System.Web.Http;
using System.Web.Http.Cors;
using Dal.Repositories;
using Dal.Services;
using System.Net.Http;
using System.Net;
using System;
using Dal.Models;

namespace Dal.ApiControllers
{
    public class ListsController : BaseApiController
    {
        #region InjectionAndCors
        public ListsController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
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
        [Route("api/{id}/Galeries/GetListOfItemsByMagId")]
        public HttpResponseMessage GetListOfItemsByMagId(Int32 id)
        {
            var list = listService.GetListOfItemsByMagazineId(id);

            if (list.Count == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lo sentimos, no hay listas para mostrar.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateList(list));
        }

        [AcceptVerbs("POST")]
        [Route("api/ItemList/Save")]
        public HttpResponseMessage ItemList_Save(ItemList model)
        {
            try
            {
                var itemList = listService.CreateListOfItems(model);

                if (itemList == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lo sentimos, no ha sido posible completar la opearción.");

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Lo sentimos, no ha sido posible completar la opearción.");
            }
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