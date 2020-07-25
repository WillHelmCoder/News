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
    public class CategoriesController : BaseApiController
    {
        public CategoriesController(UserService userService, MagazineService magazineService, ResourceService resourceService, InfluencerService influencerService, GaleryService galeryService, ListService listService)
            : base(userService, magazineService, resourceService, influencerService, galeryService, listService) { }

        private EFDataBase db = new EFDataBase();

        [Route("api/Categories")]
        public HttpResponseMessage GetCategoriesList()
        {
            var items = magazineService.GetAllCategories();

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/Categories/{id:int}")]
        public HttpResponseMessage GetCategory(int id)
        {
            var category = magazineService.GetCategoryById(id);

            if (category == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la categoría.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(category));
        }

        [Route("api/Categories/MagazineCategories/{id:int}")]
        public HttpResponseMessage GetCategoriesByMagazineId(int id)
        {
            var magazine = magazineService.GetMagazineById(id);

            if (magazine == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontró la revista.");
            }

            var items = magazineService.GetCategoriesByMagazineId(id);

            if (items == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encontraron elementos.");
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        [Route("api/{mGuid}/Categories/GetCategoriesByMagazine")]
        public HttpResponseMessage GetMagazineCategories(string mGuid)
        {
            var magazine = magazineService.ApiGetMagazine(mGuid);

            if (magazine == null)
                return Request.CreateErrorResponse(HttpStatusCode.OK, "No se encontró la revista.");

            var items = magazineService.ApiGetCategoriesByMagazine(mGuid);

            if (items == null)
                return Request.CreateErrorResponse(HttpStatusCode.OK, "No se encontraron elementos.");

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(items));
        }

        //[Route("api/{mGuid}/Categories/Edit")]
        //public HttpResponseMessage PutCategory(Category category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos.");
        //    }

        //    db.Entry(category).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error al actualizar la categoria");
        //    }

        //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ha ocurrido un error");
        //}

        [Route("api/{mGuid}/Categories/Create")]
        public HttpResponseMessage PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Faltan datos.");
            }

            if (category.CategoryId != 0)
            {
                db.Entry(category).State = EntityState.Modified;
            }
            else
            {
                db.CategoriesList.Add(category);
            }

            db.SaveChanges();

            return Request.CreateErrorResponse(HttpStatusCode.OK, "Categoria creada");
        }

        [Route("api/{mGuid}/Categories/Delete/{id:int}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public HttpResponseMessage DeleteCategory(int id)
        {
            Category category = db.CategoriesList.Find(id);
            if (category == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.OK, "Categoria no encontrada");
            }

            category.IsDeleted = true;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();

            return Request.CreateErrorResponse(HttpStatusCode.OK, "Categoria eliminada");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.CategoriesList.Count(e => e.CategoryId == id) > 0;
        }
    }
}