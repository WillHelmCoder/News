using System.Web.Mvc;
using Dal.AuthControllers;
using Dal.Interfaces;
using Dal.Repositories;
using Dal.Services;

namespace News.Controllers
{
    public class NewsletterController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();

        private readonly ResourceService ResourceService;
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly InfluencerService InfluencerService;
        private readonly IRoleService RoleService;


        public NewsletterController(ResourceService resourceService, IRoleService roleService, UserService userService, MagazineService magazineService, InfluencerService influencerService)
        {
            ResourceService = resourceService;
            UserService = userService;
            MagazineService = magazineService;
            RoleService = roleService;
            InfluencerService = influencerService;
        }

        // GET: Newsletter
        public ActionResult Index()
        {
            return View();
        }

        // GET: Newsletter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Newsletter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Newsletter/Create
        [HttpPost]
        public ActionResult Create(string mGuid, string email)
        {
            try
            {
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Newsletter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Newsletter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Newsletter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Newsletter/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
