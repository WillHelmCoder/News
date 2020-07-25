using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Dal.Interfaces;
using Dal.Repositories;
using Dal.Models;
using Dal.Classes;
using News.Models;

namespace Dal.Services
{
    public class ResourceService : BaseService
    {
        public ResourceService(Repository repository, ILog logger)
            : base(repository, logger)
        {
        }

        private Int32 PhotoMinWidth { get { return 350; } }
        private Int32 PhotoMinHeight { get { return 175; } }
        public Boolean IsEmailUnique(String email)
        {
            return !Repository.Users().Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        public ImageUploadModel SaveImage(String mapPath, HttpPostedFileBase file, bool isThumbnail)
        {
            var guid = Guid.NewGuid().ToString();

            var format = GetFormat(file);
            if (format == null) return new ImageUploadModel { Error = "El formato de la imagen no se encuentra entre los permitidos (jpg | png)" };

            var universalFormat = ".jpeg";

            var path = mapPath +"/"+ guid + format;
            path = Path.ChangeExtension(path, universalFormat);
            System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);
            if (image == null) return new ImageUploadModel { Error = "No se encontro la imagen en el servidor" };

            var realH = isThumbnail ? PhotoMinHeight : image.Height;
            var realW = isThumbnail ? PhotoMinWidth : image.Width;

            using (var input = new Bitmap(image))
            {
                using (var thumb = new Bitmap(realW, realH))
                using (var graphic = Graphics.FromImage(thumb))
                {
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = SmoothingMode.AntiAlias;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphic.DrawImage(input, 0, 0, realW, realH);

                    using (var output = File.Create(path))
                    {
                        switch (file.ContentType)
                        {
                            case "image/jpeg": thumb.Save(output, ImageFormat.Jpeg); break;
                            case "image/png": thumb.Save(output, ImageFormat.Png); break;
                        }
                    }
                }
            }

            return new ImageUploadModel
            {
                FullFileName = String.Format("{0}{1}", guid, universalFormat),
                Height = image.Height,
                Width = image.Width,
                FileName = guid,
                Format = universalFormat,
            };
        }

        public String CreateBase64Image(String Path, String Base64, Int32 CategoryId)
        {

            //EL PRIMER CONFLICTO ERA EN EL MODELO SaveImageModel. LOS MODELOS EN LA APP Y EL SISTEMA NO COINCIDIAN. YA SE CAMBIARON LOS NOMBRES

            //EL METODO ACTUAL PARA GUARDAR LA IMAGEN NO FUNCIONA CUANDO EL INICIO DEL STRING INDICA EL FORMATO (data:image/png;base64,)  

            //SE CREO LA SIGUIENTE LINEA PARA REMOVER LOS PRIMEROS 22 CARACTERES

            string base64Formated = Base64.Substring(Base64.IndexOf(',') + 1);
            base64Formated = base64Formated.Trim('\0');

            DirectoryInfo mapPath = new DirectoryInfo( System.Web.HttpContext.Current.Server.MapPath("/Content/data/"));

            if (!mapPath.Exists)
            {
                mapPath.Create();
            }

            var guid = Guid.NewGuid().ToString();


            //SE CAMBIO EL FORMATO A PNG
            string format = "image/png";
            string ext = "png";

            byte[] bytes = Convert.FromBase64String(base64Formated);

            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }

            var path = Path + guid + "." + ext;

            var size = GetImageSize(CategoryId);

             Int32 MinWidth = size.Width;
             Int32 MinHeight = size.Height;
            var realH = image.Height < MinHeight ? MinHeight : image.Height;
            var realW = image.Width < MinWidth ? MinWidth : image.Width;

            //realH = image.Height > MaxHeight ? MaxHeight : image.Height;
            //realW = image.Width > MaxWidth ? MaxWidth : image.Width;

            using (var input = new Bitmap(image))
            {
                using (var thumb = new Bitmap(realW, realH))
                using (var graphic = Graphics.FromImage(thumb))
                {
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.SmoothingMode = SmoothingMode.AntiAlias;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphic.DrawImage(input, 0, 0, realW, realH);

                    using (var output = File.Create(path))
                    {
                        switch (format)
                        {
                            case "image/jpeg": thumb.Save(output, ImageFormat.Jpeg); break;
                            case "image/png": thumb.Save(output, ImageFormat.Png); break;
                        }

                        thumb.Save(output, ImageFormat.Jpeg);
                    }
                }
            }

            return guid + "." + ext;
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
        public City GetCityByName(String name)
        {
            return Repository.Cities()
                .SingleOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
        public List<State> GetStates()
        {
            return Repository.States()
                .OrderBy(x => x.Name)
                .ToList();
        }
        public List<City> GetCities(Int32 id)
        {
            return Repository.Cities()
                .Where(x => x.StateId == id)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public Int32 GetStateByCityId(Int32 id)
        {
            return Repository.Cities()
                .Where(x => x.CityId == id)
                .Select(x => x.StateId)
                .SingleOrDefault();
        }

        public ImageSizeByCategoryModel GetImageSize(Int32 Id)
        {
            var item = Repository.Categories()
                .SingleOrDefault(x => x.CategoryId == Id);

            var model = new ImageSizeByCategoryModel()
            {
                Height = item.Height,
                Width = item.Width
            };


            return model;
        }
    }
}