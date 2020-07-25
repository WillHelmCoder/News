using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Dal.Models;
using Dal.Interfaces;
using Database = WebMatrix.Data.Database;

namespace Dal.Repositories
{
    public class Repository
    {
        private readonly IDataBase DataBase;

        public Repository(IDataBase database)
        {
            DataBase = database;
        }
        #region IQueryables
        public IQueryable<User> Users()
        {
            var abc = DataBase.Users();

            return abc;
        }
        public IQueryable<Dal.Models.News> Newses()
        {
            return DataBase.Newses()
                .Include(x => x.Category)
                .Where(x => !x.Category.IsDeleted)
                .Where(x => !x.IsDeleted);
        }
        public IQueryable<Category> Categories()
        {
            return DataBase.Categories()
                .Where(x => !x.IsDeleted)
                .Include(x => x.Magazine);
        }
        public IQueryable<Comment> Comments()
        {
            return DataBase.Comments()
                .Include(x => x.Users)
                .Include(x => x.News)
                .Where(x => !x.IsDeleted);
        }
        public IQueryable<Visit> Visits()
        {
            return DataBase.Visits()
                .Where(x => !x.Ip.Contains("66.220."))
                .Where(x => !x.Ip.Contains("66.249."))
                .Where(x => !x.Ip.Contains("208.115."))
                .Where(x => !x.Ip.Contains("199.59."))
                .Where(x => !x.Ip.Contains("127.0.0"))
                .Where(x => x.social != "Pruebas")
                .Where(x => x.social != "Bot");
        }
        public IQueryable<Visit> topVisits(Int32 cantidad)
        {
            return DataBase.Visits()
             .Take(cantidad);


            // .Include(x => x.News)
            //.Include(x => x.Users);
        }
        public IQueryable<Magazine> Magazines()
        {
            return DataBase.Magazines();
        }
        public IQueryable<SmartLink> SmartLinks()
        {
            return DataBase.SmartLinks();
        }
        public IQueryable<NewsLetter> NewsLetters()
        {
            return DataBase.NewsLetters();
        }
        public IQueryable<Slider> Sliders()
        {
            return DataBase.Sliders();
        }
        public IQueryable<Slide> Slides()
        {
            return DataBase.Slides();
        }
        public IQueryable<UserMagazine> UserMagazines()
        {
            return DataBase.UserMagazines();
        }
        public IQueryable<State> States()
        {
            return DataBase.States();
        }
        public IQueryable<City> Cities()
        {
            return DataBase.Cities();
        }
        public IQueryable<Media> Medias()
        {
            return DataBase.Medias();
        }
        public IQueryable<Report> Reports()
        {
            return DataBase.Reports();
        }
        public IQueryable<Vote> Votes()
        {
            return DataBase.Votes();
        }

        public IQueryable<Advertise> Advertises()
        {
            return DataBase.Advertises();
        }

        public IQueryable<AdCategory> AdCategories()
        {
            return DataBase.AdCategories();
        }

        public IQueryable<Galery> Galeries()
        {
            return DataBase.Galeries();
        }

        public IQueryable<GaleryImage> GaleryImages()
        {
            return DataBase.GaleryImages();
        }

        public IQueryable<Image> Images()
        {
            return DataBase.Images();
        }

        public IQueryable<KeyPoint> KeyPoints()
        {
            return DataBase.KeyPoints().Where(x => !x.IsDeleted);
        }

        public IQueryable<KeyPointsContainer> KeyPointsContainers()
        {
            return DataBase.KeyPointsContainers().Include(x => x.KeyPoints).Where(x => !x.IsDeleted);
        }

        public IQueryable<Quiz> Quizes()
        {
            return DataBase.Quizes();
        }

        public IQueryable<Question> Questions()
        {
            return DataBase.Questions().Where(x => !x.IsDeleted);
        }

        public IQueryable<Answer> Answers()
        {
            return DataBase.Answers().Where(x => !x.IsDeleted);
        }

        public IQueryable<Option> Options()
        {
            return DataBase.Options().Where(x => !x.IsDeleted);
        }

        public IQueryable<ItemList> ListsOfItems()
        {
            return DataBase.ItemsList().Where(x => !x.IsDeleted);
        }

        #endregion IQueryables

        public void Save()
        {
            DataBase.Save();
        }
        public void Add<T>(T model)
        {
            DataBase.Add(model);
        }
    }
}