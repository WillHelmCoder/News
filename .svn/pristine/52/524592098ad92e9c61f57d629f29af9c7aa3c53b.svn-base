using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Dal.Interfaces;
using Dal.Models;
using Microsoft.Owin.Security.Provider;
using NUnit.Framework;

namespace Dal.Repositories
{
    public class EFDataBase : DbContext, IDataBase
    {
        public EFDataBase() : base() { }

        #region DbSet
        public DbSet<User> UsersList { set; get; }
        public DbSet<Dal.Models.News> NewsesList { set; get; }
        public DbSet<Category> CategoriesList { set; get; }
        public DbSet<Magazine> MagazinesList { set; get; }
        public DbSet<SmartLink> SmartLinksList { set; get; }
        public DbSet<Comment> CommentsList { set; get; }
        public DbSet<Visit> VisitsList { set; get; }
        public DbSet<NewsLetter> NewsLettersList { set; get; }
        public DbSet<Slider> SlidersList { set; get; }
        public DbSet<Slide> SlidesList { set; get; }
        public DbSet<UserMagazine> UserMagazinesList { set; get; }
        public DbSet<City> CitiesList { get; set; }
        public DbSet<State> StatesList { get; set; }
        public DbSet<Media> MediasList { get; set; }
        public DbSet<Report> ReportsList { get; set; }
        public DbSet<Vote> VotesList { get; set; }
        public DbSet<Advertise> AdvertisesList { get; set; }
        public DbSet<AdCategory> AdCategoriesList { get; set; }
        public DbSet<Galery> GaleriesList { get; set; }
        public DbSet<GaleryImage> GaleryImagesList { get; set; }
        public DbSet<Image> ImagesList { get; set; }
        public DbSet<KeyPoint> KeyPointsList{ get; set; }
        public DbSet<KeyPointsContainer> KeyPointsContainersList { get; set; }
        public DbSet<Quiz> QuizesList { get; set; }
        public DbSet<Question> QuestionsList { get; set; }
        public DbSet<Answer> AnswersList { get; set; }
        public DbSet<Option> OptionList { get; set; }
        public DbSet<ItemList> ItemsListList { get; set; }
        
        #endregion DbSet
        #region IQueryable
        public IQueryable<User> Users()
        {
            return UsersList;
        }
        public IQueryable<Dal.Models.News> Newses()
        {
            return NewsesList;
        }
        public IQueryable<Category> Categories()
        {
            return CategoriesList;
        }
        public IQueryable<Magazine> Magazines()
        {
            return MagazinesList;
        }
        public IQueryable<SmartLink> SmartLinks()
        {
            return SmartLinksList;
        }
        public IQueryable<Comment> Comments()
        {
            return CommentsList;
        }
        public IQueryable<Visit> Visits()
        {

            return VisitsList;
        }
        public IQueryable<NewsLetter> NewsLetters()
        {
            return NewsLettersList;
        }
        public IQueryable<Slider> Sliders()
        {
            return SlidersList.Where(x => !x.IsDeleted);
        }
        public IQueryable<Slide> Slides()
        {
            return SlidesList.Where(x => !x.IsDeleted);
        }
        public IQueryable<UserMagazine> UserMagazines()
        {
            return UserMagazinesList;
        }
        public IQueryable<City> Cities()
        {
            return CitiesList;
        }
        public IQueryable<State> States()
        {
            return StatesList;
        }
        public IQueryable<Media> Medias()
        {
            return MediasList;
        }
        public IQueryable<Report> Reports()
        {
            return ReportsList;
        }
        public IQueryable<Vote> Votes()
        {
            return VotesList;
        }
        public IQueryable<Advertise> Advertises()
        {
            return AdvertisesList;
        }

        public IQueryable<AdCategory> AdCategories()
        {
            return AdCategoriesList;
        }

        public IQueryable<Galery> Galeries()
        {
            return GaleriesList.Where(x => !x.IsDeleted);
        }

        public IQueryable<GaleryImage> GaleryImages()
        {
            return GaleryImagesList.Where(x => !x.IsDeleted);
        }

        public IQueryable<Image> Images()
        {
            return ImagesList;
        }

        public IQueryable<KeyPoint> KeyPoints()
        {
            return KeyPointsList.Where(x => !x.IsDeleted);
        }

        public IQueryable<KeyPointsContainer> KeyPointsContainers()
        {
            return KeyPointsContainersList.Where(x => !x.IsDeleted);
        }

        public IQueryable<Quiz> Quizes()
        {
            return QuizesList;
        }

        public IQueryable<Question> Questions()
        {
            return QuestionsList;
        }

        public IQueryable<Answer> Answers()
        {
            return AnswersList;
        }

        public IQueryable<Option> Options()
        {
            return OptionList;
        }


        public IQueryable<ItemList> ItemsList()
        {
            return ItemsListList;
        }
        #endregion IQueryable

        public void Add<T>(T model)
        {
            var theType = model.GetType().Name;
            switch (theType)
            {
                case "User": UsersList.Add(model as User); return;
                case "News": NewsesList.Add(model as Dal.Models.News); return;
                case "Category": CategoriesList.Add(model as Category); return;
                case "Magazine": MagazinesList.Add(model as Magazine); return;
                case "SmartLink": SmartLinksList.Add(model as SmartLink); return;
                case "Comment": CommentsList.Add(model as Comment); return;
                case "Visit": VisitsList.Add(model as Visit); return;
                case "NewsLetter": NewsLettersList.Add(model as NewsLetter); return;
                case "Slider": SlidersList.Add(model as Slider); return;
                case "Slide": SlidesList.Add(model as Slide); return;
                case "UserMagazine": UserMagazinesList.Add(model as UserMagazine); return;
                case "State": StatesList.Add(model as State); return;
                case "City": CitiesList.Add(model as City); return;
                case "Media": MediasList.Add(model as Media); return;
                case "Report": ReportsList.Add(model as Report); return;
                case "Vote": VotesList.Add(model as Vote); return;
                case "Advertise": AdvertisesList.Add(model as Advertise); return;
                case "AdCategory": AdCategoriesList.Add(model as AdCategory); return;
                case "Galery": GaleriesList.Add(model as Galery); return;
                case "GaleryImage": GaleryImagesList.Add(model as GaleryImage); return;
                case "Image": ImagesList.Add(model as Image); return;
                case "KeyPoint": KeyPointsList.Add(model as KeyPoint); return;
                case "KeyPointsContainer": KeyPointsContainersList.Add(model as KeyPointsContainer); return;
                case "Quiz": QuizesList.Add(model as Quiz); return;
                case "Question": QuestionsList.Add(model as Question); return;
                case "Answer": AnswersList.Add(model as Answer); return;
                case "Option": OptionList.Add(model as Option); return;
                case "ItemList": ItemsListList.Add(model as ItemList); return;
                
                default: throw new Exception("The type " + theType + " is not supported.");
            }
        }
        public Int32 Save() { return base.SaveChanges(); }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;

            modelBuilder.Entity<Category>()
                .HasRequired(x => x.Magazine);

        }


    }
}