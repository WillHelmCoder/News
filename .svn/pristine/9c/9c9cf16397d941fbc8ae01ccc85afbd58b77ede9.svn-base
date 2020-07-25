using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Dal.ApiControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;

namespace Dal.ApiControllers
{
    [EnableCors("*", "Accept,Origin,Content-Type", "GET,POST,PUT,DELETE")]
    public class MagazinesController : BaseApiController
    {
        public MagazinesController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService) 
            : base(userService, magazineService, resourceService, influencerService, galeryService, listService) { }

        private EFDataBase db = new EFDataBase();

        [Route("api/Magazines")]
        public HttpResponseMessage GetMagazinesList()
        {
            var magazines = magazineService.GetAllMagazines();

            if (magazines == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron revistas.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(magazines));
        }

        [Route("api/Magazines/{id:int}")]
        public HttpResponseMessage GetMagazine(int id)
        {
            var magazine = magazineService.GetMagazineById(id);
            if (magazine == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la revista.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(magazine));
        }

        [Route("api/Magazines/GetMagazines/{code}")]
        public HttpResponseMessage GetMagazinesByCode(String code)
        {
            var magazine = magazineService.GetMagazineByCode(code);

            if (magazine == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró el restaurant.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(magazine));
        }

        [Route("api/Magazines/UserMagazines/{id:int}")]
        public HttpResponseMessage GetUserMagazines(int id)
        {
            var user = userService.GetById(id);

            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró el usuario.");
            }

            var items = magazineService.GetMagazinesByUserId(id);

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [HttpGet]
        [Route("api/Magazines/GetCities/{id:int}")]
        public HttpResponseMessage GetStateCities(int id)
        {
            var cities = resourceService.GetCities(id);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(cities));
        }

        // PUT: api/Magazines/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMagazine(int id, Magazine magazine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != magazine.MagazineId)
            {
                return BadRequest();
            }

            db.Entry(magazine).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MagazineExists(id))
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

        // POST: api/Magazines
        [ResponseType(typeof(Magazine))]
        public IHttpActionResult PostMagazine(Magazine magazine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MagazinesList.Add(magazine);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = magazine.MagazineId }, magazine);
        }

        // DELETE: api/Magazines/5
        [ResponseType(typeof(Magazine))]
        public IHttpActionResult DeleteMagazine(int id)
        {
            Magazine magazine = db.MagazinesList.Find(id);
            if (magazine == null)
            {
                return NotFound();
            }

            db.MagazinesList.Remove(magazine);
            db.SaveChanges();

            return Ok(magazine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MagazineExists(int id)
        {
            return db.MagazinesList.Count(e => e.MagazineId == id) > 0;
        }

        [HttpGet]
        [Route("api/{kGuid}/Magazines/GetKeyPointsByKguid/")]
        public HttpResponseMessage GetKeyPointsByKguid(String kGuid)
        {
            var cities = magazineService.GetsKeyPointsByGuid(kGuid);
            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(cities));
        }
    }
}