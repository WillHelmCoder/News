using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dal.AuthControllers;
using Dal.Classes;
using Dal.Models;
using Dal.Interfaces;
using Dal.Repositories;
using Dal.Services;
using DotNetOpenAuth.AspNet;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Web.WebPages.OAuth;
using PruebaLogin.Models;
using WebMatrix.WebData;
using ExternalLogin = Dal.Models.ExternalLogin;
using FacebookClient = Facebook.FacebookClient;
using RegisterExternalLoginModel = Dal.Models.RegisterExternalLoginModel;

namespace Dal.Controllers
{
    [Authorize]
    public class AccountController : BasicController
    {
        private readonly IMembershipService MembershipService;
        private readonly IRoleService RoleService;
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;

        public AccountController(IMembershipService membershipService, IRoleService roleService, ResourceService resourceService, UserService userService, MagazineService magazineService)
        {
            MembershipService = membershipService;
            RoleService = roleService;
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                var email = UserService.GetCurrentUser().Email;
                if (RoleService.IsUserInRole(email, "SuperAdmin") || RoleService.IsUserInRole(email, "Admin") || RoleService.IsUserInRole(email, "Editor"))
                {
                    return RedirectToAction("Index", "Magazines");
                }

                if (RoleService.IsUserInRole(email, "Influencer"))
                {
                    return RedirectToAction("Index", "Influencer");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid) 
            {
                SetMessage("Algo salio mal, intenta iniciar sesion de nuevo.", BootstrapAlertTypes.Warning);
                return Redirect("/"); 
            }

            if (System.Web.HttpContext.Current.Request.Cookies["currentUser"] != null)
            {
                HttpCookie currentUserCookie = System.Web.HttpContext.Current.Request.Cookies["currentUser"];
                System.Web.HttpContext.Current.Response.Cookies.Remove("currentUser");
                 currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                 currentUserCookie.Value = null;
                 System.Web.HttpContext.Current.Response.SetCookie(currentUserCookie);
            }
            
            if (!WebSecurity.Login(model.Email, model.Password)) {
                SetMessage("Alguno de los datos que ingresaste son incorrectos. Intentelo de nuevo.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Login"); 
            }

            var user = UserService.GetUserbyEmail(model.Email);

            if (user == null)
            {
                SetMessage("Lo sentimos, no se encontró el usuario. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Login");
            }

            if (user.ActivationDate == null)
            {
                SetMessage("Valida tu cuenta de correo electrónico para poder iniciar sesión.", BootstrapAlertTypes.Warning);
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, model.Email));
            claims.Add(new Claim(ClaimTypes.Email, model.Email));
            var id = new ClaimsIdentity(claims,
                                        DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);

            SetAuthCookie(model.Email);

            SetEncryptedCookie(Configuration.UserCookie, new Dictionary<String, String>
            {
                { "Email", user.Email },
                { "Code", user.Code }
            });

            var currentUser = MagazineService.GetCurrentUser();
            SetCookie("currentUser", user.Email, true);

            if (RoleService.IsUserInRole(model.Email, "SuperAdmin"))
            {
                return RedirectToAction("Index", "Magazines");
            }

            if (RoleService.IsUserInRole(model.Email, "Admin"))
            {
                return RedirectToAction("Index", "Magazines");
            }

            if (RoleService.IsUserInRole(model.Email, "Influencer"))
            {
                return RedirectToAction("Index", "Influencer");
            }
            var editor = RoleService.IsUserInRole(model.Email, "Editor");

            if (RoleService.IsUserInRole(model.Email, "Editor"))
            {
                return RedirectToAction("Index", "Magazines");
            }

            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/");
            }

            return Redirect(returnUrl);
        }

        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            if (Session["facebooktoken"] != null)
            {
                var fb = new Facebook.FacebookClient();
                string accessToken = Session["facebooktoken"] as string;
                var logoutUrl = fb.GetLogoutUrl(new { access_token = accessToken, next = "http://expose.mx" });


                Session.RemoveAll();
                return Redirect("/");
            }

            TempData.Clear();

            if (Request.Cookies[Configuration.UserCookie] != null) { RemoveCookie(Configuration.UserCookie); }

            return Redirect("/");
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                var currentUser = MagazineService.GetCurrentUser();
                SetCookie("currentUser", currentUser.Email, true);

                if (RoleService.IsUserInRole(currentUser.Email, "SuperAdmin"))
                {
                    return RedirectToAction("Index", "Magazines");
                }

                if (RoleService.IsUserInRole(currentUser.Email, "Admin"))
                {
                    return RedirectToAction("Index", "Magazines");
                }

                if (RoleService.IsUserInRole(currentUser.Email, "Influencer"))
                {
                    return RedirectToAction("Index", "Influencer");
                }
                var editor = RoleService.IsUserInRole(currentUser.Email, "Editor");

                if (RoleService.IsUserInRole(currentUser.Email, "Editor"))
                {
                    return RedirectToAction("Index", "Magazines");
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserService.GetUserbyEmail(model.Email);

            if (user != null)
            {
                SetMessage("Lo sentimos, correo electrónico no disponible. Inténtelo de nuevo con uno distinto.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            var createdUser = UserService.CreateUser_And_MembershipAccount(model.Email, model.Password, model.UserName);

            if (createdUser == null)
            {
                SetMessage("Lo sentimos, ha ocurrido un error al intentar realizar el registro. Inténtelo de nuevo.", BootstrapAlertTypes.Success);
                return View(model);
            }

            RoleService.AddUserToRole(model.Email, "Admin");
            //RoleService.AddUserToRole(model.Email, "Influencer");

            SetMessage("Registro exitoso. Se ha enviado un email de confirmación. Es necesario validar tu cuenta para poder iniciar sesión.", BootstrapAlertTypes.Success);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserService.GetUserbyEmail(model.Email);

            if (user != null)
            {
                SetMessage("Lo sentimos, este email ya se encuentra en uso. Inténtelo con uno distinto.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            var userCrate = UserService.CreateAdmin(model.Email, model.Password);

            if (userCrate == null)
            {
                SetMessage("Ocurrio un error al realizar el registro, inténtelo de nuevo.", BootstrapAlertTypes.Success);
                return View(model);
            }

            RoleService.AddUserToRole(model.Email, "Admin");

            SetMessage("Se te ha enviado un email de validación. Debes validar tu cuenta para iniciar sesión.", BootstrapAlertTypes.Success);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ValidateUser(String code)
        {
            var model = new ValidateUserViewModel
            {
                Code = code,
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ValidateUser(ValidateUserViewModel model)
        {
            if (UserService.ActivateUser(model.Code))
            {
                SetMessage(UserService.ServiceTempData);
                return RedirectToAction("Login");
            }

            ModelState.Merge(UserService.ServiceModelState);
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserService.GetUserbyEmail(model.Email);

            if (user == null)
            {
                SetMessage("Se ha enviado un correo de recuperación a " + user.Email + ". No olvides revisar tu bandeja de SPAM.", BootstrapAlertTypes.Success);
                return View(model);
            }

            var code = Guid.NewGuid().ToString();

            if (!UserService.CreateSmartLink(user.Email, code, DateTime.Now.AddHours(1), SmartLinkTypes.PasswordRecovery))
            {
                SetMessage("Lo sentimos, error al enviar correo de recuperación. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            using (var emailServices = new EmailService())
            {
                emailServices.SetRecipient(user.Email);

                emailServices.Password(code, user.Email);

                if (!emailServices.Send())
                {
                    SetMessage("Lo sentimos, error al enviar correo de recuperación. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                    return View(model);
                }

                SetMessage("Se ha enviado un correo de recuperación a " + user.Email + ". No olvides revisar tu bandeja de SPAM.", BootstrapAlertTypes.Success);
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult PasswordChange(String code)
        {
            if (String.IsNullOrEmpty(code))
                return Redirect("/");

            var model = new PasswordChangeViewModel()
            {
                Code = code,
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PasswordChange(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de verificar los datos e intentarlo de nuevo.", BootstrapAlertTypes.Warning);
                return View(model);
            }

            var smartLink = UserService.GetPasswordRecoveryLink(model.Code);
            if (smartLink == null)
            {
                SetMessage("Lo sentimos, código inválido. Inténtelo de nuevo más tarde.", BootstrapAlertTypes.Warning);
                return View(model);
            }
            
            bool changePasswordSucceeded;
            try
            {
                var userName = UserService.GetUserbyEmail(smartLink.Email);
                var passwordToken = WebSecurity.GeneratePasswordResetToken(smartLink.Email);
                changePasswordSucceeded = WebSecurity.ResetPassword(passwordToken, model.Password);
            }
            catch (Exception ex)
            {
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
            {
                if (UserService.ConsumePasswordRecoverySmartLink(model.Code))
                {
                    if (!WebSecurity.Login(smartLink.Email, model.Password, persistCookie: true))
                    {
                        SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo más tarde.", BootstrapAlertTypes.Danger);
                        return View(model);
                    }
                    
                    var userName = smartLink.Email;
                    var user = UserService.GetUserbyEmail(userName);
                    if (user == null)
                    {
                        SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo más tarde.", BootstrapAlertTypes.Danger);
                        return View(model);
                    }
                    
                    SetAuthCookie(user.Email);
                    SetEncryptedCookie(Configuration.UserCookie, new Dictionary<String, String>
                    {
                       { "Code", user.Code },
                       { "Email", user.Email }
                    });
                    
                    SetMessage("Su contraseña ha sido cambiada. Ya puedes iniciar sesión.", BootstrapAlertTypes.Success);
                    return Redirect("/");
                }
                else
                {
                    SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo más tarde.", BootstrapAlertTypes.Danger);
                    return View(model);
                }
            }
            else
            {
                SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo más tarde.", BootstrapAlertTypes.Danger);
                return View(model);
            }
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            //return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }


            string first_name = "";
            string last_name = "";
            string email = "";
            if (result.ExtraData.Keys.Contains("accesstoken"))
            {
                var client = new FacebookClient(result.ExtraData["accesstoken"]);
                dynamic me = client.Get("me");
                Session["facebooktoken"] = result.ExtraData["accesstoken"];
                first_name = me.first_name;
                last_name = me.last_name;
                email = me.email;
            }
            else
            {
                String[] full_name = result.ExtraData["name"].ToString().Split(' ');
                if (full_name[0] != null)
                {
                    first_name = full_name[0];
                }
                if (full_name[1] != null)
                {
                    last_name = full_name[1];
                }


            }


            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {

                SetAuthCookie(email);

                var user = UserService.GetUserbyEmail(email);

                if (user == null)
                {
                    SetMessage("Error.", BootstrapAlertTypes.Danger);
                    return Redirect("/Account/login");
                }

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
                var id = new ClaimsIdentity(claims,
                                            DefaultAuthenticationTypes.ApplicationCookie);

                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignIn(id);



                SetEncryptedCookie(Configuration.UserCookie, new Dictionary<String, String>
                        {
                            { "Email", user.Email },
                            { "Code", user.Code }
                        });


                if (RoleService.IsUserInRole(user.Email, "Admin"))
                {
                    return Redirect("/Magazines");
                }

                if (RoleService.IsUserInRole(user.Email, "Influencer"))
                {
                    return Redirect("/Home/Index");
                }

                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/Influencer");


                //SetEncryptedCookie(Configuration.UserCookie, first_name, Configuration.CookieLifeTime);
            }


            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, email);

                SetAuthCookie(email);


                //SetEncryptedCookie(Configuration.UserCookie, first_name, Configuration.CookieLifeTime);

                //UserService.LastSeenUpdate(email);

                //if (Request.Cookies["isFromCheckOut"] != null)
                //{
                //    SetCookie("isFromCheckOut", false, false);
                //    RemoveCookie("isFromCheckOut");
                //    return Redirect("/store/checkout");


                //}

                return Redirect("/");
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                //return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel
                {
                    UserName = result.UserName,
                    ExternalLoginData = loginData,
                    Name = first_name
                });
            }
        }
        [HttpGet]
        public ActionResult Perfil()
        {
            var user = UserService.GetCurrentUser();
            return View(user);
        }
        public ActionResult EditUser()
        {
            var user = UserService.GetCurrentUser();
            return View(user);
        }

        public ActionResult SaveImageProfile(HttpPostedFileBase file)
        {
            var imageCode = "";
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var Image = file;
                //System.Web.HttpContext.Current.Request.Files["file"];
                imageCode = Image.FileName;

                if (Image != null && Image.ContentLength > 0)
                {
                    var imageModel = ResourceService.SaveImage(Server.MapPath("~/content/data/"), Image, false);

                    if (imageModel == null)
                    {
                        ModelState.AddModelError("", "No se pudo guardar la imagen. Intentalo de nuevo.");

                        return Json("No se pudo guardar la imagen", JsonRequestBehavior.AllowGet); ;
                    }

                    imageCode = imageModel.FullFileName;
                }

                if (UserService.AsignProfilePicture(imageCode))
                {
                    return Json(new
                    {
                        Message = "Su imagen de perfíl ha sido actualizada exitosamente.",
                        ImageCode = imageCode,
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult UpdateInfo(String UserName, String Email)
        {
            if (!UserService.GetCurrentUser().Email.Equals(Email))
            {
                if (UserService.UpdateInfo(UserName, Email))
                {
                    SetMessage("Inicia sesión con tu nuevo correo electrónico.", BootstrapAlertTypes.Success);
                    return RedirectToAction("LogOff");
                }
            }

            if (UserService.UpdateInfo(UserName, Email))
            {
                SetMessage("Tus datos se han actualizado exitosamente.", BootstrapAlertTypes.Success);
                return RedirectToAction("Perfil");
            }

            SetMessage("Ha ocurrido un error el intentar actualizar tus datos. Inténtalo de nuevo.", BootstrapAlertTypes.Success);

            return RedirectToAction("Index", "Magazines");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;


            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }


            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (EFDataBase db = new EFDataBase())
                {
                    User user = db.Users().FirstOrDefault(u => u.Email.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        //db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        //db.SaveChanges();



                        bool facebookVerified;


                        var client = new Facebook.FacebookClient(Session["facebooktoken"].ToString());
                        dynamic response = client.Get("me", new { fields = "id,verified" });
                        if (response.ContainsKey("verified"))
                        {
                            facebookVerified = response["verified"];
                        }
                        else
                        {
                            facebookVerified = false;
                        }
                        var code = Guid.NewGuid().ToString();

                        var newUser = new User();


                        newUser.ActivationDate = DateTime.Now;
                        newUser.Code = code;
                        newUser.Email = model.UserName;
                        newUser.UserName = model.Name;
                        newUser.CreateDate = DateTime.Now.ToString();
                        newUser.FbId = response["id"];

                        db.UsersList.Add(newUser);

                        db.SaveChanges();

                        RoleService.AddUserToRole(model.UserName, "Influencer");

                        user = UserService.GetUserbyEmail(model.UserName);

                        if (user == null)
                        {
                            SetMessage("Error.", BootstrapAlertTypes.Danger);
                            return Redirect("/Account/login");
                        }


                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);


                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, model.UserName));
                        claims.Add(new Claim(ClaimTypes.Email, model.UserName));
                        var id = new ClaimsIdentity(claims,
                                                    DefaultAuthenticationTypes.ApplicationCookie);

                        var ctx = Request.GetOwinContext();
                        var authenticationManager = ctx.Authentication;
                        authenticationManager.SignIn(id);


                        SetAuthCookie(model.UserName);


                        SetEncryptedCookie(Configuration.UserCookie, new Dictionary<String, String>
                        {
                            { "Email", user.Email },
                            { "Code", user.Code }
                        });


                        if (RoleService.IsUserInRole(model.UserName, "Admin"))
                        {
                            return Redirect("/Magazines");
                        }

                        if (RoleService.IsUserInRole(model.UserName, "Influencer"))
                        {
                            return Redirect("/Influencer");
                        }

                        if (!String.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }


            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }


                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }


                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);


                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }


            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }
            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }
            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }

}