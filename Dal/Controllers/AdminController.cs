using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dal.Models;
using Dal.Services;
using Dal.AuthControllers;
using System.IO;
using System.Xml.Linq;
using News.Classes;


namespace Dal.Controllers
{
    [Authorize]
    public class AdminController : BasicController
    {
        private readonly UserService UserService;
        private readonly MagazineService MagazineService;
        private readonly GaleryService GaleryService;
        private readonly ResourceService ResourseService;

        public AdminController(UserService userService, MagazineService magazineService, GaleryService galeryService, ResourceService resourceService)
        {
            UserService = userService;
            MagazineService = magazineService;
            GaleryService = galeryService;
            ResourseService = resourceService;
        }

        // GET: Admin
        public ActionResult Index()
        {
            var getMagazines = MagazineService.GetCurrentUserMagazines();
            return View();
        }

        public ActionResult Register()
        {
            var magazines = MagazineService.GetCurrentUserMagazines();
            ViewBag.Magazines = magazines;
            return View();
        }

        [HttpPost]
        public ActionResult Register(AdminRegister model)
        {
            var magazines = MagazineService.GetCurrentUserMagazines();
            ViewBag.Magazines = magazines;

            if (!ModelState.IsValid)
            {
                SetMessage("Lo sentimos, favor de verificar sus datos.", BootstrapAlertTypes.Danger);
                return View(model);
            }

            var user = UserService.GetUserbyEmail(model.email);

            if (user != null)
            {

                MagazineService.UserMagazine(user.UserId, model.MagazineId);
                SetMessage("Se ha invitado al usuario " + user.Email + " a colaborar en su revista", BootstrapAlertTypes.Success);
                return View(model);
            }

            var userCreate = UserService.CreateUser_And_MembershipAccount(model.email, "123456", "");
            if (userCreate == null)
            {
                SetMessage("Ocurrió un error al realizar el registro. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                return Redirect("/");
            }

            MagazineService.UserMagazine(userCreate.UserId, model.MagazineId);

            string[] usersToAdd = new string[] { model.email };
            string[] rolToAdd = new string[] { model.rol };

            Roles.AddUserToRole(model.email, model.rol);
            //UserService.UserInRol(userCreate.UserId, model.rol);<

            SetMessage("Listo, hemos registrado a su nuevo miembro. El usuario: " + model.email + " recibirá un correo electrónico con instrucciones a seguir.", BootstrapAlertTypes.Danger);
            return View();
        }

        public ActionResult Trends()
        {

            //var oauth_token = "3252901578-szjfYbMPHy7hGI5qdqD8XzGoZFfqJjBSO8Cv8AF";
            //var oauth_token_secret = "0RTKk1tpxxHMifmNgHnGCHz8uTqjoPBIg8wex5905Navx";
            //var oauth_consumer_key = "dajFe99NeVeVwdpBCTVJSumVO";
            //var oauth_consumer_secret = "Mfg3hMcH0JLPz9bxgZ1JXP2Nx4p5wFa6EuU4ml9fhToMazefQ0";

            //// oauth implementation details
            //var oauth_version = "1.0";
            //var oauth_signature_method = "HMAC-SHA1";

            //// unique request details
            //var oauth_nonce = Convert.ToBase64String(
            //    new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            //var timeSpan = DateTime.UtcNow
            //    - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            //var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            //// message api details
            //var resource_url = "https://api.twitter.com/1.1/trends/place.json?id=1";
            //// create oauth signature
            //var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
            //                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            //var baseString = string.Format(baseFormat,
            //                            oauth_consumer_key,
            //                            oauth_nonce,
            //                            oauth_signature_method,
            //                            oauth_timestamp,
            //                            oauth_token,
            //                            oauth_version
            //                            );

            //baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            //var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
            //                        "&", Uri.EscapeDataString(oauth_token_secret));

            //string oauth_signature;
            //using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            //{
            //    oauth_signature = Convert.ToBase64String(
            //        hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            //}

            //// create the request header
            //var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
            //                   "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
            //                   "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
            //                   "oauth_version=\"{6}\"";

            //var authHeader = string.Format(headerFormat,
            //                        Uri.EscapeDataString(oauth_nonce),
            //                        Uri.EscapeDataString(oauth_signature_method),
            //                        Uri.EscapeDataString(oauth_timestamp),
            //                        Uri.EscapeDataString(oauth_consumer_key),
            //                        Uri.EscapeDataString(oauth_token),
            //                        Uri.EscapeDataString(oauth_signature),
            //                        Uri.EscapeDataString(oauth_version)
            //                );


            //// make the request

            //ServicePointManager.Expect100Continue = false;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            //request.Headers.Add("Authorization", authHeader);
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";


            //WebResponse response = request.GetResponse();
            //string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return View();
        }

        public ActionResult CreateImages()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            var imgName = "";
            var saveThumnail = new ImageUploadModel();
            var saveMainImage = new ImageUploadModel();
            try
            {
                foreach (string fileName in Request.Files)
                {
                    var guid = Guid.NewGuid().ToString();
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        //save Thumbnail
                        var originalDirectory = new DirectoryInfo(string.Format("{0}\\Content\\Thumbnails", Server.MapPath(@"\")));
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "");
                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        saveThumnail = ResourseService.SaveImage(pathString, file, true);

                        //saveMainImage
                        var originalDirectory2 = new DirectoryInfo(string.Format("{0}\\Content\\Data", Server.MapPath(@"\")));
                        string pathString2 = System.IO.Path.Combine(originalDirectory2.ToString(), "");
                        bool isExists2 = System.IO.Directory.Exists(pathString2);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString2);

                        saveMainImage = ResourseService.SaveImage(pathString2, file, false);

                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { MainImage = saveMainImage.FullFileName, Thumbnail = saveThumnail.FullFileName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public ActionResult GetImages()
        {
            var dir = new DirectoryInfo(string.Format("{0}\\Content\\Data", Server.MapPath(@"\")));

            var getNews = MagazineService.GetNewsWithOutThumbnail();
            foreach (var item in getNews)
            {
                Console.WriteLine(item.Image);
                string newDir = System.IO.Path.Combine(dir.ToString(), item.Image);
                bool isExists = System.IO.File.Exists(newDir);
                if (isExists)
                {
                    FileInfo f = new FileInfo(newDir);

                    var guid = Guid.NewGuid().ToString();
                    var thumbDir = new DirectoryInfo(string.Format("{0}\\Content\\Thumbnails", Server.MapPath(@"\")));
                    string pathString = System.IO.Path.Combine(thumbDir.ToString(), "");
                    bool thumbNailExist = System.IO.Directory.Exists(pathString);
                    if (!thumbNailExist)
                        System.IO.Directory.CreateDirectory(pathString);
                    Stream fs = System.IO.File.OpenRead(f.FullName);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(fs);


                    var realH = 175;
                    var realW = 350;

                    using (var input = new Bitmap(image))
                    {
                        using (var thumb = new Bitmap(realW, realH))
                        using (var graphic = Graphics.FromImage(thumb))
                        {
                            var path = thumbDir + "/" + guid + ".png";
                            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphic.DrawImage(input, 0, 0, realW, realH);

                            using (var output = System.IO.File.Create(path))
                            {
                                switch (MimeMapping.GetMimeMapping(f.Name))
                                {
                                    case "image/jpeg": thumb.Save(output, ImageFormat.Jpeg); break;
                                    case "image/png": thumb.Save(output, ImageFormat.Png); break;
                                }

                                MagazineService.AddThumbnail(item.NewsId, guid + ".png");
                                Console.WriteLine(guid + ".png");
                            }
                        }
                    }
                }

            }

            return View(getNews);
        }

        public ActionResult UploadImages()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            var imgName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    var guid = Guid.NewGuid().ToString();
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory =
                            new DirectoryInfo(string.Format("{0}\\Content\\Data", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);
                        var format = GetFormat(file);
                        imgName = guid + format;
                        var path = string.Format("{0}\\{1}", pathString, imgName);
                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = imgName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public ActionResult UploadImagesToGallery(Int32 id)
        {
            var getGallery = GaleryService.GetGallery(id);
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory =
                            new DirectoryInfo(string.Format("{0}\\Content\\Data", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                        var IP = Request.UserHostAddress;
                        var saveImage = GaleryService.SaveImage(getGallery.Alt, file.FileName, getGallery.Title, IP);

                        var newModel = new GaleryImage()
                        {
                            CreationDate = saveImage.UploadDate,
                            ImageId = saveImage.ImageId,
                            GaleryId = id,
                            Description = getGallery.Description,
                            Title = getGallery.Title,
                            Order = 1,
                        };

                        var SaveImageInGallery = GaleryService.AddImageToGalery(newModel, false);
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public String SiteMap()
        {
            /*
             Cache = 38
             Vertice = 41
             Nifor = 1043
             Unocero= 37
             */

            var id = 38;
            Sitemap sitemap = new Sitemap();
            sitemap.Add(new SitemapLocation
            {
                ChangeFrequency = SitemapLocation.eChangeFrequency.weekly,
                Url = "Http://cache.news"
            });

            var getNews = MagazineService.GetNewsByMagazineId(id);

            foreach (var item in getNews)
            {
                sitemap.Add(new SitemapLocation
                {
                    ChangeFrequency = SitemapLocation.eChangeFrequency.never,
                    Url = "Http://cache.news/noticia/" + item.NewsId + "/" + item.Permalink
                });
            }

            var getXml = sitemap.WriteSitemapToFile(AppDomain.CurrentDomain.BaseDirectory + "sitemap.xml");
            //MUESTRA EN PANTALLA EL XML
            var doc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "/Content/Sitemaps/" + getNews[0].Category.Magazine.Title + "sitemap.xml");
            string encodedXml = HttpUtility.HtmlEncode(doc);

            return encodedXml;
        }

        private String GetFormat(HttpPostedFileBase file)
        {
            switch (file.ContentType)
            {
                case "image/jpeg": return ".jpg";
                case "image/png": return ".png";
                default: return null;
            }
        }

    }
}