using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dal.Models;
using Dal.Repositories;
using Dal.AuthControllers;
using Dal.ViewModels;
using Dal.Services;

namespace News.Controllers
{
    public class DatasController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;

        public DatasController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            InfluencerService = influencerService;
        }

        private EFDataBase db = new EFDataBase();

        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            return View(db.DatasList.Where(x => x.MagazineId == magId).Where(x => !x.IsDeleted).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Data data = db.DatasList.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            ViewBag.DataParentId = new SelectList(db.Datas().Where(x => x.MagazineId == magId).Where(x => !x.IsDeleted).Where(x => x.DataParentId == null), "DataId", "Title");

            return View();
        }

        [HttpPost]
        public ActionResult Create(DataViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var imageCode = "Expose_Default_New.png";
            var imageSignCode = "Expose_Default_New.png";

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), model.Image);

                if (imageModel == null)
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");
                    return View(model);
                }
                imageCode = imageModel.FullFileName;
            }

            if (model.ImageSign != null && model.ImageSign.ContentLength > 0)
            {
                var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), model.ImageSign);

                if (imageModel == null)
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");
                    return View(model);
                }
                imageSignCode = imageModel.FullFileName;
            }

            var create = MagazineService.CreateData(model, magId, imageCode, imageSignCode);

            ViewBag.DataParentId = new SelectList(db.Datas().Where(x => x.MagazineId == magId).Where(x => !x.IsDeleted).Where(x => x.DataParentId == null).ToList(), "DataId", "Title", model.DataParentId);

            if (!create)
            {
                SetMessage(MagazineService.ServiceTempData);
                SetMessage("Ocurrió un error inesperado. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            SetMessage("Dato ha sido creado exitosamente.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.DatasList.Find(id);
            var model = new DataViewModel()
            {
                DataId = data.DataId,
                DataParentId = data.DataParentId,
                Description = data.Description,
                ImageString = data.Image,
                ImageSignString = data.imageSign,
                Order = data.Order,
                Title = data.Title,
                Url = data.Url
            };
            if (data == null)
            {
                return HttpNotFound();
            }

            ViewBag.DataParentId = new SelectList(db.Datas().Where(x => x.MagazineId == magId).Where(x => !x.IsDeleted).Where(x => x.DataId != id).Where(x => x.DataParentId == null).ToList(), "DataId", "Title", data.DataParentId);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DataViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var imageCode = model.ImageString;
            var imageSignCode = model.ImageSignString;

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), model.Image);

                if (imageModel == null)
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");
                    return View(model);
                }

                imageCode = imageModel.FullFileName;
            }

            if (model.ImageSign != null && model.ImageSign.ContentLength > 0)
            {
                var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), model.ImageSign);

                if (imageModel == null)
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");
                    return View(model);
                }

                imageSignCode = imageModel.FullFileName;
            }

            var create = MagazineService.CreateData(model, magId, imageCode, imageSignCode);

            ViewBag.DataParentId = new SelectList(db.Datas().Where(x => x.MagazineId == magId).Where(x => x.DataId != model.DataId).Where(x => !x.IsDeleted).Where(x => x.DataParentId == null).ToList(), "DataId", "Title", model.DataParentId);

            if (!create)
            {
                SetMessage(MagazineService.ServiceTempData);
                SetMessage("Ocurrió un error inesperado. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            SetMessage("Dato ha sido editado exitosamente.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Data data = db.DatasList.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Data data = db.DatasList.Find(id);

            data.IsDeleted = true;

            db.SaveChanges();
            return RedirectToAction("Index");
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
