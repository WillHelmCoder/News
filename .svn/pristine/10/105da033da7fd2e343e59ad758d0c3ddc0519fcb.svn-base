using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace News.Controllers
{
    public class ReportsController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();
        private readonly UserService UserService;

        public ReportsController(UserService userService)
        {
            UserService = userService;
        }

        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var reportsList = db.ReportsList.Where(x => x.MagazineId == magId).Include(r => r.Media);

            return View(reportsList.ToList());
        }


        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            ViewBag.NewsId = new SelectList(db.NewsesList.Where(x => x.Category.MagazineId == magId).Where(x => !x.IsDeleted), "NewsId", "Title");
            ViewBag.MediaId = new SelectList(db.MediasList.Where(x => !x.IsDeleted), "MediaId", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "ReportId,Title,NewsId,IsPositive,IsPaid,Impact,Comments,MediaId,MagazineId,IsDeleted")] Report report)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            report.MagazineId = magId;

            if (ModelState.IsValid)
            {
                db.ReportsList.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NewsId = new SelectList(db.NewsesList.Where(x => x.Category.MagazineId == magId).Where(x => !x.IsDeleted), "NewsId", "Title");
            ViewBag.MediaId = new SelectList(db.MediasList.Where(x => !x.IsDeleted), "MediaId", "Name");
            return View(report);
        }

        public ActionResult Edit(int? id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.ReportsList.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }

            var model = new ReportViewModel
            {
                Comments = report.Comments,
                Impact = report.Impact,
                IsPaid = report.IsPaid,
                IsPositive = report.IsPositive,
                MagazineId = magId,
                MediaId = report.MediaId,
                ReportId = report.ReportId,
                Title = report.Title,
            };

            ViewBag.NewsId = new SelectList(db.NewsesList.Where(x => x.Category.MagazineId == magId).Where(x => !x.IsDeleted), "NewsId", "Title");
            ViewBag.MediaId = new SelectList(db.MediasList.Where(x => !x.IsDeleted), "MediaId", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ReportViewModel report)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            if (!ModelState.IsValid) { return View(report); }
            var model = new Report
            {
                Comments = report.Comments,
                Impact = report.Impact,
                IsPaid = report.IsPaid,
                IsPositive = report.IsPositive,
                MagazineId = magId,
                MediaId = report.MediaId,
                ReportId = report.ReportId,
                Title = report.Title
            };

            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NewsId = new SelectList(db.NewsesList.Where(x => x.Category.MagazineId == magId).Where(x => !x.IsDeleted), "NewsId", "Title");
            ViewBag.MediaId = new SelectList(db.MediasList.Where(x => !x.IsDeleted), "MediaId", "Name");
            return View(report);
        }

        public ActionResult Delete(int? id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.ReportsList.SingleOrDefault(x => x.ReportId == id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            Report report = db.ReportsList.Find(id);
            db.ReportsList.Remove(report);
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
