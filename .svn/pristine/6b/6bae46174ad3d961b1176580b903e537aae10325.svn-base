using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using Dal.ViewModels;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace News.Controllers
{
    public class AdvertisesController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;

        public AdvertisesController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            InfluencerService = influencerService;
        }

        private EFDataBase db = new EFDataBase();

        private int? CheckMagRelation()
        {
            if (Request.Cookies["MagazineId"].Value == null) return null;
            int magId = int.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();

            if (!UserService.UserInMagazine(magId, user.UserId))
                return null;

            return magId;
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            var magId = CheckMagRelation();

            if (magId == null)
            {
                SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            ViewBag.AdCategoryId = id;

            var advertisesList = db.AdvertisesList.Where(x => x.AdCategoryId == id).Where(x => !x.IsDeleted).Include(a => a.AdCategory).Where(x => x.AdCategory.MagazineId == magId);
            return View(advertisesList.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertise advertise = db.AdvertisesList.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            return View(advertise);
        }

        [HttpGet]
        public ActionResult Create(int adCatId)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = int.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();

            if (!UserService.UserInMagazine(magId, user.UserId)) return RedirectToAction("Index", "Magazines");

            var adGroup = db.AdCategoriesList.Find(adCatId);

            return View(new AdvertiseViewModel()
            {
                AdCategoryId = adCatId,
                Source = "blog",
                Medium = "banner",
                Campaign = adGroup.Name,
                Term = !string.IsNullOrEmpty(adGroup.Tags) ? adGroup.Tags.Split(',').First() : "",
            });
        }

        [HttpPost]
        public ActionResult Create(AdvertiseViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            if (!UserService.UserInMagazine(int.Parse(Request.Cookies["MagazineId"].Value), UserService.GetCurrentUser().UserId)) return RedirectToAction("Index", "Magazines");

            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de verificar los datos. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            var imageCode = "Expose_Default_New.jpg";

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), model.Image, false);

                if (imageModel == null)
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");
                    return View(model);
                }
                imageCode = imageModel.FullFileName;
            }

            var create = MagazineService.CreateAd(model, imageCode, null);

            if (!create)
            {
                SetMessage("Ocurrió un error inesperado. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            SetMessage("Anuncio creado exitosamente", BootstrapAlertTypes.Success);
            return RedirectToAction("Index", new { id = model.AdCategoryId });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            if (!UserService.UserInMagazine(int.Parse(Request.Cookies["MagazineId"].Value), UserService.GetCurrentUser().UserId)) return RedirectToAction("Index", "Magazines");

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Advertise advertise = db.AdvertisesList.SingleOrDefault(x => x.AdvertiseId == id);

            if (advertise == null)
                return HttpNotFound();

            var model = new AdvertiseViewModel()
            {
                AdvertiseId = advertise.AdvertiseId,
                AdCategoryId = advertise.AdCategoryId.Value,
                Content = advertise.Content,
                IFrame = advertise.IFrame,
                ImageString = advertise.Image,
                Name = advertise.Name,
                Url = advertise.Url,
                Campaign = advertise.Campaign,
                Horizontal = advertise.Horizontal,
                Medium = advertise.Medium,
                Source = advertise.Source,
                Term = advertise.Term
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdvertiseViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            if (!UserService.UserInMagazine(int.Parse(Request.Cookies["MagazineId"].Value), UserService.GetCurrentUser().UserId)) return RedirectToAction("Index", "Magazines");

            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de verificar los datos. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            var imageCode = model.ImageString;

            if (model.Image != null && model.Image.ContentLength > 0)
            {
                var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), model.Image, false);

                if (imageModel == null)
                {
                    ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");
                    return View(model);
                }
                imageCode = imageModel.FullFileName;
            }

            if (!MagazineService.CreateAd(model, imageCode, model.AdvertiseId))
            {
                SetMessage("Ocurrió un error inesperado. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            SetMessage("Anuncio creado exitosamente", BootstrapAlertTypes.Success);
            return RedirectToAction("Index", new { id = model.AdCategoryId });
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertise advertise = db.AdvertisesList.Find(id);
            if (advertise == null)
            {
                return HttpNotFound();
            }
            return View(advertise);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Advertise advertise = db.AdvertisesList.Find(id);

            advertise.IsDeleted = true;

            db.SaveChanges();

            SetMessage("Anuncio eliminado exitosamente", BootstrapAlertTypes.Success);
            return RedirectToAction("Index", new { id = advertise.AdCategoryId });
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
