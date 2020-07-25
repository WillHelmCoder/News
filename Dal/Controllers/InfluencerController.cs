using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Dal.AuthControllers;
using Dal.Interfaces;
using Dal.Services;
using Dal.Models;

namespace Dal.Controllers
{
    [Authorize]
    public class InfluencerController : BasicController
    {
        private readonly IMembershipService MembershipService;
        private readonly IRoleService RoleService;
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;
        public InfluencerController(IMembershipService membershipService, IRoleService roleService, ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            MagazineService = magazineService;
            MembershipService = membershipService;
            RoleService = roleService;
            ResourceService = resourceService;
            UserService = userService;
            InfluencerService = influencerService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListOfNews()
        {
            ViewBag.UserId = UserService.GetCurrentUser();
            var items = MagazineService.GetAllNewses();
            return View(items);
        }

        public JsonResult Stadistics(Int32? id)
        {
            //if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value); 

            if (id.HasValue)
            {
                var getfb = InfluencerService.GetFbCount(id.Value, magId);
                var countFb = 0;
                if (getfb.Count > 0) foreach (var item in getfb) { countFb = countFb + item.Count; }

                var gettw = InfluencerService.GetTwCount(id.Value, magId);
                var countTw = 0;
                if (gettw.Count > 0) foreach (var item in gettw) { countTw = countTw + item.Count; }

                var getgp = InfluencerService.GetGpCount(id.Value, magId);
                var countGp = 0;
                if (getgp.Count > 0) foreach (var item in getgp) { countGp = countGp + item.Count; }

                var getLi = InfluencerService.GetLiCount(id.Value, magId);
                var countLi = 0;
                if (getLi.Count > 0) foreach (var item in getLi) { countLi = countLi + item.Count; }

                List<Int32> items = new List<Int32>();

                items.Add(countFb);
                items.Add(countTw);
                items.Add(countGp);
                items.Add(countLi);

                return Json(items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.UserId = UserService.GetCurrentUser();
                var userId = ViewBag.UserId.UserId;

                var getfb = InfluencerService.GetFbCount(userId, magId);
                var countFb = getfb.Count;

                var gettw = InfluencerService.GetTwCount(userId, magId);
                var counttw = gettw.Count;


                var getgp = InfluencerService.GetGpCount(userId, magId);
                var countgp = getgp.Count;

                var getLi = InfluencerService.GetLiCount(userId, magId);
                var countLi = getLi.Count;

                List<Int32> items = new List<Int32>();

                items.Add(countFb);
                items.Add(counttw);
                items.Add(countgp);
                items.Add(countLi);

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMyInfluencers()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
 
            var items = MagazineService.GetMyInfluencers(magId);

            return View(items);
        }

        public ActionResult GetMyEditors()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value); 

            var items = MagazineService.GetMyEditors(magId);

            List<EditorListViewModel> editors = new List<EditorListViewModel>();
            foreach (var item in items)
            {
                var addEditor = new EditorListViewModel
                {
                    UserId = item.UserId,
                    UserName = item.User.UserName,
                    Email= item.User.Email,
                    GeneratedViews = MagazineService.GetCountsByUser(item.UserId)
                };

                editors.Add(addEditor);
            }

            return View(editors);
        }

        public ActionResult EditorDetails(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            var magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var item = MagazineService.GetEditorWithNewsAndGeneratedVisitsById(id, magId);

            return View(item);
        }

        public ActionResult TopHundred()
        {
            var top100 = MagazineService.GetTop100Influencer();
            return View(top100);
        }

        //public ActionResult Influencer()
        //{
        //    var Influencers = InfluencerService.GetInfluencers();
        //    ViewBag.Influencers = Influencers;
        //    return View();
        //}

        public ActionResult getNews(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            var magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var model = InfluencerService.GetVisitsByMagazineIdAndInfluencerId(id, magId);
            return View(model);
        }

        public ActionResult Counter(Int32 id)
        {
            var model = InfluencerService.GetCounter(id);

            return View(model);
        }
    }
}