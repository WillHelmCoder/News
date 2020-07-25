using Dal.AuthControllers;
using Dal.Classes;
using Dal.Interfaces;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using Dal.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Dal.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class MagazinesController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();

        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;
        private readonly IRoleService RoleService;


        public MagazinesController(ResourceService resourceService, IRoleService roleService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            RoleService = roleService;
            InfluencerService = influencerService;
        }

        public ActionResult Index()
        {
            var rol = UserService.GetRolUser();
            var magazines = MagazineService.GetCurrentUserMagazines();

            if (RoleService.IsUserInRole(UserEmail, "SuperAdmin"))
            {
                List<int> termsList = new List<int>();
                foreach (var item in magazines)
                {
                    var Count = InfluencerService.GetVisitsByMagazine(item.MagazineId);
                    termsList.Add(Count);
                }
            }

            var model = new MagazinesIndexViewModel
            {
                MagazinesList = magazines,
            };

            ViewBag.CurrenUser = Session["currentUser"];
            return View(model);
        }

        public ActionResult Details(Int32 id)
        {

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            Magazine magazine = db.MagazinesList.Find(id);
            if (magazine == null)
            {
                SetMessage("No se encontró la revista.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index");
            }
            return View(magazine);
        }

        public ActionResult Create()
        {
            var states = new SelectList(ResourceService.GetStates(), "StateId", "Name");
            var cities =new SelectList(ResourceService.GetCities(1), "CityId", "Name");
            ViewBag.States = states;
            ViewBag.Cities = cities;

            Int32 magazines = MagazineService.CountUserMagazines();
            var user = UserService.GetCurrentUser();

            if (user == null)
            {
                SetMessage("Inicie sesión por favor.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrenUser = Session["currentUser"];
            //if (magazines >= 10 && user.Stage != PaymentStages.Pagado)
            //{
            //    return Redirect("/Magazines/PaymentDetails");

            //}
            return View();
        }

        [HttpPost]
        public ActionResult Create(MagazineViewModel model)
        {
            ViewBag.States = new SelectList(ResourceService.GetStates(), "StateId", "Name", model.StateId);
            ViewBag.Cities = new SelectList(ResourceService.GetCities(model.StateId), "CityId", "Name", model.CityId);

            if (!ModelState.IsValid) { return View(model); }

            var imageCode = "Expose_Default_Magazine.png";

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

            var create = MagazineService.Create(model.Title, model.Description, imageCode, model.Address, model.CityId,
                model.Email, model.IsPrivate, model.Domain, model.FacebookAccount, model.TwitterAccount, model.GoogleAnlyticsId);

            if (create == null)
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("Index");
            }

            SetMessage("Revista creada.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Int32 id)
        {
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var magazine = MagazineService.GetMagazineById(id);

            if (magazine == null)
            {
                SetMessage("No se encontró la revista.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index");
            }

            var model = new MagazineViewModel
            {
                MagazineId = magazine.MagazineId,
                Address = magazine.Address,
                CityId = magazine.CityId,
                Description = magazine.Description,
                Email = magazine.Email,
                LogoImage = magazine.Logo,
                Title = magazine.Title,
                Domain = magazine.Domain,
                FacebookAccount = magazine.FacebookPage,
                GoogleAnlyticsId = magazine.GoogleAnalyticsId,
                TwitterAccount = magazine.TwitterPage,
                Guid = magazine.Guid
            };

            var stateId = ResourceService.GetStateByCityId(magazine.CityId);

            ViewBag.States = new SelectList(ResourceService.GetStates(), "StateId", "Name", stateId);
            ViewBag.Cities = new SelectList(ResourceService.GetCities(stateId), "CityId", "Name", magazine.CityId);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MagazineViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var imageCode = model.LogoImage;

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


            if (!MagazineService.EditMagazine(model.MagazineId, model.Title, model.Description, imageCode, model.Address,
                model.CityId, model.Email, model.IsPrivate, model.Domain, model.FacebookAccount, model.TwitterAccount, model.GoogleAnlyticsId))
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("Index");
            }

            SetMessage("Revista editada exitosamente.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id.Value, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var mag = MagazineService.GetMagazineById(id.Value);

            if (mag == null)
            {
                return HttpNotFound();
            }
            return View(mag);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(id, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            Magazine magazine = db.MagazinesList.Find(id);
            db.MagazinesList.Remove(magazine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TopHundred()
        {
            var top100 = MagazineService.GetTop100Influencer();
            return View(top100);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Paypal()
        {
            const int tot = 150;

            var orderId = Guid.NewGuid().ToString();

            if (!UserService.UpdateUserPaymentToken(orderId))
            {
                SetMessage(UserService.ServiceTempData);
                return RedirectToAction("PaymentDetails");
            }

            Uri uri = Request.Url;
            string host = uri.Scheme + Uri.SchemeDelimiter + uri.Host;

            PayPalModel paypal = new PayPalModel();
            paypal.cmd = "_xclick";
            paypal.business = Configuration.BusinessAccountKey;
            paypal.currency_code = Configuration.CurrencyCode;

            if (!Configuration.UseSandBox)
            {
                ViewBag.actionUrl = "https://www.paypal.com/cgi-bin/websrc";
            }
            else
            {
                ViewBag.actionUrl = "https://sandbox.paypal.com/cgi-bin/webscr";
            }



            paypal.cancel_return = host + Configuration.CancelUrl + "?orderId=" + orderId; //host + ConfigurationManager.AppSettings["CancelUrl"];
            paypal.@return = host + Configuration.ReturnUrl + "?orderId=" + orderId;
            paypal.notify_url = host + Configuration.NotifyUrl;
            //paypal.currency_code = payment.Currency;
            paypal.item_name = "Orde de compra de Noticias";
            paypal.item_number = orderId.ToString();
            paypal.image_url = "http://www.lacubiella.com/Content/images/lacubiella.png";
            paypal.amount = tot.ToString();

            return View(paypal);
        }

        public ActionResult PaymentDetails()
        {
            return View();
        }


        public ActionResult speedTest()
        {

            var model = MagazineService.speedtest();          
            
            return View(model);
           
        }

    }
}
