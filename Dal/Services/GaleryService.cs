using Dal.Interfaces;
using Dal.Repositories;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Models;
using System.Data.Entity;

namespace Dal.Services
{
    public class GaleryService : BaseService
    {
        public GaleryService(Repository repository, ILog logger) : base(repository, logger) { }

        public List<Galery> GetGaleries_ByMGuid(String mGuid)
        {
            return Repository.Galeries()
                .Include(x => x.GaleryImages)
                .Include(x => x.GaleryImages.Select(z => z.Image))
                .Where(x => x.Magazine.Guid.Equals(mGuid))
                .ToList();
        }

        public Galery GetGalery_ByMGuid(String mGuid, Int32 id)
        {
            return Repository.Galeries()
                .Where(x => x.GaleryId == id)
                .Include(x => x.GaleryImages)
                .Include(x => x.GaleryImages.Select(z => z.Image))
                .SingleOrDefault(x => x.Magazine.Guid.Equals(mGuid));
        }

        public Boolean AddImageToGalery(GaleryImage item, Boolean isAnEdit)
        {
            var image = Repository.Images().SingleOrDefault(x => x.ImageId == item.ImageId);

            if (image == null)
                return false;

            var galery = Repository.Galeries().SingleOrDefault(x => x.GaleryId == item.GaleryId);

            if (galery == null)
                return false;

            if (!isAnEdit)
            {
                var model = new GaleryImage()
                {
                    GaleryId = item.GaleryId,
                    ImageId = item.ImageId,
                    CreationDate = DateTime.Now,
                    Description = item.Description,
                    Order = item.Order,
                    Title = item.Title
                };

                Repository.Add(model);
                Repository.Save();

                return true;
            }

            var galeryImage = Repository.GaleryImages().SingleOrDefault(x => x.GaleryImageId == item.GaleryImageId);

            if (galeryImage == null)
                return false;

            galeryImage.ImageId = item.ImageId;
            galeryImage.Description = item.Description;
            galeryImage.Order = item.Order;
            galeryImage.Title = item.Title;

            Repository.Save();

            return true;
        }

        public GaleryImageViewModel GetGaleryImageById(Int32 id)
        {
            var image = Repository.GaleryImages().SingleOrDefault(x => x.GaleryImageId == id);

            if (image == null)
                return null;

            var model = new GaleryImageViewModel()
            {
                GaleryId = image.GaleryId,
                GaleryImageId = image.GaleryImageId,
                Image = Repository.Images().SingleOrDefault(x => x.ImageId == image.ImageId),
                ImageId = Repository.Images().SingleOrDefault(x => x.ImageId == image.ImageId).ImageId
            };

            return model;
        }

        public Boolean DeleteGaleryImage(Int32 id)
        {
            var model = Repository.GaleryImages().SingleOrDefault(x => x.GaleryImageId == id);

            if (model == null)
                return false;

            model.IsDeleted = true;

            Repository.Save();

            return true;
        }

        public Galery GetGallery(Int32 id)
        {
            var model = Repository.Galeries().SingleOrDefault(x => x.GaleryId == id);

            return model;
        }

        public Image SaveImage(string alt, string imageName, string title, string IP)
        {
            var model = new Image()
            {
                Alt = alt,
                Code = imageName,
                Title = title,
                UploadIP = IP,
                UploadDate = DateTime.Now
            };

            Repository.Add(model);
            Repository.Save();

            return model;
        }
    }
}