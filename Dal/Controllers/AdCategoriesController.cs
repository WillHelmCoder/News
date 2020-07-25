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
    public class AdCategoriesController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;

        public AdCategoriesController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            InfluencerService = influencerService;
        }

        private EFDataBase db = new EFDataBase();

        [HttpGet]
        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = int.Parse(Request.Cookies["MagazineId"].Value);

            return View(db.AdCategoriesList.Where(x => !x.IsDeleted).Where(x => x.MagazineId == magId).Include(a => a.Magazine).ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AdCategory adCategory = db.AdCategoriesList.Find(id);

            if (adCategory == null)
                return HttpNotFound();

            return View(adCategory);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = int.Parse(Request.Cookies["MagazineId"].Value);
            var user = UserService.GetCurrentUser();

            if (!UserService.UserInMagazine(magId, user.UserId)) return RedirectToAction("Index", "Magazines");

            ViewBag.Keywords = Keywords_GetForAdGroup(db.NewsesList.Where(x => x.Category.MagazineId == magId).ToList()).OrderByDescending(x => x.Value);

            return View(new AdCategory
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                MagazineId = magId
            });
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "AdCategoryId,Name,Tags,StartDate,EndDate,Active,MagazineId")] AdCategory adCategory)
        {
            if (ModelState.IsValid)
            {
                db.AdCategoriesList.Add(adCategory);
                db.SaveChanges();

                SetMessage("Campaña creada", BootstrapAlertTypes.Success);
                return RedirectToAction("Index");
            }
            
            ViewBag.Keywords = Keywords_GetForAdGroup(db.NewsesList.Where(x => x.Category.MagazineId == adCategory.MagazineId).ToList()).OrderByDescending(x => x.Value);

            SetMessage("Ha ocurrido un error al crear la campaña", BootstrapAlertTypes.Danger);
            return View(adCategory);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AdCategory adCategory = db.AdCategoriesList.Find(id);
            if (adCategory == null)
                return HttpNotFound();

            ViewBag.Keywords = Keywords_GetForAdGroup(db.NewsesList.Where(x => x.Category.MagazineId == adCategory.MagazineId).ToList()).OrderByDescending(x => x.Value);

            return View(adCategory);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "AdCategoryId,Name,Tags,StartDate,EndDate,Active,MagazineId")] AdCategory adCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adCategory).State = EntityState.Modified;
                db.SaveChanges();

                SetMessage("Campaña editada", BootstrapAlertTypes.Success);
                return RedirectToAction("Index");
            }

            ViewBag.Keywords = Keywords_GetForAdGroup(db.NewsesList.Where(x => x.Category.MagazineId == adCategory.MagazineId).ToList()).OrderByDescending(x => x.Value);

            SetMessage("Ha ocurrido un error al editar la campaña", BootstrapAlertTypes.Danger);
            return View(adCategory);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            AdCategory adCategory = db.AdCategoriesList.Find(id);
            if (adCategory == null)
                return HttpNotFound();

            return View(adCategory);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdCategory adCategory = db.AdCategoriesList.Find(id);

            adCategory.IsDeleted = true;

            db.SaveChanges();

            SetMessage("Campaña eliminada", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public class Keyword_Model
        {
            public string Key { get; set; }
            public int Count { get; set; }
        }

        public Dictionary<string, int> Keywords_GetForAdGroup(List<Dal.Models.News> articles)
        {
            var keywords = "";
            foreach (var article in articles)
            {
                keywords += article.Keywords + ",";
            }

            var modelList = new List<Keyword_Model>();
            foreach (var item in keywords.Split(','))
            {
                modelList.Add(new Keyword_Model()
                {
                    Key = item
                });
            }

            var groupedList = modelList.GroupBy(x => x.Key).ToList();

            var returnModel = new Dictionary<string, int>();
            foreach (var item in groupedList)
            {
                returnModel.Add(item.Key, item.Count());
            }

            return returnModel;
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
