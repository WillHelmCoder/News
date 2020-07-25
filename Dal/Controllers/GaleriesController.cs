using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace News.Controllers
{
    public class GaleriesController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;
        private readonly GaleryService GaleryService;

        public GaleriesController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService, GaleryService galeryService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            InfluencerService = influencerService;
            GaleryService = galeryService;
        }

        private EFDataBase db = new EFDataBase();

        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var galeriesList = db.GaleriesList.Where(x => x.MagazineId == magId).Where(x => !x.IsDeleted).Include(g => g.Magazine);
            return View(galeriesList.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Galery galery = db.GaleriesList.Find(id);
            if (galery == null)
            {
                return HttpNotFound();
            }
            return View(galery);
        }

        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }

            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "GaleryId,Title,Description,Alt,MetaDesc,Keywords,Permalink,MagazineId")] Galery galery)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            galery.MagazineId = magId;
            galery.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.GaleriesList.Add(galery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(galery);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Galery galery = db.GaleriesList.Find(id);
            if (galery == null)
            {
                return HttpNotFound();
            }
            return View(galery);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "GaleryId,Title,Description,Alt,MetaDesc,Keywords,Permalink,CreationDate")] Galery galery)
        {
            Galery model = db.GaleriesList.Find(galery.GaleryId);

            model.Alt = galery.Alt;
            model.Description = galery.Description;
            model.MetaDesc = galery.MetaDesc;
            model.Title = galery.Title;
            model.Keywords = galery.Keywords;

            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(galery);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Galery galery = db.GaleriesList.Find(id);
            if (galery == null)
            {
                return HttpNotFound();
            }
            return View(galery);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Galery galery = db.GaleriesList.Find(id);

            galery.IsDeleted = true;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Images(Int32 id)
        {
            var galeryImages = db.GaleryImagesList.Where(x => x.GaleryId == id).Where(x => !x.IsDeleted).OrderBy(x => x.Order).ToList();

            var model = new List<GaleryImageViewModel>();

            foreach (var item in galeryImages)
            {
                var image = db.ImagesList.Find(item.ImageId);

                var modelItem = new GaleryImageViewModel()
                {
                    GaleryImageId = item.GaleryImageId,
                    Image = image,
                    ImageId = image.ImageId,
                    Title = item.Title,
                    Description = item.Description,
                    Order = item.Order
                };

                model.Add(modelItem);
            }

            ViewBag.GaleryId = id;
            ViewBag.ImageId = new SelectList(db.ImagesList.OrderBy(x => x.Title), "ImageId", "Code");

            return View(model);
        }

        [HttpPost]
        public ActionResult Images(GaleryImage model, Int32 isAnEdit)
        {
            Boolean forEdit = false;
            if (isAnEdit == 1)
                forEdit = true;

            if (!GaleryService.AddImageToGalery(model, forEdit))
            {
                SetMessage("Lo sentimos, no hemos podido agregar la imagen a la galería. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            return RedirectToAction("Images", new { id = model.GaleryId });
        }

        [HttpGet]
        public ActionResult DeleteGaleryImage(Int32 id)
        {
            var item = GaleryService.GetGaleryImageById(id);

            return View(item);
        }

        [HttpPost]
        public ActionResult DeleteGaleryImage(GaleryImageViewModel model)
        {
            if (!GaleryService.DeleteGaleryImage(model.GaleryImageId))
            {
                SetMessage("Lo sentimos, no hemos podido eliminar la imagen de la galería. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            SetMessage("Imagen de la galería eliminada exitosamente.", BootstrapAlertTypes.Success);
            return RedirectToAction("Add", new { id = model.GaleryId });
        }

        public ActionResult Add(Int32 id)
        {
            ViewBag.GalleryId = id;
            ViewBag.CurrentImages = db.GaleryImagesList.Where(x => !x.IsDeleted).Where(x => x.GaleryId == id).Include(x => x.Image).ToList();

            return View();
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
