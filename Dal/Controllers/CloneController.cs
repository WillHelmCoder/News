using Dal.AuthControllers;
using Dal.Interfaces;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace News.Controllers
{
    public class CloneController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();

        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;
        private readonly IRoleService RoleService;

        public CloneController(ResourceService resourceService, IRoleService roleService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            RoleService = roleService;
            InfluencerService = influencerService;
        }
        // GET: Clone
        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var getLocation = MagazineService.GetCurrentMagazine(magId);
            var GetNearlyMags = MagazineService.GetNearlyMagazines(getLocation.CityId);

            List<NearlyNews> ListOfNews = new List<NearlyNews>();
            foreach (var item in GetNearlyMags)
            {
                var getNews = MagazineService.GetPublicNewsByMagazineId(item.MagazineId, 1);
                if (getNews.Any())
                {
                    ListOfNews.Add(new NearlyNews
                    {
                        MagazineId = getNews[0].Category.MagazineId,
                        MagazineName = getNews[0].Category.Magazine.Title,
                        NewsList = getNews
                    });
                }
            }
           
            var magazines = MagazineService.GetCurrentUserMagazines();

            ViewBag.MagazineId = magId;
            ViewBag.Magazines = JsonConvert.SerializeObject(magazines, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return View(ListOfNews);
        }
    }
}