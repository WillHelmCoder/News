using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.ApplicationServices;
using System.Linq;
using System.Net;

using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Dal.Repositories;
using Dal.Services;
using System.Web;
using News.Models;
using RestSharp;

namespace Dal.ApiControllers
{
    [EnableCors("*", "Accept,Origin,Content-Type", "GET,POST,PUT,DELETE")]
    public class NewsletterController : BaseApiController
    {
        public NewsletterController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService) 
            : base(userService, magazineService, resourceService, influencerService, galeryService, listService) { }

        private EFDataBase db = new EFDataBase();

        //private static void EnableCrossSiteRequests(HttpConfiguration config)
        //{
        //    var cors = new EnableCorsAttribute(
        //        origins: "*",
        //        headers: "*",
        //        methods: "*");
        //    config.EnableCors(cors);
        //}

        // PUT: api/News/5
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

        // POST: api/News
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

        // DELETE: api/News/5
        public IHttpActionResult DeleteNews(int id)
        {
            var news = db.NewsesList.Find(id);
            if (news == null)
            {
                return NotFound();
            }

            db.NewsesList.Remove(news);
            db.SaveChanges();

            return Ok(news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsExists(int id)
        {
            return db.NewsesList.Count(e => e.NewsId == id) > 0;
        }

        
        [Route("api/Newsletter/Subscribe")]
        public HttpResponseMessage Subscribe(Newsletter_Sub model)
        {
            var subscription = magazineService.ApiSubscribeEmail(model.mGuid, model.email);

            if (subscription == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateNewsletterSubscription(subscription));
        }

        public class Newsletter_Sub
        {
            public string mGuid { get; set; }
            public string email { get; set; }
        }
    }
}