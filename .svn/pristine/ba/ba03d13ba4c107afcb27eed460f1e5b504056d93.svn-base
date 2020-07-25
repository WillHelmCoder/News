using Dal.AuthControllers;
using Dal.Models;
using Dal.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Dal.Controllers
{
    public class HomeController : BasicController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;

        public HomeController(ResourceService resourceService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            MagazineService = magazineService;
            ResourceService = resourceService;
            UserService = userService;
            InfluencerService = influencerService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _Menu()
        {
            var model = MagazineService.GetCategoriesByMagazineId(7);
            return PartialView(model);
        }

        public PartialViewResult _LastNews()
        {
            var model = MagazineService.GetNewsesByMagazineId(7).Take(3).ToList();
            return PartialView(model);
        }

        public PartialViewResult _Sections()
        {
            var model = MagazineService.GetCategoriesByMagazineId(7);
            return PartialView(model);
        }

        public PartialViewResult _LastNewsFooter()
        {
            var model = MagazineService.LastNewsFooter();
            return PartialView(model);
        }

        public PartialViewResult _Comment(Int32 id)
        {
            var model = new ComentarioViewModel
            {
                NewsId = id
            };

            return PartialView(model);
        }

        public JsonResult AddComment(Int32 id, String comment)
        {
            var noticia = MagazineService.GetNewsById(id);

            if (noticia == null)
            {
                return Json("Lo sentimos, el artículo no existe. Inténtelo con otro distinto.", JsonRequestBehavior.AllowGet);
            }

            if (!ModelState.IsValid)
            {
                return Json("Lo sentimos, los datos no son válidos. Inténtelo de nuevo.", JsonRequestBehavior.AllowGet);
            }

            if (!MagazineService.CreateComment(comment, id))
            {
                return Json("Lo sentimos, no se pudo guardar el comentario. Inténtelo de nuevo.", JsonRequestBehavior.AllowGet);
            }

            return Json("Comentario públicado exitosamente.", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Noticia(Int32 id, String social, Int32? idInfluencer)
        {
            var IP = Request.UserHostAddress;
            SetCookie("VisitCookie", IP, true);
            var date = DateTime.Now;

            var label = "Público";

            if (idInfluencer.HasValue)
                InfluencerService.CreateVisit(id, idInfluencer, IP, date, social, Request.Cookies["VisitCookie"].Value);
            else
                if (!Request.Browser.Crawler && !Request.UserHostAddress.Contains("127.0.0"))
                InfluencerService.CreateVisit(id, null, IP, date, label, Request.Cookies["VisitCookie"].Value);

            var noticia = MagazineService.GetNewsById(id);
            var model = new NoticiasViewModel
            {
                Noticia = noticia,
            };
            var idCategory = noticia.CategoryId;

            if (UserService.GetCurrentUser() != null)
                ViewBag.userEmail = UserService.GetCurrentUser().Email.ToString();

            ViewBag.todaysDate = DateTime.Now.ToLongDateString();
            ViewBag.Cat = MagazineService.GetLastNewsesByCategoryId(idCategory, id);

            return View(model);
        }

        public ActionResult GetComments(Int32 id)
        {
            var items = MagazineService.GetComments(id);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Categories(Int32 id)
        {
            var category = MagazineService.GetCategoryById(id);

            if (category == null)
            {
                SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                return Redirect("/");
            }

            var newsByCategories = MagazineService.GetNewsesByCategoryId(id);

            var model = new CategoriesViewModel
            {
                NewsByCategories = newsByCategories,
                Category = category
            };
            return View(model);
        }

        public ActionResult Search(String tag, String from, String to)
        {
            if (!ModelState.IsValid) { return View("/"); }

            DateTime fromDate;
            DateTime toDate;

            if (from != null && tag == null && to == null)
            {
                if (!DateTime.TryParse((from), out fromDate))
                {
                    ModelState.AddModelError("", "Fecha incorrecta.");
                    SetMessage("¡ Lamentablemente, la fecha introducida es incorrecta ! ", BootstrapAlertTypes.Danger);
                    return Redirect("/");
                }
                var model = MagazineService.GetNewsesByDateNewses(fromDate);
                return View(model);
            }

            if (from != null && to != null && tag != null)
            {
                if (!DateTime.TryParse((from), out fromDate))
                {
                    ModelState.AddModelError("", "Fecha incorrecta.");
                    SetMessage("¡ Lamentablemente, la fecha inicial introducida es incorrecta ! ", BootstrapAlertTypes.Danger);
                    return Redirect("/");
                }

                if (!DateTime.TryParse((to), out toDate))
                {
                    ModelState.AddModelError("", "Fecha incorrecta.");
                    SetMessage("¡ Lamentablemente, la fecha final introducida es incorrecta ! ", BootstrapAlertTypes.Danger);
                    return Redirect("/");
                }
                var items = MagazineService.GetNewsByTagAndDateRange(tag, fromDate, toDate);
                return View(items);
            }

            var news = MagazineService.GetNewsByTag(tag, 7);
            return View(news);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            using (var emailService = new EmailService())
            {
                emailService.SetRecipient("contacto@expose.mx");
                emailService.Contacto(model.Name, model.Email, model.Comments);
                emailService.Send();
            }

            SetMessage("Tu mensaje ha sido recibido!", BootstrapAlertTypes.Success);
            //InsertContact(model.Name, model.Email, model.Comments);
            //TempData["Notice"] = "Tu mensaje ha sido recibido.";
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult Suscribete(String Email)
        {
            MagazineService.Create(Email);
            SetMessage("¡ Gracias por suscribirte ! ", BootstrapAlertTypes.Success);
            return View();
        }

        public ActionResult Find(String keyword)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                return Redirect("/");
            }

            var model = MagazineService.FindNewsByKeyword(keyword, 7);
            SetMessage("¡ Disfrute de la noticia ! ", BootstrapAlertTypes.Success);
            return View(model);

        }

        public ActionResult Buscarporfecha(String fecha)
        {
            return View();
        }

        public ActionResult Testing()
        {
            var news = MagazineService.GetNewsesByMagazineId(7);
            var categories = MagazineService.GetCategoriesByMagazineId(7);
            return View();
        }

        public ActionResult Hash()
        {
            byte[] HashValue;

            string domain = "http://www.wineux.mx".Trim();
            string magazineTitle = "wineux".Trim();

            string MessageString = domain + " " + magazineTitle;

            //Create a new instance of the UnicodeEncoding class to 
            //convert the string into an array of Unicode bytes.
            UnicodeEncoding UE = new UnicodeEncoding();

            //Convert the string into an array of bytes.
            byte[] MessageBytes = UE.GetBytes(MessageString);

            //Create a new instance of the SHA1Managed class to create 
            //the hash value.
            SHA1Managed SHhash = new SHA1Managed();

            //Create the hash value from the array of bytes.
            HashValue = SHhash.ComputeHash(MessageBytes);

            //Display the hash value to the console. 
            return Json(HashValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompareHash()
        {
            //This hash value is produced from "This is the original message!" 
            //using SHA1Managed.  
            byte[] SentHashValue = { 81, 6, 35, 202, 251, 135, 55, 25, 3, 238, 184, 133, 16, 89, 246, 231, 12, 55, 11, 141 };

            //This is the string that corresponds to the previous hash value.
            string domain = "http://www.wineux.mx".Trim();
            string magazineTitle = "winaux".Trim();

            string MessageString = domain + " " + magazineTitle;

            byte[] CompareHashValue;

            //Create a new instance of the UnicodeEncoding class to 
            //convert the string into an array of Unicode bytes.
            UnicodeEncoding UE = new UnicodeEncoding();

            //Convert the string into an array of bytes.
            byte[] MessageBytes = UE.GetBytes(MessageString);

            //Create a new instance of the SHA1Managed class to create 
            //the hash value.
            SHA1Managed SHhash = new SHA1Managed();

            //Create the hash value from the array of bytes.
            CompareHashValue = SHhash.ComputeHash(MessageBytes);

            bool Same = true;

            //Compare the values of the two byte arrays.
            for (int x = 0; x < SentHashValue.Length; x++)
            {
                if (SentHashValue[x] != CompareHashValue[x])
                {
                    Same = false;
                }
            }
            //Display whether or not the hash values are the same.
            if (Same)
                return Json("Match", JsonRequestBehavior.AllowGet);
            else
                return Json("No match", JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateGuids(Int32 id)
        {
            if (MagazineService.ChangeMagGuid(id))
                return Json("Done", JsonRequestBehavior.AllowGet);

            return Json("Not done", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Something()
        {
            MagazineService.SetCategoryToNews();

            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        //public String UpdateImate()
        //{
        //    var getNews = MagazineService.GetAllNews();

        //    foreach (var item in getNews)
        //    {
        //        if (item.Image != null)
        //        {
        //            string[] imageUrl = item.Image.Split('/');
        //            var newUrl = "";
        //            if (imageUrl.Count() <= 1)
        //            {
        //                newUrl = "http://comulink.com/TheContent/Content/data/" + item.Image;
        //            }
        //            else
        //            {
        //                newUrl = "http://comulink.com/TheContent/Content/data/" + imageUrl.Last();
        //            }
        //            var editUrl = MagazineService.EditNews(item.NewsId, item.Title, item.Description, newUrl, item.Body, item.CategoryId, item.Permalink, item.MetaDesc, item.Keywords, item.Alt, item.VideoEmbed, null);

        //        }
        //    }

        //    return "Ready";
        //}
    }
}