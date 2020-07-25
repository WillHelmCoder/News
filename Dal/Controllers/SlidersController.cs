using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Dal.Controllers
{
    public class SlidersController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();
        private readonly MagazineService MagazineService;
        private readonly UserService UserService;


        public SlidersController(UserService userService, MagazineService magazineService)
        {
            UserService = userService;
            MagazineService = magazineService;
        }

        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var SlidesList = db.SlidesList.Include(s => s.News).Include(x => x.News).Include(x => x.News.Category).Include(x => x.Slider).Where(x => x.News.Category.MagazineId == magId).Where(x => !x.News.IsDeleted).ToList();
            List<Slider> SliderList = new List<Slider>();
            foreach(var item in SlidesList)
            {
                SliderList.Add(item.Slider);
            }


            return View(SliderList.Distinct().ToList());
        }

        public ActionResult Preview(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var slideList = db.SlidesList.Include(x => x.Slider).Include(x => x.News).Where(x => x.SliderId == id).ToList();

            return View(slideList);
        }

        public ActionResult EditOrder(int id)
        {
            var getSlides = db.SlidesList.Include(s => s.News).Where(x => x.SliderId == id).ToList();
            List<int> orderList = new List<int>();
            foreach(var item in getSlides)
            {
                orderList.Add(item.Order); 
            }

            ViewBag.OrderCount = orderList.Count();

            return View(getSlides);
        }
        [HttpPost]
        public ActionResult EditOrder(List<int>slideId, List<int>order)
        {
           for(var i = 0; i < slideId.Count; i++)
            {
                var id = slideId[i];
                var getSlide = db.SlidesList.SingleOrDefault(x => x.SlideId == id);
                getSlide.Order = order[i];

                db.Entry(getSlide).State = EntityState.Modified;
                db.SaveChanges();
            }

            SetMessage("Se cambio el orden de tus slides exitosamente.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }
        public ActionResult CreateSlider()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            ViewBag.News= db.NewsesList.Include(x => x.Category).Where(x => !x.IsDeleted).Where(x => x.Category.MagazineId == magId).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult CreateSlider(CreateSliderModel model)
        {
            Guid guid = Guid.NewGuid();

            var slider = new Slider
            {
                Name = model.Slider.Name,
                sGuid = guid.ToString()
            };

            db.SlidersList.Add(slider);
            db.SaveChanges();

            var slide = new Slide { };
            var i = 0;
            foreach(var item in model.NewId)
            {
                slide = new Slide
                {
                    NewsId = item,
                    Order = i,
                    SliderId = slider.SliderId
                };
                db.SlidesList.Add(slide);
                db.SaveChanges();
                i++;
            }
            SetMessage("Tu slider fue creado con éxito.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Create(int id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
            ViewBag.SliderId = id;
            ViewBag.News = db.NewsesList.Include(x => x.Category).Where(x => !x.IsDeleted).Where(x => x.Category.MagazineId == magId).ToList();
            var news = ViewBag.News = db.NewsesList.Include(x => x.Category).Where(x => !x.IsDeleted).Where(x => x.Category.MagazineId == magId).ToList();
            var slides = db.SlidesList.Include(x => x.News).Where(x => x.SliderId == id).Where(x => !x.IsDeleted).ToList();
            List<Slide> Slides= slides;
            ViewBag.LastItems = db.SlidesList.Where(x => x.SliderId == id).Where(x => !x.IsDeleted).OrderByDescending(x => x.Order).Take(1).Select(x => x.Order);
            return View(Slides);
        }

        [HttpPost]
        public ActionResult Create(int SliderId, List<int> NewId)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation) return RedirectToAction("Index", "Magazines");
            var order= db.SlidesList.Where(x => x.SliderId == SliderId).OrderByDescending(x => x.Order).Take(1).Select(x => x.Order).First();
            
            var slide = new Slide();
            foreach(var item in NewId)
            {
                slide = new Slide
                {
                    NewsId = item,
                    SliderId = SliderId,
                    Order = order
                };

                db.SlidesList.Add(slide);
                db.Save();

                order++;
            }
            SetMessage("Tus slides fueron agregados con éxito.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            Slide Slides = db.SlidesList.Include(x => x.News).Where(x => x.SlideId == id).SingleOrDefault();
            if (Slides == null)
            {
                return HttpNotFound();
            }

            var noticia = MagazineService.GetNewsById(Slides.NewsId);

            if (noticia == null)
            {
                SetMessage("No se encontró la noticia.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            ViewBag.MagId = noticia.Category.MagazineId;
            ViewBag.NewsId = new SelectList(db.NewsesList.Include(x => x.Category).Where(x => !x.IsDeleted).Where(x => x.Category.MagazineId == noticia.Category.MagazineId).ToList(), "NewsId", "Title", Slides.NewsId);
            return View(Slides);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "SlidesId,NewsId,Order")] Slide Slides)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            if (ModelState.IsValid)
            {
                db.Entry(Slides).State = EntityState.Modified;
                db.SaveChanges();

                var newId = MagazineService.GetNewsById(Slides.NewsId);

                return RedirectToAction("Index", new { id = magId });
            }
            var noticia = MagazineService.GetNewsById(Slides.NewsId);

            if (noticia == null)
            {
                SetMessage("No se encontró la noticia.", BootstrapAlertTypes.Danger);
                return RedirectToAction("MagazineNews", "News");
            }

            ViewBag.NewsId = new SelectList(db.NewsesList.Include(x => x.Category).Where(x => x.Category.MagazineId == noticia.Category.MagazineId).ToList(), "NewsId", "Title", Slides.NewsId);
            return View(Slides);
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
            Slide Slides = db.SlidesList.Find(id);
            if (Slides == null)
            {
                return HttpNotFound();
            }

            var newId = MagazineService.GetNewsById(Slides.NewsId);
            ViewBag.MagId = newId.Category.MagazineId;

            return View(Slides);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            Slide Slides = db.SlidesList.Find(id);

            var news = MagazineService.GetNewsById(Slides.NewsId);

            db.SlidesList.Remove(Slides);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = magId });
        }

        [HttpPost]
        public JsonResult DeleteSlide(int id)
        {
            var item = db.SlidesList.SingleOrDefault(x => x.SlideId == id);
            item.IsDeleted = true;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();

            return Json("funciona");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //UNNECESARY
        //public ActionResult Details(Int32 id)
        //{
        //    Slides Slides = db.SlidessList.Find(id);
        //    if (Slides == null)
        //    {
        //        SetMessage("No se encontró el Slides.", BootstrapAlertTypes.Danger);
        //        return RedirectToAction("Index");
        //    }
        //    return View(Slides);
        //}
    }
}
