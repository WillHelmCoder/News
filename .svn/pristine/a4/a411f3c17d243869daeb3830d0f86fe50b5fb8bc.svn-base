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

namespace Dal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserMagazinesController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();

        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;

        public UserMagazinesController(ResourceService resourceService, UserService userService, MagazineService magazineService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
        }

        // GET: UserMagazines
        public ActionResult Index()
        {
            var userMagazinesList = MagazineService.GetUserMagazinesByUserAdmin();
            return View(userMagazinesList.ToList());
        }

        // GET: UserMagazines/Details/5
        public ActionResult Details(Int32 id)
        {

            UserMagazine userMagazine = db.UserMagazinesList.Find(id);
            if (userMagazine == null)
            {
                SetMessage("No se encontró ningun elemento.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index");
            }
            return View(userMagazine);
        }

        // GET: UserMagazines/Create
        public ActionResult Create()
        {
            ViewBag.MagazineId = new SelectList(db.MagazinesList.Include(x => x.User).Where(x => x.User.Email.Equals(UserEmail, StringComparison.OrdinalIgnoreCase)), "MagazineId", "Title");
            ViewBag.UserId = new SelectList(db.UsersList.Where(x => !x.Email.Equals(UserEmail, StringComparison.OrdinalIgnoreCase)), "UserId", "Email");
            return View();
        }

        // POST: UserMagazines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "UserMagazineId,UserId,MagazineId")] UserMagazine userMagazine)
        {
            if (ModelState.IsValid)
            {
                db.UserMagazinesList.Add(userMagazine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MagazineId = new SelectList(db.MagazinesList.Include(x => x.User).Where(x => x.User.Email.Equals(UserEmail, StringComparison.OrdinalIgnoreCase)), "MagazineId", "Title", userMagazine.MagazineId);
            ViewBag.UserId = new SelectList(db.UsersList.Where(x => !x.Email.Equals(UserEmail, StringComparison.OrdinalIgnoreCase)), "UserId", "Email", userMagazine.UserId);
            return View(userMagazine);
        }

        // GET: UserMagazines/Edit/5
        public ActionResult Edit(Int32 id)
        {

            UserMagazine userMagazine = db.UserMagazinesList.Find(id);
            if (userMagazine == null)
            {
                SetMessage("No se encontraron resultados.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index");
            }
            ViewBag.MagazineId = new SelectList(db.MagazinesList.Include(x => x.User).Where(x => x.User.Email.Equals(UserEmail)), "MagazineId", "Title", userMagazine.MagazineId);
            ViewBag.UserId = new SelectList(db.UsersList.Where(x => !x.Email.Equals(UserEmail, StringComparison.OrdinalIgnoreCase)), "UserId", "Email", userMagazine.UserId);
            return View(userMagazine);
        }

        // POST: UserMagazines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "UserMagazineId,UserId,MagazineId")] UserMagazine userMagazine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userMagazine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MagazineId = new SelectList(db.MagazinesList.Include(x => x.User).Where(x => x.User.Email.Equals(UserEmail)), "MagazineId", "Title", userMagazine.MagazineId);
            ViewBag.UserId = new SelectList(db.UsersList.Where(x => !x.Email.Equals(UserEmail, StringComparison.OrdinalIgnoreCase)), "UserId", "Email", userMagazine.UserId);
            return View(userMagazine);
        }

        // GET: UserMagazines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMagazine userMagazine = db.UserMagazinesList.Find(id);
            if (userMagazine == null)
            {
                return HttpNotFound();
            }
            return View(userMagazine);
        }

        // POST: UserMagazines/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMagazine userMagazine = db.UserMagazinesList.Find(id);
            db.UserMagazinesList.Remove(userMagazine);
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
