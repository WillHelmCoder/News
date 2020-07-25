using Dal.AuthControllers;
using Dal.Models;
using Dal.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Twitterizer;

namespace Dal.Controllers
{
    public class NewsController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;

        public NewsController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            InfluencerService = influencerService;
        }

        public ActionResult MagazineNews(Int32 id)
        {
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            Request.Cookies.Remove("MagazineId");
            SetCookie("MagazineId", id, true);

            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }

            var magazine = MagazineService.GetMagazineById(id);
            if (magazine == null)
            {
                SetMessage("Lo sentimos, la revista no existe. Inténtelo de nuevo con una distinta.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            var mostUsedCat = MagazineService.MostUsedCat(id);
            var mostViewedCat = MagazineService.MostViewedCat(id);

            var mostUsedCatName = "Ninguna";
            if (mostUsedCat != null) mostUsedCatName = mostUsedCat.Name;

            var mostViewedCatName = "Ninguna";
            if (mostViewedCat != null) mostViewedCatName = mostViewedCat.Name;

            var mostViewedCatCount = 0;
            if (mostViewedCat != null) mostViewedCatCount = MagazineService.GetVisitsByCategoryId(mostViewedCat.CategoryId);

            var mostUsedCatCount = 0;
            if (mostUsedCat != null) mostUsedCatCount = MagazineService.MostUsedCatCount(mostUsedCat.CategoryId);

            var newsAndVisits = MagazineService.GetNewsWithVisitsByMagazineId(id);

            var viewsCounts = 0;
            if (newsAndVisits != null) viewsCounts = newsAndVisits.TotalVisits;

            var newsCount = 0;
            if (newsAndVisits != null) newsCount = newsAndVisits.TotalNews;

            var model = new MagazineIndexViewModel
            {
                MagazineId = id,
                MostUsedCat = mostUsedCatName,
                MostViewedCat = mostViewedCatName,
                MostViewedCatCount = mostViewedCatCount,
                MostUsedCatCount = mostUsedCatCount,
                ViewsCount = viewsCounts,
                NewsCount = newsCount,
            };
            //TOP 10 STATISTICS

            var GetNews = MagazineService.GetNewsByMagazineId(id);

            var top10Count = new List<Last10CountModel>();
            foreach (var item in GetNews)
            {
                var addNew = new Last10CountModel
                {
                    NewsId = item.NewsId,
                    NewTitle = item.Title,
                    Count = (item.VisitsCount ?? 0)
                };

                top10Count.Add(addNew);
            }

            var top10Visited = top10Count.OrderByDescending(x => x.Count).Take(10);
            var last10Published = top10Count.Take(10);

            ViewBag.Top10Visited = JsonConvert.SerializeObject(top10Visited);
            ViewBag.Last10Published = JsonConvert.SerializeObject(last10Published);

            //REPORTES
            var reportsList = MagazineService.GetMyReports(id);

            var reportList = new List<ReportViewModel>();
            foreach (var item in reportsList)
            {
                var report = new ReportViewModel
                {
                    ReportId = item.MediaId,
                    Impact = MagazineService.getProm(item.MediaId),
                    MediaName = MagazineService.getMedioName(item.MediaId)
                };

                reportList.Add(report);
            }

            ViewBag.Reports = null;
            if (reportsList.Count > 0)
            {
                ViewBag.Reports = reportList;
            }

            ViewBag.JsonReports = JsonConvert.SerializeObject(reportList);

            return View(model);
        }

        public ActionResult MyNews()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }

            var id = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var news = MagazineService.GetNewsByMagazineId(id);

            var cats = MagazineService.GetCategoriesByMagazineId(id);
            if (cats.Count == 0) return RedirectToAction("CreateCategory", "Wizard", new { id = id });
            var mags = MagazineService.GetCurrentUserMagazines();
            List<MagazineToCLone> magList = new List<MagazineToCLone>();
            foreach (var item in mags)
            {
                var magModel = new MagazineToCLone()
                {
                    MagazineId = item.MagazineId,
                    Title = item.Title
                };
                magList.Add(magModel);
            }
            //var top10Influencers = MagazineService.GetTop10Influencers(id);

            var model = new MagazineIndexViewModel
            {
                MagazineId = id,
                Categories = cats,
                //Top10Influencers = top10Influencers,
                News = news
            };

            //var magazines = MagazineService.GetCurrentUserMagazines();
            //var allCats = new List<Category>();
            //foreach (var item in magazines)
            //{
            //    var items = MagazineService.GetCategoriesByMagazineId(item.MagazineId);
            //    foreach (var a in items) { allCats.Add(a); }
            //}

            //ViewBag.Visits = JsonConvert.SerializeObject(newsAndVisits.VisitCounts);
            //ViewBag.Categories = JsonConvert.SerializeObject(allCats, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            ViewBag.Magazines = JsonConvert.SerializeObject(magList, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return View(model);
        }

        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");
            ViewBag.MagId = magId;
            var mags = MagazineService.GetCategoriesByMagazineId(magId);
            if (mags.Count < 1)
            {
                SetMessage("Al parecer no tienes ninguna categoria registrada, te invitamos a crear una antes de redactar una noticia.", BootstrapAlertTypes.Danger);
                return RedirectToAction("CreateCategory", "Wizard", new { id = Request.Cookies["MagazineId"].Value });
            }

            ViewBag.CategoryId = new SelectList(mags, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewsViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);
            ViewBag.MagId = magId;
            if (!relation) return RedirectToAction("Index", "Magazines");

            ViewBag.CategoryId = new SelectList(MagazineService.GetCategoriesByMagazineId(magId), "CategoryId", "Name");

            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de utilizar una imagen de menor peso debe de tener un máximo 1.6 Mb. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            var category = MagazineService.GetCategoryById(model.CategoryId);
            if (category == null)
            {
                SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("MyNews");
            }

            var parsedDate = (!String.IsNullOrEmpty(model.CreationDate) ? DateTime.ParseExact(model.CreationDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture) : DateTime.Now);

            var create = MagazineService.CreateNews(model.Title, model.Description, model.MainImage, model.Thumbnail, model.Body,
                model.CategoryId, model.Permalink, model.MetaDesc, model.MetaTags, model.Alt, model.VideoEmbed, parsedDate);

            if (create == null)
            {
                SetMessage(MagazineService.ServiceTempData);
                SetMessage("Ocurrió un error inesperado. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return RedirectToAction("MyNews");
            }

            if (user == null)
            {
                TweetNews("http://www.expose.mx/noticia/" + create.NewsId + "/tw/" + 2 + " " + create.Description);
            }
            else
            {
                TweetNews("Expose.mx http://www.expose.mx/noticia/" + create.NewsId + "/tw/" + user.UserId + " " + create.Description);
            }

            SetMessage("Noticia creada exitosamente", BootstrapAlertTypes.Success);
            return RedirectToAction("MyNews");
        }

        public ActionResult Edit(Int32 id, Int32? newMagId)
        {
            if (newMagId.HasValue)
            {
                Request.Cookies.Remove("MagazineId");
                SetCookie("MagazineId", newMagId, true);
            }

            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
            ViewBag.MagId = magId;
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation) return RedirectToAction("Index", "Magazines");

            var news = MagazineService.GetNewsById(id);
            if (news == null)
            {
                SetMessage("Lo sentimos, no se encontró la noticia", BootstrapAlertTypes.Danger);
                return RedirectToAction("MyNews");
            }

            ViewBag.CategoryId = new SelectList(MagazineService.GetCategoriesByMagazineId(magId), "CategoryId", "Name", news.CategoryId);

            var model = new NewsViewModel
            {
                Body = news.Body,
                CategoryId = news.CategoryId,
                Description = news.Description,
                LogoImage = news.Image,
                NewsId = news.NewsId,
                Title = news.Title,
                Alt = news.Alt,
                MetaDesc = news.MetaDesc,
                MetaTags = news.Keywords,
                Permalink = news.Permalink,
                IsClon = news.IsClon,
                ThankNote = news.ThankNote,
                VideoEmbed = news.VideoEmbed,
                CreationDate = news.CreationDate.ToString()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(NewsViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);
            ViewBag.MagId = magId;
            if (!relation) return RedirectToAction("Index", "Magazines");

            ViewBag.CategoryId = new SelectList(MagazineService.GetCategoriesByMagazineId(magId), "CategoryId", "Name");

            if (!ModelState.IsValid)
                return View(model);

            var category = MagazineService.GetCategoryById(model.CategoryId);

            if (category == null)
            {
                SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("MyNews");
            }
            //}

            DateTime? parsedDate = DateTime.Now;

            try
            {
                parsedDate = DateTime.ParseExact(model.CreationDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                parsedDate = null;
            }

            if (!MagazineService.EditNews(model.NewsId, model.Title, model.Description, model.MainImage, model.Thumbnail, model.Body, model.CategoryId, model.Permalink, model.MetaDesc, model.MetaTags, model.Alt, model.VideoEmbed, parsedDate))
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("MyNews");
            }

            SetMessage("Noticia editada exitosamente", BootstrapAlertTypes.Success);
            return RedirectToAction("MyNews");
        }

        public ActionResult Delete(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");


            var news = MagazineService.GetNewsById(id);
            if (news == null)
            {
                SetMessage("No se encontró la noticia.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var news = MagazineService.GetNewsById(id);
            if (news == null)
            {
                SetMessage("No se encontró la noticia.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            if (!MagazineService.DeleteNews(id))
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("Index", new { id = news.CategoryId });
            }

            SetMessage("Noticia eliminada.", BootstrapAlertTypes.Success);
            return RedirectToAction("MagazineNews", "News", new { id = magId });
        }

        [AllowAnonymous]
        public ActionResult CreateSlug(string phrase)
        {
            int maxLength = 100;

            string str = phrase.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n");
            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // cut and trim it
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            string permalink = str;
            return Json(permalink, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CounterByInfluencer(Int32 id)
        {
            var noticia = MagazineService.GetNewsById(id);

            if (noticia == null)
            {
                return Redirect("/magazines");
            }

            ViewBag.NewsTitle = noticia.Title;
            var model = InfluencerService.GetCounterByInfluencer(id);
            return View(model);
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

        public JsonResult CloneNew(Int32 catId, Int32 newId)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultModel()
                {
                    OK = false,
                    Message = "Verifique que los datos sean correctos. Inténtelo de nuevo.",
                }, JsonRequestBehavior.AllowGet);
            }

            var currentNew = MagazineService.GetNewsById(newId);
            if (currentNew == null)
            {
                return Json(new JsonResultModel()
                {
                    OK = false,
                    Message = "Noticia no encontrada. Inténtelo de nuevo.",
                }, JsonRequestBehavior.AllowGet);
            }

            var category = MagazineService.GetCategoryById(catId);
            if (category == null)
            {
                return Json(new JsonResultModel()
                {
                    OK = false,
                    Message = "Categoría no encontrada. Inténtelo de nuevo.",
                }, JsonRequestBehavior.AllowGet);
            }

            var clonedNew = MagazineService.CloneNew(currentNew.NewsId, category.CategoryId);
            if (clonedNew == null)
            {
                return Json(new JsonResultModel()
                {
                    OK = false,
                    Message = "Ha ocurrido un error inesperado. Inténtelo de nuevo.",
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new JsonResultModel()
            {
                OK = true,
                Message = "¡Noticia clonada exitosamente!",
                Model = new
                {
                    Url = "<a href='" + Url.Action("Edit", new { id = clonedNew.NewsId, newMagId = category.MagazineId }) + "'>editar nota</a>",
                }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VoteUp(Int32 Id, Int32 type)
        {
            var user = UserService.GetCurrentUser();

            if (MagazineService.VoteUp(Id, user.UserId, type))
            {
                return Json("Gracias por tu opinión.", JsonRequestBehavior.AllowGet);
            }

            return Json("No ha sido posible generar el voto.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult VoteDown(Int32 Id, Int32 type)
        {
            var user = UserService.GetCurrentUser();
            if (MagazineService.VoteDown(Id, user.UserId, type))
            {
                return Json("Gracias por tu opinión.", JsonRequestBehavior.AllowGet);
            }

            return Json("No ha sido posible generar el voto.", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewsVotes(Int32 Id)
        {
            var votes = MagazineService.GetSplittedNewsVotes(Id);
            var user = UserService.GetCurrentUser();

            if (votes == null)
            {
                return Json("Lo sentimos, no pudimos conseguir los votos de esta publicación.", JsonRequestBehavior.AllowGet);
            }

            return Json(votes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCommentVotes(Int32 Id)
        {
            var votes = MagazineService.GetSplittedCommentVotes(Id);

            if (votes == null)
            {
                return Json("Lo sentimos, no pudimos conseguir los votos de esta publicación.", JsonRequestBehavior.AllowGet);
            }

            return Json(votes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVisitsByMonth()
        {
            if (Request.Cookies["MagazineId"].Value == null)
            {
                return Json("Lo sentimos, la revista no existe. Inténtelo de nuevo con una distinta.", JsonRequestBehavior.AllowGet);
            }

            var id = Int32.Parse(Request.Cookies["MagazineId"].Value);
            var visits = MagazineService.VisitsOfDayByMonth(id);

            return Json(visits, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewsStatistics()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var model = InfluencerService.GetMagazineNewsStatistics(magId);

            return View(model);
        }

        public ActionResult ViewNewStatistics(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var model = InfluencerService.GetNewsStatistics(id);

            return View(model);
        }

        public ActionResult CalculateFullVisits()
        {
            if (MagazineService.CalculateTotalVisits())
                return Json("Hecho", JsonRequestBehavior.AllowGet);

            return Json("Algo salio mal", JsonRequestBehavior.AllowGet);
        }

        //UNNECESARY
        //public ActionResult Index(Int32 id)
        //{
        //    var category = MagazineService.GetCategoryById(id);

        //    if (category == null)
        //    {
        //        SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
        //        return RedirectToAction("Index", "Magazines");
        //    }

        //    ViewBag.MagazineId = category.MagazineId;
        //    var newsesList = MagazineService.GetNewsesByCategoryId(id);
        //    return View(newsesList);
        //}
        //public ActionResult Details(Int32 id)
        //{
        //    var news = MagazineService.GetNewsById(id);

        //    if (news == null)
        //    {
        //        SetMessage("No se encontró la noticia.", BootstrapAlertTypes.Danger);
        //        return RedirectToAction("Index", "Magazines");
        //    }

        //    return View(news);
        //}
    }
}
