using Dal.AuthControllers;
using Dal.Models;
using Dal.Repositories;
using Dal.Services;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Dal.Controllers
{
    public class ItemListsController : UserAuthController
    {
        private EFDataBase db = new EFDataBase();
        private readonly MagazineService MagazineService;
        private readonly UserService UserService;
        private readonly ListService ListService;

        public ItemListsController(UserService userService, MagazineService magazineService, ListService listService)
        {
            UserService = userService;
            MagazineService = magazineService;
            ListService = listService;
        }

        public ActionResult Index()
        {
            if (Request.Cookies["MagazineId"].Value == null) { SetMessage("Lo sentimos, ha ocurrido un error. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return RedirectToAction("Index", "Magazines"); }
            int magId = Int32.Parse(Request.Cookies["MagazineId"].Value);

            var list = ListService.GetListOfItemsByMagazineId(magId);

            var model = new ListViewModel()
            {
                ItemsList = list
            };

            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new ListViewModel()
            {
                MagazineId = Int32.Parse(Request.Cookies["MagazineId"].Value),
            });
        }

        [HttpPost]
        public ActionResult Create(ListViewModel model, HttpPostedFileBase FileUp)
        {
            var guid = Guid.NewGuid().ToString();

            var items = new ItemList()
            {
                MagazineId = model.MagazineId,
                Name = model.Name,
                ItemListId = model.ItemListId,
                Content = model.Content,
                IsDeleted = model.IsDeleted
            };

            if (FileUp.ContentLength > 0)
            {
                string ext = System.IO.Path.GetExtension(FileUp.FileName);
                if (ext == ".pdf")
                {
                    string Filename = guid + ext;

                    var originalDirectory =
                            new DirectoryInfo(string.Format(Server.MapPath("~/Content/Pdf/")));

                    string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "");

                    bool isExists = System.IO.Directory.Exists(pathString);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, Filename);

                    FileUp.SaveAs(path);

                    items.Content = "http://TheContent.mx/Content/pdf/" + Filename;
                }
                else
                {
                    SetMessage("Solo puedes subir archivos Pdf.", BootstrapAlertTypes.Danger);
                    return View(model);
                }
            }

            try
            {
                var list = ListService.CreateListOfItems(items);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var list = ListService.GetListOfItemByListId(id);

            var model = new ListViewModel()
            {
                ItemListId = id,
                MagazineId = list.MagazineId,
                Name = list.Name,
                Content = list.Content
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ListViewModel model)
        {
            var items = new ItemList()
            {
                ItemListId = model.ItemListId,
                MagazineId = model.MagazineId,
                Name = model.Name,
                Content = model.Content
            };

            try
            {
                var list = ListService.EditListOfItems(items);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            var list = ListService.GetListOfItemByListId(id);

            var model = new ListViewModel()
            {
                ItemListId = id,
                MagazineId = list.MagazineId,
                Name = list.Name,
                Content = list.Content
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                var list = ListService.DeleteListOfItem(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete", new { id = id });
            }
        }
    }
}
