using System;
using System.Linq;
using Dal.Models;

namespace Dal.Interfaces
{
    public interface IDataBase
    {
        IQueryable<User> Users();
        IQueryable<Dal.Models.News> Newses();
        IQueryable<Category> Categories();
        IQueryable<Magazine> Magazines();
        IQueryable<SmartLink> SmartLinks();
        IQueryable<Comment> Comments();
        IQueryable<Visit> Visits();
        IQueryable<NewsLetter> NewsLetters();
        IQueryable<Slide> Slides();
        IQueryable<Slider> Sliders();
        IQueryable<UserMagazine> UserMagazines();
        IQueryable<State> States();
        IQueryable<City> Cities();
        IQueryable<Media> Medias();
        IQueryable<Report> Reports();
        IQueryable<Vote> Votes();
        IQueryable<Advertise> Advertises();
        IQueryable<AdCategory> AdCategories();
        IQueryable<Galery> Galeries();
        IQueryable<GaleryImage> GaleryImages();
        IQueryable<Image> Images();
        IQueryable<KeyPoint> KeyPoints();
        IQueryable<KeyPointsContainer> KeyPointsContainers();
        IQueryable<Quiz> Quizes();
        IQueryable<Question> Questions();
        IQueryable<Answer> Answers();
        IQueryable<Option> Options();
        IQueryable<ItemList> ItemsList();
        

        Int32 Save();
        void Add<T>(T model);
    }
}