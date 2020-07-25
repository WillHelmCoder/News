using Dal.AuthControllers;
using Dal.Models;
using Dal.Services;
using System;
using System.Web.Mvc;

namespace Dal.Controllers
{
    public class CategoriesController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;

        public CategoriesController(ResourceService resourceService, UserService userService, MagazineService magazineService)
        {
            ResourceService = resourceService;
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

            var categories = MagazineService.GetCategoriesByMagazineId(magId);
            if (categories.Count == 0)
            {
                return RedirectToAction("CreateCategory", "Wizard", new { id = magId });
            }
            return View(categories);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);
            var categories = MagazineService.GetCategoriesByMagazineId(magId);

            if (!relation || !ModelState.IsValid) { return RedirectToAction("MagazineNews", "News", new { id = magId }); }

            var model = new CategoryViewModel
            {
                MagazineId = magId,
                ParentsCategories = categories
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
            
            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(model.MagazineId, user.UserId);

            if (!relation)
            {
                SetMessage("Lo sentimos, no tienes permisos para visualizar este contenido.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de verificar los datos.", BootstrapAlertTypes.Danger);
                return View(model);
            }

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

            var create = MagazineService.CreateCategory(model.Name, model.Permalink, imageCode, model.MagazineId, model.Width, model.Height, model.ParentCategoryId, model.showImage);
            if (create == null)
            {
                SetMessage("Lo sentimos, ha ocurrido un error al crear categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Categories");
            }

            SetMessage("Listo, categoría creada.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index", "Categories");
        }

        public ActionResult Edit(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var category = MagazineService.GetCategoryById(id);
            if (category == null)
            {
                SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            ViewBag.MagazineId = new SelectList(MagazineService.GetCurrentUserMagazines(), "MagazineId", "Title", category.MagazineId);
            var categories = MagazineService.GetCategoriesByMagazineId(magId);
            ViewBag.ParentCategory = category.ParentCategoryId ?? null;
            var model = new CategoryViewModel
            {
                Name = category.Name,
                CategoryId = category.CategoryId,
                LogoImage = category.Image,
                MagazineId = category.MagazineId,
                Permalink = category.Permalink,
                ParentsCategories = categories
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation)
            {
                SetMessage("Lo sentimos, no tienes permisos para visualizar este contenido.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de verificar los datos.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            ViewBag.MagazineId = new SelectList(MagazineService.GetCurrentUserMagazines(), "MagazineId", "Title", model.MagazineId);

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

            if (!MagazineService.EditCategory(model.CategoryId, model.Name, model.Permalink, imageCode, model.MagazineId))
            {
                SetMessage(MagazineService.ServiceTempData);

                SetMessage("Lo sentimos, ha ocurrido un error al editar categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Categories");
            }

            SetMessage("Listo, categoría editada.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index", "Categories");
        }

        public ActionResult Delete(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var category = MagazineService.GetCategoryById(id);

            if (category == null)
            {
                SetMessage("No se encontro la categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }


            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Int32 id)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var user = UserService.GetCurrentUser();
            var relation = UserService.UserInMagazine(magId, user.UserId);

            if (!relation || !ModelState.IsValid) return RedirectToAction("Index", "Magazines");

            var category = MagazineService.GetCategoryById(id);
            if (category == null)
            {
                SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                return RedirectToAction("Index", "Magazines");
            }

            if (!MagazineService.DeleteCategory(id))
            {
                SetMessage(MagazineService.ServiceTempData);
                return RedirectToAction("Index", "Categories");
            }

            SetMessage("Listo, categoría eliminada.", BootstrapAlertTypes.Success);
            return RedirectToAction("Index", "Categories");
        }

        public ActionResult GetImageSize(Int32 Id)
        {
            var getSize = ResourceService.GetImageSize(Id);
            return Json(getSize, JsonRequestBehavior.AllowGet);
        }
    }
}
