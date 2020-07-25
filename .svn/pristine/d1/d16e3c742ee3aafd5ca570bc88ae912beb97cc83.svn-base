using Dal.Repositories;
using Dal.Services;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Dal.ApiControllers
{
    public class NewsController : BaseApiController
    {
        public NewsController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
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

        [ResponseType(typeof(void))]
        public IHttpActionResult PutNews(int id, Dal.Models.News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != news.NewsId)
            {
                return BadRequest();
            }

            db.Entry(news).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult PostNews(Dal.Models.News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NewsesList.Add(news);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = news.NewsId }, news);
        }

        [Route("api/{mGuid}/Articles/Create_Test")]
        public HttpResponseMessage PostNew(Models.News article)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos.");

            if (article.NewsId != 0)
            {
                var articleObj = db.NewsesList.Find(article.NewsId);

                articleObj.Title = article.Title;

                db.Entry(articleObj).State = EntityState.Modified;
            }
            else
                db.NewsesList.Add(article);

            db.SaveChanges();

            return Request.CreateErrorResponse(HttpStatusCode.OK, "Listo");
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Articles/Delete/{id:int}")]
        public HttpResponseMessage DeleteNew(int id)
        {
            Models.News article = db.NewsesList.Find(id);
            if (article == null)
                return Request.CreateErrorResponse(HttpStatusCode.OK, "Articulo no encontrado");

            article.IsDeleted = true;
            db.Entry(article).State = EntityState.Modified;
            db.SaveChanges();

            return Request.CreateErrorResponse(HttpStatusCode.OK, "Articulo eliminado");
        }

        private bool NewsExists(int id)
        {
            return db.NewsesList.Count(e => e.NewsId == id) > 0;
        }

        [Route("api/News/NewsWithVisitsByMagazineId/{id:int}")]
        public HttpResponseMessage GetNewsWithVisitsByMagazineId(int id)
        {
            var magazine = magazineService.GetMagazineById(id);

            if (magazine == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la revista.");
            }

            var items = magazineService.GetNewsWithVisitsByMagazineId(id);
            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/News")]
        public HttpResponseMessage GetNewsesList()
        {
            var items = magazineService.GetAllNewses();

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/News/{id:int}")]
        public HttpResponseMessage GetNews(int id)
        {
            var news = magazineService.GetNewsById(id);

            if (news == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");

            var ads = magazineService.Api_Ads_GetByTags(news.Keywords, news.Category.MagazineId);

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Article_Response(news, ads));
        }

        [Route("api/News/{id:int}/{permalink}")]
        public HttpResponseMessage GetNewsByIdAndPermalink(int id, string permalink)
        {
            var article = magazineService.GetNewsByIdAndPermalink(id, permalink);

            if (article == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");

            var ads = magazineService.Api_Ads_GetByTags(article.Keywords, article.Category.MagazineId);

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Article_Response(article, ads));
        }

        [Route("api/News/GetByTag/{tag}")]
        public HttpResponseMessage GetNewByTag(string tag)
        {
            var articles = magazineService.GetNewsesByTag(tag);

            if (articles == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(articles));
        }

        [Route("api/News/GetByTagFromUpgrade/{tag}")]
        public HttpResponseMessage GetNewByTagUpgrade(string tag)
        {
            var news = magazineService.GetNewsesByTag(tag);

            if (news == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateModelUpgrade(news));
        }

        [HttpPost]
        [AcceptVerbs("POST", "GET")]
        [Route("api/News/CreateVisit/{tag}/{id:int}/{idUser:int}/{ip}/{cookie}")]
        public HttpResponseMessage CreateVisit(string tag, int id, int? idUser, string ip, string cookie)
        {
            var date = DateTime.Now;
            if (!HttpContext.Current.Request.Browser.Crawler && !HttpContext.Current.Request.UserHostAddress.Contains("127.0.0"))
            {
                var create = influencerService.CreateVisit(id, idUser, ip, date, tag, cookie);

                if (create != null)
                    return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("api/News/NewsByCategory/{id:int}")]
        public HttpResponseMessage GetNewsByCategory(int id)
        {
            var category = magazineService.GetCategoryById(id);

            if (category == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la categoría.");
            }

            var items = magazineService.GetNewsesByCategoryId(id);

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/News/NewsByMagazine/{id:int}")]
        public HttpResponseMessage GetNewsByMagazineId(int id)
        {
            var magazine = magazineService.GetMagazineById(id);

            if (magazine == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la revista.");
            }

            var items = magazineService.GetNewsesByMagazineId(id);

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/News/PublicMagazinesNews")]
        public HttpResponseMessage GetPublicMagazinesNews()
        {
            var items = magazineService.GetAllPublicMagsNews();
            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/News/GetCatByMagazine/{id:int}")]
        public HttpResponseMessage GetCatByMagazine(int id)
        {
            var items = magazineService.GetCategoriesByMagazineId(id);

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/News/GetSharedNews")]
        public HttpResponseMessage GetSharedNewsByUserId()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tienes que iniciar sesion.");
            }

            var items = influencerService.GetVisitsByInfluencerId(user.UserId);

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));

        }

        [Route("api/News/GetFbVisits")]
        public HttpResponseMessage GetFbVisitByMonth()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tienes que iniciar sesion.");
            }

            var items = influencerService.GetCounterByInfluencerAndMonth(user.UserId);

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Slider/GetSliderByMagazine")]
        public HttpResponseMessage GetMagazineSlider(string mGuid)
        {
            var slider = magazineService.ApiGetMagSlider(mGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(slider));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Quiz/GetQuizByMagazine")]
        public HttpResponseMessage GetQuizByMagazineId(string mGuid)
        {
            var quiz = magazineService.ApiGetQuizByMagazineId(mGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(quiz));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Galery/GetGaleryByMagazine")]
        public HttpResponseMessage GetGaleryByMagazineId(string mGuid)
        {
            var galery = magazineService.ApiGetGaleryByMagazineId(mGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateGaleries_ResponseModel(galery));
        }

        [HttpPost]
        [AcceptVerbs("POST", "GET")]
        [Route("api/{mGuid}/Answer/CreateAnswerByQuestionId")]
        public HttpResponseMessage CreateAnswer(int QuestionId, string Description, int? id)
        {

            var create = magazineService.CreateAnswer(QuestionId, Description, id);

            if (create != null)
                return Request.CreateResponse(HttpStatusCode.OK);

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Advertise/GetAdvertisesByMagazine")]
        public HttpResponseMessage GetAdvertisesByMagazine(string mGuid)
        {
            var advertise = magazineService.ApiGetAdsByMagGuid(mGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(advertise));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/GetNewsByMagazine")]
        public HttpResponseMessage GetMagazineNews(string mGuid)
        {
            var news = magazineService.ApiGetNewsByMagazine(mGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/ViewMagazineNew/{nId:int}")]
        public HttpResponseMessage ViewMagazineNew(string mGuid, int nId)
        {
            var article = magazineService.ApiGetNewById(mGuid, nId);

            if (article == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");

            var ads = magazineService.Api_Ads_GetByTags(article.Keywords, article.Category.MagazineId);

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Article_Response(article, ads));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/GetNewsByDateRange/{fromDate}/{toDate}")]
        public HttpResponseMessage GetNewsByDateRange(string mGuid, string fromDate, string toDate)
        {
            var news = magazineService.ApiGetNewsByDateRange(mGuid, fromDate, toDate);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/GetNewsByDateRangeAndTag/{fromDate}/{toDate}/{tag}")]
        public HttpResponseMessage GetNewsByDateRangeAndTag(string mGuid, string fromDate, string toDate, string tag)
        {
            var news = magazineService.ApiGetNewsByDateRangeAndTag(mGuid, fromDate, toDate, tag);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/GetNewsByCatId/{nId:int}")]
        public HttpResponseMessage GetNewsByCatId(string mGuid, int nId)
        {
            var news = magazineService.ApiGetNewsByCatId(mGuid, nId);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/GetLastNewsByCat/{nId:int}")]
        public HttpResponseMessage GetLastNewsByCatId(string mGuid, int nId)
        {
            var news = magazineService.ApiGetLastNewsByCatId(mGuid, nId);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/News/GetCommentsByNewId/{nId:int}")]
        public HttpResponseMessage GetCommentsByNewId(string mGuid, int nId)
        {
            var comments = magazineService.ApiGetComments(mGuid, nId);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(comments));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Ads/GetAdCategories")]
        public HttpResponseMessage GetAdCategories(string mGuid)
        {
            var cats = magazineService.ApiGetAdCategories(mGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(cats));
        }

        [AcceptVerbs("GET")]
        [Route("api/{mGuid}/Ads/GetAdvertisesByCatId/{aId:int}")]
        public HttpResponseMessage GetAdvertisesByCatId(string mGuid, int aId)
        {
            var ads = magazineService.ApiGetAdsByCatId(mGuid, aId);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(ads));
        }

        [Route("api/News/GetMarcoPazNews")]
        public HttpResponseMessage GetMarcoPazNews()
        {
            var news = magazineService.ApiGetMarcoPazNews();

            if (news == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        [Route("api/News/GetNewsByTagAndQty/{qty:int}/{tag}")]
        public HttpResponseMessage GetNewsByTagAndQty(int qty, string tag)
        {
            var news = magazineService.ApiGetNewsByTagAndQty(qty, tag);

            if (news == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la noticia.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(news));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}