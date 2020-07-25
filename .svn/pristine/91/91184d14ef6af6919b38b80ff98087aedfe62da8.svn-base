using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using News.Models;
using News = Dal.Models.News;

namespace News.Controllers
{
    public class KeyPointController : UserAuthController
    {
        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;

        public KeyPointController(ResourceService resourceService, UserService userService,
            MagazineService magazineService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
        }

        private EFDataBase db = new EFDataBase();
        // GET: KeyPoint
        public ActionResult Index()
        {
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
            var model = MagazineService.GetKeyPointsContainersById(magId);
            return View(model);
        }

        // GET: KeyPoint/Create
        public ActionResult CreateKeyPointContainer()
        {

            return View();
        }

        // POST: KeyPoint/Create
        [HttpPost]
        public ActionResult CreateKeyPointContainer(PostCreateKeyPointModel model)
        {
            try
            {
                if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
                int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

                var length = model.Image.Count();
                var keyPointContainerModel = new KeyPointsContainer
                {
                    Name = model.KeyPointsContainer.Name,
                    Description = model.KeyPointsContainer.Description,
                    Kguid = Guid.NewGuid().ToString(),
                    MagazineId = magId,
                    IsDeleted = false
                };

                var keyPointContainer = MagazineService.CreateKeyPointContainer(keyPointContainerModel);

                for (var i = 0; i < length; i++)
                {
                    var title = model.Title != null ? model.Title.Count >= i ? model.Title[i] : "" : "";
                    var description = model.Description != null ? model.Description.Count >= i ? model.Description[i] : "" : "";
                    var url = model.Url != null ? model.Url.Count >= i ? model.Url[i] : "" : "";
                    var mainImage = model.Image != null ? model.Image.Count >= i ? model.Image[i] : "" : "";
                    var secondaryImage = model.ImageBackground != null ? model.ImageBackground.Count >= i ? model.ImageBackground[i] : "" : "";
                    var order = model.Order != null ? model.Order.Count >= i ? model.Order[i] : "1" : "1";

                    var keyPoint = new KeyPoint
                    {
                        Title = title,
                        Description = description,
                        Url = url,
                        MainImage = mainImage,
                        SecondaryImage = secondaryImage,
                        IsDeleted = false,
                        CreationDate = DateTime.Now,
                        Order = Convert.ToInt32(order),
                        KeyPointsContainerId = keyPointContainer.KeyPointsContainerId
                    };

                    MagazineService.CreateKeyPoint(keyPoint);

                }

                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                return View();
            }
            return View();
        }

        public ActionResult AddKeyPoint(Int32 id)
        {
            var getKeypoints = MagazineService.GetsKeyPointsById(id);

            var keyPointContainer= new KeyPointsContainer()
            {
                KeyPointsContainerId = getKeypoints.KeyPointsContainerId,
                Name = getKeypoints.Name,
                Description = getKeypoints.Description,
                Kguid = getKeypoints.Kguid
            };

            var model = new CreateKeyPointContainerModel()
            {
                KeyPointsContainer = keyPointContainer,
                KeyPoints = getKeypoints.KeyPoints
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult AddKeyPoint(PostCreateKeyPointModel model)
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);
            var length = model.Image.Count();
            var keyPointContainerId = Request["keyPointContainerId"];

            for (var i = 0; i < length; i++)
            {
                var title = model.Title != null ? model.Title.Count >= i ? model.Title[i] : "" : "";
                var description = model.Description != null ? model.Description.Count >= i ? model.Description[i] : "" : "";
                var url = model.Url != null ? model.Url.Count >= i ? model.Url[i] : "" : "";
                var mainImage = model.Image != null ? model.Image.Count >= i ? model.Image[i] : "" : "";
                var secondaryImage = model.ImageBackground != null ? model.ImageBackground.Count >= i ? model.ImageBackground[i] : "" : "";
                var order = model.Order != null ? model.Order.Count >= i ? model.Order[i] : "1" : "1";

                var keyPoint = new KeyPoint
                {
                    Title = title,
                    Description = description,
                    Url = url,
                    MainImage = mainImage,

                    SecondaryImage = secondaryImage,
                    IsDeleted = false,
                    CreationDate = DateTime.Now,
                    Order = Convert.ToInt32(order),
                    KeyPointsContainerId = Convert.ToInt32(keyPointContainerId)
                };

                MagazineService.CreateKeyPoint(keyPoint);

            }

            return RedirectToAction("KeyPoints", new{id=keyPointContainerId});
        }
        // GET: KeyPoint/Edit/5
        public ActionResult Edit(int id)
        {
            var getKeypoints = MagazineService.GetsKeyPointsById(id);
            return View(getKeypoints);
        }

        // POST: KeyPoint/Edit/5
        [HttpPost]
        public JsonResult Edit(EditKeyPointModel model)
        {
            try
            {
                var keyPointContainerModel = new KeyPointsContainer
                {
                    KeyPointsContainerId = model.KeyPointsContainer.KeyPointsContainerId,
                    Description = model.Description,
                    Name = model.KeyPointsContainer.Name
                };

                var keyPointContainer = MagazineService.EditKeyPointsContainer(keyPointContainerModel);
                var keyPointModel = new KeyPoint
                {
                    KeyPointId = model.KeyPointId,
                    Title = model.Title,
                    Description = model.Description,
                    MainImage = model.Image,
                    SecondaryImage = model.ImageBackground,
                    Order = Convert.ToInt32(model.Order),
                    Url = model.Url,
                     
                };

                var keyPoint = MagazineService.EditKeyPoint(keyPointModel);

                return Json("Exito");
            }
            catch (Exception e)
            {
                return Json("Algo salio mal.");
            }
        }

        public ActionResult KeyPoints(Int32 id)
        {
            var getKeypoints = MagazineService.GetsKeyPointsById(id);
            return View(getKeypoints);
        }

        // POST: KeyPoint/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {

            if (MagazineService.DeleteKeyPointContainer(id))
            {
                return Json("Elemento eliminado con exito.");
            }

            return Json("Algo salio mal");
        }
    }
}
