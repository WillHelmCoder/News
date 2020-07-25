using Dal.AuthControllers;
using Dal.Models;
using Dal.Services;
using System;
using System.Web.Mvc;
using Twitterizer;

namespace Dal.Controllers
{
    public class WizardController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;

        public WizardController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            InfluencerService = influencerService;
        }

        [HttpGet]
        public ActionResult CreateCategory(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id, user.UserId);
            if (!relation || !ModelState.IsValid) { return RedirectToAction("Index", "Magazines"); }

            var model = new CategoryViewModel
            {
                MagazineId = id,
                ParentsCategories = MagazineService.GetCategoriesByMagazineId(magId)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCategory(CategoryViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            if (!ModelState.IsValid) { return View(model); }

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(model.MagazineId, user.UserId);

            if (!relation) { return RedirectToAction("Index", "Magazines"); }

            var imageCode = "Expose_Default_Category.png";
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

            var create = MagazineService.CreateCategory(model.Name, model.Permalink, imageCode, model.MagazineId, model.Width, model.Height, null, model.showImage);
            if (create == null)
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("Index", "Magazines");
            }
            return RedirectToAction("CreateNew", new { id = create.CategoryId });
        }

        [HttpGet]
        public ActionResult CreateNew(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var model = new NewsViewModel
            {
                CategoryId = id,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateNew(NewsViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            if (!ModelState.IsValid) { return View(model); }

            var category = MagazineService.GetCategoryById(model.CategoryId);
            if (category == null)
            {
                return RedirectToAction("Index", "Magazines");
            }

            var imageCode = "Expose_Default_New.png";
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

            var create = MagazineService.CreateNews(model.Title, model.Description, model.MainImage, model.Thumbnail, model.Body,
                model.CategoryId, model.Permalink, model.MetaDesc, model.MetaTags, model.Alt, model.VideoEmbed, DateTime.Now);

            if (create == null)
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("Index", "Magazines");
            }

            var user = MagazineService.GetCurrentUser();
            if (user == null)
            {
                TweetNews("http://www.expose.mx/noticia/" + create.NewsId + "/tw/" + 2 + " " + create.Description);
            }
            else
            {
                TweetNews("Expose.mx http://www.expose.mx/noticia/" + create.NewsId + "/tw/" + user.UserId + " " + create.Description);
            }

            return RedirectToAction("MagazineNews", "News", new { id = create.Category.MagazineId });
        }

        [HttpGet]
        public ActionResult CloneNew()
        {
            return View();
        }

        public ActionResult FirstNew()
        {
            return View();
        }

        public void TweetNews(String message)
        {
            try
            {
                OAuthTokens tokens = new OAuthTokens();
                tokens.ConsumerKey = "quTJ9sYIOXTGfSeHfKGd1TI89";
                tokens.ConsumerSecret = "JJA3Kn984RDWcEE24sTihLtivKSxNTpEk0xSMwERy74x8DQ04B";
                tokens.AccessToken = "3161508222-rS1xFdKGbQg5H8se4wpmr8YgLUMowhBjeeMKpv7";
                tokens.AccessTokenSecret = "S1Q8udzNpOeKvvZlXNUpN52sPeRRXATdOS065GONjG9O8";
                var status = TwitterStatus.Update(tokens, message, new StatusUpdateOptions { UseSSL = true, APIBaseAddress = "http://api.twitter.com/1.1/" });
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}