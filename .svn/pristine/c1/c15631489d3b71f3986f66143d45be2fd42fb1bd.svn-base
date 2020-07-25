using Dal.Interfaces;
using Dal.Models;
using Dal.Repositories;
using Dal.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using User = Dal.Models.User;

namespace Dal.Services
{
    public class MagazineService : BaseService
    {
        public MagazineService(Repository repository, ILog logger) : base(repository, logger) { }

        public User GetCurrentUser()
        {
            var user = Repository.Users()
                .SingleOrDefault(x => x.Email.Equals(CurrentUserEmail));

            return user;
        }
        public User GetUserById(int? id)
        {
            return Repository.Users()
                .Where(x => x.UserId == id)
                .SingleOrDefault();
        }

        #region MagazineRelated
        public bool EditMagazine(int id, string title, string description, string logo, string address, int city, string email, bool isPrivate, string domain, string fb, string tw, string gA)
        {
            try
            {
                var magazine = Repository.Magazines()
                    .SingleOrDefault(x => x.MagazineId == id);

                if (magazine == null)
                {
                    SetMessage("No se encontró la revista.", BootstrapAlertTypes.Danger);
                    return false;
                }

                magazine.Title = title;
                magazine.Description = description;
                magazine.Logo = logo;
                magazine.Address = address;
                magazine.CityId = city;
                magazine.Email = email;
                magazine.IsPrivate = isPrivate;
                magazine.Domain = domain;
                magazine.TwitterPage = tw;
                magazine.FacebookPage = fb;
                magazine.GoogleAnalyticsId = gA;

                Repository.Save();

                return true;

            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public int CountUserMagazines()
        {
            var item = Repository.Magazines()
                .Include(x => x.User).Count(x => x.User.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase));

            return item;
        }

        public Magazine Create(string title, string description, string logo, string address, int cityId, string email, bool isPrivate, string domain, string fb, string tw, string gA)
        {
            try
            {
                var city = Repository.Cities().Where(x => x.CityId == cityId);
                if (city == null) { return null; }

                var user = Repository.Users()
                    .SingleOrDefault(x => x.Email.Equals(CurrentUserEmail));
                if (user == null) { return null; }

                var code = Guid.NewGuid().ToString();
                var magazine = new Magazine
                {
                    Title = title,
                    Code = code,
                    Description = description,
                    Logo = logo,
                    Address = address,
                    CityId = cityId,
                    Email = email,
                    UserId = user.UserId,
                    IsPrivate = isPrivate,
                    Domain = domain,
                    TwitterPage = tw,
                    FacebookPage = fb,
                    GoogleAnalyticsId = gA,
                    Guid = code,
                };

                Repository.Add(magazine);
                Repository.Save();

                UserMagazine(user.UserId, magazine.MagazineId);

                return magazine;
            }
            catch (Exception ex)
            {

                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }

        public bool UserMagazine(int UserId, int MagazineId)
        {
            var alreadyExists = Repository.UserMagazines()
                .Where(x => x.MagazineId == MagazineId)
                .SingleOrDefault(x => x.UserId == UserId);

            if (alreadyExists != null)
                return false;

            var model = new UserMagazine
            {
                UserId = UserId,
                MagazineId = MagazineId
            };

            Repository.Add(model);
            Repository.Save();

            return true;
        }
        public Magazine GetMagazineByCode(string code)
        {
            var item = Repository.Magazines()
                .Include(x => x.User)
                .SingleOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

            return item;
        }
        public Magazine GetMagazineById(int id)
        {
            var item = Repository.Magazines()
               .Include(x => x.City)
               .Include(x => x.User)
               .SingleOrDefault(x => x.MagazineId == id);

            return item;
        }
        public Magazine GetCurrentMagazine(int id)
        {
            var item = Repository.Magazines()
                        .SingleOrDefault(x => x.MagazineId == id);
            return item;
        }
        public List<Magazine> GetCurrentUserMagazines()
        {
            var items = Repository.UserMagazines()
                .Where(x => x.User.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase))
                .Include(x => x.User)
                .Include(x => x.Magazine)
                .Include(x => x.Magazine.Categories)
                .Select(x => x.Magazine)
                .ToList();

            return items;
        }
        public List<Magazine> GetAllMagazines()
        {
            var items = Repository.Magazines()
                .Include(x => x.User)
                .Include(x => x.Categories)
                .OrderByDescending(x => x.MagazineId)
                .ToList();

            return items;
        }
        public List<Magazine> GetMagazinesByUserId(int id)
        {
            var items = Repository.UserMagazines()
                .Include(x => x.User)
                .Include(x => x.Magazine)
                .Where(x => x.UserId == id)
                .Select(x => x.Magazine)
                .ToList();

            return items;
        }
        public List<Magazine> GetNearlyMagazines(int id)
        {
            var items = Repository.Magazines()
                        .Where(x => x.CityId == id)
                        .ToList();
            return items;
        }
        public List<UserMagazine> GetUserMagazinesByUserAdmin()
        {
            var items = Repository.UserMagazines()
                .Include(x => x.Magazine)
                .Include(x => x.Magazine.User)
                .Include(x => x.User)
                .Where(x => !x.User.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.Magazine.User.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return items;
        }
        #endregion
        #region CategoryRelated
        public bool EditCategory(int id, string name, string permalink, string image, int magazineId)
        {
            try
            {
                var category = Repository.Categories()
                    .SingleOrDefault(x => x.CategoryId == id);

                if (category == null)
                {
                    SetMessage("No se encontro la categoría.", BootstrapAlertTypes.Danger);
                    return false;
                }

                category.Name = name;
                category.Image = image;
                category.MagazineId = magazineId;
                category.Permalink = permalink;

                Repository.Save();

                return true;

            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public bool DeleteCategory(int id)
        {
            try
            {
                var category = Repository.Categories()
                   .SingleOrDefault(x => x.CategoryId == id);

                if (category == null)
                {
                    SetMessage("No se encontro la categoría.", BootstrapAlertTypes.Danger);
                    return false;
                }

                category.IsDeleted = true;

                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public int GetVisitsByCategoryId(int id)
        {
            var news = GetNewsesByCategoryId(id);

            var count = 0;
            foreach (var item in news)
            {
                var visits = CountVisits(item.NewsId);
                count = count + visits;
            }

            return count;
        }
        public int MostUsedCatCount(int id)
        {
            var catCount = Repository.Newses()
                .Where(x => x.CategoryId == id)
                .Count();
            return catCount;
        }
        public Category CreateCategory(string name, string permalink, string image, int magazineId, int width, int height, int? parentCategoryId, bool? showImage)
        {
            try
            {
                var restaurant = Repository.Magazines()
                    .SingleOrDefault(x => x.MagazineId == magazineId);

                if (restaurant == null)
                {
                    SetMessage("No se encontró la revista.", BootstrapAlertTypes.Danger);
                    return null;
                }

                var item = new Category
                {
                    Name = name,
                    Image = image,
                    IsDeleted = false,
                    MagazineId = magazineId,
                    Permalink = permalink,
                    Width = width,
                    Height = height,
                    ParentCategoryId = parentCategoryId,
                    showImage = showImage
                };

                Repository.Add(item);
                Repository.Save();

                return item;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }
        public Category MostViewedCat(int id)
        {
            var catId = Repository.Visits()
                .Where(x => x.News.Category.MagazineId == id)
                .Where(x => !x.Ip.Contains("66.220."))
                .Where(x => !x.Ip.Contains("66.249."))
                .Where(x => !x.Ip.Contains("208.115."))
                .Where(x => !x.Ip.Contains("199.59."))
                .Where(x => !x.Ip.Contains("127.0.0"))
                .GroupBy(x => x.News.CategoryId)
                .OrderByDescending(x => x.Count())
                .Take(1)
                .Select(x => x.Key)
                .SingleOrDefault();

            var item = GetCategoryById(catId);

            return item;
        }
        public Category GetCategoryById(int id)
        {
            var item = Repository.Categories()
                .Include(x => x.Magazine)
                .SingleOrDefault(x => x.CategoryId == id);
            return item;
        }
        public Category MostUsedCat(int id)
        {
            var catId = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Category.MagazineId == id)
                .GroupBy(x => x.CategoryId)
                .OrderByDescending(x => x.Count())
                .Take(1)
                .Select(x => x.Key)
                .SingleOrDefault();

            var item = GetCategoryById(catId);

            return item;
        }
        public List<Category> GetCategoriesByMagazineId(int id)
        {
            var items = Repository.Categories()
                .Where(x => x.MagazineId == id)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToList();

            return items;
        }
        public List<Category> GetAllCategories()
        {
            var items = Repository.Categories()
                .Include(x => x.Magazine)
                .ToList();

            return items;
        }
        public List<Category> GetCurrentUserCategories()
        {
            var items = Repository.Categories()
                .Include(x => x.Magazine)
                .Include(x => x.Magazine.User)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Magazine.User.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return items;
        }
        public List<GroupByCategory> LastNewsFooter()
        {
            var item = (from v in Repository.Newses().Where(x => !x.Keywords.Contains("MarcoPaz"))
                        join n in Repository.Categories() on v.CategoryId equals n.CategoryId
                        where n.MagazineId == 7
                        group v by new
                        {
                            Category = n.Name,
                            CategoryId = n.CategoryId
                        }
                            into x
                        select x
                        )
                        .AsEnumerable()
                        .Select(x => new GroupByCategory
                        {
                            CategoryId = x.Key.CategoryId,
                            Category = x.Key.Category,
                            Count = x.Where(v => v.CategoryId == x.Key.CategoryId).Count()
                        })
                        .ToList();
            return item;
        }
        #endregion
        #region NewsRelated
        public bool EditNews(int id, string title, string description, string image, string thumbnail, string body, int categoryId, string permalink, string metaDescription, string metaTags, string altImage, string videoUrl, DateTime? creationDate)
        {
            try
            {
                var news = Repository.Newses()
                    .Where(x => !x.Keywords.Contains("MarcoPaz"))
                    .SingleOrDefault(x => x.NewsId == id);

                if (news == null)
                {
                    SetMessage("No se encontró la noticia.", BootstrapAlertTypes.Danger);
                    return false;
                }

                var category = Repository.Categories()
                    .SingleOrDefault(x => x.CategoryId == categoryId);

                if (category == null)
                {
                    SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                    return false;
                }

                var withVideo = false;

                if (videoUrl != null)
                    withVideo = true;

                news.Title = title;
                news.Description = description;
                news.Image = image;
                news.Thumbnail = thumbnail;
                news.Body = body;
                news.CategoryId = categoryId;
                news.Alt = altImage;
                news.MetaDesc = metaDescription;
                news.Keywords = metaTags;
                news.Permalink = permalink;
                news.VideoEmbed = videoUrl;
                news.WithVideo = withVideo;
                news.CreationDate = (creationDate.HasValue ? creationDate.Value : news.CreationDate);

                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }

        public List<Models.News> GetNewsWithOutThumbnail()
        {
            var item = Repository.Newses().Where(x => x.Thumbnail == null).ToList();
            return item;
        }
        public bool AddThumbnail(int id, string thumbnail)
        {
            try
            {
                var news = Repository.Newses()
                    .Where(x => !x.Keywords.Contains("MarcoPaz"))
                    .SingleOrDefault(x => x.NewsId == id);

                news.Thumbnail = thumbnail;
                Repository.Save();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteNews(int id)
        {
            try
            {
                var news = Repository.Newses()
                    .Where(x => !x.Keywords.Contains("MarcoPaz"))
                    .SingleOrDefault(x => x.NewsId == id);

                if (news == null)
                {
                    SetMessage("No se encontró el producto.", BootstrapAlertTypes.Danger);
                    return false;
                }

                news.IsDeleted = true;

                Repository.Save();
                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public Dal.Models.News CreateNews(string title, string description, string image, string thumbnail, string body, int categoryId, string permalink, string metaDescription, string metaTags, string altImage, string videoUrl, DateTime creationDate)
        {
            var userId = GetCurrentUser().UserId;
            try
            {
                var category = Repository.Categories()
                    .SingleOrDefault(x => x.CategoryId == categoryId);

                if (category == null)
                {
                    SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                    return null;
                }

                var withVideo = false;

                if (videoUrl != null)
                    withVideo = true;

                var news = new Dal.Models.News
                {
                    UserId = userId,
                    Title = title,
                    Description = description,
                    Image = image,
                    Thumbnail = thumbnail,
                    Body = body,
                    IsDeleted = false,
                    CategoryId = categoryId,
                    CreationDate = creationDate,
                    Alt = altImage,
                    MetaDesc = metaDescription,
                    Keywords = metaTags,
                    Permalink = permalink,
                    VideoEmbed = videoUrl,
                    WithVideo = withVideo,
                    UpVotes = 0,
                    DownVotes = 0,
                    VisitsCount = 0,
                    CommentsCount = 0
                };

                Repository.Add(news);
                Repository.Save();

                return news;

            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }
        public Dal.Models.News CloneNew(int clonedFrom, int categoryId)
        {
            try
            {
                var category = Repository.Categories()
                    .SingleOrDefault(x => x.CategoryId == categoryId);

                if (category == null)
                {
                    SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger); return null;
                }

                var ownerMagazine = Repository.Magazines().SingleOrDefault(x => x.MagazineId == category.MagazineId);

                if (ownerMagazine == null)
                {
                    SetMessage("No se encontró la revista destino. Inténtelo de nuevo.", BootstrapAlertTypes.Danger); return null;
                }

                var originalNew = GetNewsById(clonedFrom);

                var thankNote = "<p>Extraído del artículo original en <a href='" + ownerMagazine.Domain + "/noticia/"
                    + originalNew.NewsId
                    + "/"
                    + originalNew.Permalink
                    + "' target='_blank' >"
                    + originalNew.Title
                    + "</a>.<br />Redactado por la revista: " + originalNew.Category.Magazine.Title + "</p>";

                var clonedNew = new Dal.Models.News
                {
                    UserId = originalNew.UserId,
                    Title = originalNew.Title,
                    Description = originalNew.Description,
                    Image = originalNew.Image,
                    Body = originalNew.Body,
                    CategoryId = categoryId,
                    CreationDate = DateTime.Now,
                    Alt = originalNew.Alt,
                    MetaDesc = originalNew.MetaDesc,
                    Keywords = originalNew.Keywords,
                    Permalink = originalNew.Permalink,
                    ClonedFrom = originalNew.NewsId,
                    IsClon = true,
                    ThankNote = thankNote,
                    VisitsCount = 0,
                };

                Repository.Add(clonedNew);
                Repository.Save();

                return clonedNew;

            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }
        public Dal.Models.News GetNewsById(int id)
        {
            var item = Repository.Newses()
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .SingleOrDefault(x => x.NewsId == id);

            return item;
        }
        public Dal.Models.News GetNewsByIdAndPermalink(int id, string permalink)
        {
            var item = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .Include(x => x.Visits)
                .Where(x => x.Permalink.Equals(permalink, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefault(x => x.NewsId == id);

            return item;
        }
        public List<Dal.Models.News> GetNewsesByCategoryId(int id)
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.CategoryId == id)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> GetLastNewsesByCategoryId(int idCategory, int id)
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.CategoryId == idCategory && x.NewsId != id)
                .OrderByDescending(x => x.CreationDate)
                .Take(4)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> GetAllNewses()
        {
            var items = Repository.Newses()
                .Where(x => !x.IsDeleted)
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> GetAllPublicMagsNews()
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => !x.Category.Magazine.IsPrivate)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> GetNewsesByDateNewses(DateTime fecha)
        {
            var newdate = fecha.AddDays(1);

            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .Include(x => x.Visits)
                .Where(x => x.CreationDate >= fecha)
                .Where(x => x.CreationDate <= newdate)
                .Where(x => x.Category.MagazineId == 7)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> GetNewsByMagazineId(int id)
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Category.MagazineId == id)
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .OrderByDescending(x => x.CreationDate)
                .ToList();
            return items;
        }
        public List<Dal.Models.News> GetPublicNewsByMagazineId(int id, int a)
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Category.MagazineId == id)
                .OrderByDescending(x => x.CreationDate)
                .Take(5)
                .ToList();
            return items;
        }
        public List<Dal.Models.News> GetNewsesByMagazineId(int id)
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .Where(x => x.Category.MagazineId == id)
                .Where(x => !x.IsDeleted)
                .Where(x => x.VisitsCount != null)
                .OrderByDescending(x => x.CreationDate)
                .ToList();
            return items;
        }
        public NewsWithVisitsModel GetNewsWithVisitsByMagazineId(int id)
        {
            var items = GetNewsesByMagazineId(id);

            if (!items.Any()) { return null; }

            var itemCount = items.Count;
            var visitCount = 0;

            var visits = new List<int>();
            foreach (var item in items)
            {
                int count = (item.VisitsCount ?? 0);

                visitCount = visitCount + count;

                visits.Add(count);
            }

            var model = new NewsWithVisitsModel
            {
                TotalNews = items.Count,
                VisitCounts = visits,
                TotalVisits = visitCount
            };

            return model;
        }
        public List<Last10CountModel> GetTop10NewsVisitsByMagazineId(int id)
        {
            var items = GetNewsesByMagazineId(id);

            List<Last10CountModel> counts = new List<Last10CountModel>();
            foreach (var item in items)
            {
                var otra = (from v in Repository.Visits()
                            where v.NewsId == item.NewsId
                            where !v.Ip.Contains("66.220.")
                            where !v.Ip.Contains("66.249.")
                            where !v.Ip.Contains("208.115.")
                            where !v.Ip.Contains("199.59.")
                            where !v.Ip.Contains("127.0.0")
                            group v by new { newTitle = v.News.Title, newId = v.NewsId }
                                into x
                            select x)
                            .Select(x => new Last10CountModel
                            {
                                NewTitle = x.Key.newTitle,
                                NewsId = x.Key.newId,
                                Count = x.GroupBy(v => v.Ip).Count()
                            }).ToList();
                counts.AddRange(otra);
            }
            var count = counts.OrderByDescending(x => x.Count)
                              .Take(10)
                              .ToList();
            Random rnd = new Random();

            var list = count.OrderBy(item => rnd.Next()).ToList();

            return list;
        }
        public List<Last10CountModel> GetLast10NewsByMagazineId(int id)
        {
            var items = GetNewsesByMagazineId(id);

            List<Last10CountModel> counts = new List<Last10CountModel>();
            foreach (var item in items)
            {
                var otra = (from v in Repository.Visits()
                            where v.NewsId == item.NewsId
                            where !v.Ip.Contains("66.220.")
                            where !v.Ip.Contains("66.249.")
                            where !v.Ip.Contains("208.115.")
                            where !v.Ip.Contains("199.59.")
                            where !v.Ip.Contains("127.0.0")
                            group v by new { newTitle = v.News.Title, newId = v.NewsId }
                                into x
                            select x)
                            .Select(x => new Last10CountModel
                            {
                                NewTitle = x.Key.newTitle,
                                NewsId = x.Key.newId,
                                Count = x.GroupBy(v => v.Ip).Count()
                            }).ToList();
                counts.AddRange(otra);
            }
            var count = counts.OrderByDescending(x => x.NewsId)
                              .Take(10)
                              .ToList();
            return count;
        }
        #endregion
        #region VisitRelated
        public int CountVisits(int id)
        {
            var count = Repository.Visits()
                    .Where(x => x.NewsId == id)
                    .Where(x => !x.Ip.Contains("66.220."))
                    .Where(x => !x.Ip.Contains("66.249."))
                    .Where(x => !x.Ip.Contains("208.115."))
                    .Where(x => !x.Ip.Contains("199.59."))
                    .Where(x => !x.Ip.Contains("127.0.0"))
                    .GroupBy(x => x.Ip)
                    .Count();

            return count;
        }

        public List<VisitsByMonth> VisitsOfDayByMonth(int Magazine)
        {

            var results = (from v in Repository.Visits()
                           where !v.Ip.Contains("66.220.")
                           where !v.Ip.Contains("66.249.")
                           where !v.Ip.Contains("208.115.")
                           where !v.Ip.Contains("199.59.")
                           where !v.Ip.Contains("127.0.0")
                           where v.Date.Year == DateTime.Now.Year
                           where v.Date.Month == DateTime.Now.Month
                           where v.News.Category.MagazineId == Magazine
                           where !v.News.IsDeleted
                           group v by new { day = v.Date.Day } into x
                           select x)
                         .Select(x => new VisitsByMonth
                         {
                             Day = x.Key.day,
                             Count = x.Where(v => v.social == "Público").Count(),
                             CountFb = x.Where(v => v.social == "fb").Count(),
                             CountTw = x.Where(v => v.social == "tw").Count(),
                         })

                         .OrderByDescending(x => x.Day)
                         .ToList();

            List<VisitsByMonth> items = new List<VisitsByMonth>();

            foreach (var item in results)
            {
                var tempDate = DateTime.Now.AddDays(-DateTime.Now.Day).AddDays(+item.Day).ToShortDateString();

                var date = DateTime.ParseExact(tempDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                VisitsByMonth model = new VisitsByMonth
                {
                    Count = item.Count,
                    CountFb = item.CountFb,
                    CountTw = item.CountTw,
                    Day = item.Day,
                    Date = date
                };
                items.Add(model);
            }

            return items;
        }

        public List<Dal.Models.Visit> speedtest()
        {
            var item = Repository.topVisits(20)

                .OrderByDescending(x => x.VisitId)

                .ToList();
            return item;

        }

        #endregion
        #region InfluencerRelated
        public List<TopInfluencers> GetTop10Influencers(int id)
        {
            var items = GetNewsesByMagazineId(id);

            var counts = new List<TopInfluencers>();
            foreach (var item in items)
            {
                var otra = (from v in Repository.Visits()
                            where !v.Ip.Contains("66.220.")
                            where !v.Ip.Contains("66.249.")
                            where !v.Ip.Contains("208.115.")
                            where !v.Ip.Contains("199.59.")
                            where !v.Ip.Contains("127.0.0")
                            where v.NewsId == item.NewsId
                            where v.UserId != null
                            group v by new { UserMail = v.Users.Email, UserId = v.UserId, Ip = v.Ip }
                                into x
                            select x)
                            .Select(x => new TopInfluencers
                            {
                                User = x.Key.UserMail,
                                UserId = x.Key.UserId,
                                Count = x.GroupBy(v => v.Ip).Count()
                            }).ToList();
                counts.AddRange(otra);
            }

            var count = (from v in counts
                         group v by new { userId = v.UserId, user = v.User } into x
                         select x).AsEnumerable()
                        .Select(x => new TopInfluencers
                        {
                            UserId = x.Key.userId,
                            User = x.Key.user,
                            Count = x.Where(v => v.User.Equals(x.Key.user)).Sum(v => v.Count)
                        }).OrderByDescending(x => x.Count).Take(10).ToList();

            return count;
        }
        public EditorViewModel GetEditorWithNewsAndGeneratedVisitsById(int id, int magId)
        {
            var user = Repository.Users()
                .Where(x => x.UserId == id)
                .SingleOrDefault();

            var items = GetNewsAndVisitsByEditorId(user.UserId, magId);

            var model = new EditorViewModel
            {
                Editor = user,
                News = items,
            };

            return model;

        }

        public List<GroupNewsModel> GetNewsAndVisitsByEditorId(int id, int magId)
        {
            var items = (from v in Repository.Visits()
                         where v.News.UserId == id
                         where v.News.Category.MagazineId == magId
                         join n in Repository.Newses() on v.NewsId equals n.NewsId
                         group v by new { NewId = v.NewsId, Title = v.News.Title, Permalink = v.News.Permalink }
                             into x
                         select x)
                .AsEnumerable()
                .Select(x => new GroupNewsModel
                {
                    Id = x.Key.NewId,
                    Permalink = x.Key.Permalink,
                    Count = x.DistinctBy(v => v.Ip).Count(),
                    Title = x.Key.Title,
                })
                .OrderBy(v => v.Id)
                .ToList();

            return items;
        }

        public List<UserMagazine> GetMyEditors(int id)
        {
            //EditorListViewModel
            var editors = Repository.UserMagazines()
                                    .Include(x => x.User)
                                    .Where(x => x.MagazineId == id)
                                    .ToList();

            return editors;

        }

        public User getUserName(int id)
        {
            var userName = Repository.Users()
                           .SingleOrDefault(x => x.UserId == id);
            return userName;
        }

        public int GetCountsByUser(int id)
        {
            var count = Repository.Visits()
                                  .Where(x => x.News.UserId == id)
                                  .Count();
            return count;
        }

        public int GetCountsByNew(int id)
        {
            var count = Repository.Visits()
                        .Where(x => x.NewsId == id)
                        .Count();
            return count;
        }
        public List<TopInfluencers> GetMyInfluencers(int id)
        {
            var items = GetNewsesByMagazineId(id);

            List<TopInfluencers> counts = new List<TopInfluencers>();
            foreach (var item in items)
            {
                var otra = (from v in Repository.Visits()
                            where !v.Ip.Contains("66.220.")
                            where !v.Ip.Contains("66.249.")
                            where !v.Ip.Contains("208.115.")
                            where !v.Ip.Contains("199.59.")
                            where !v.Ip.Contains("127.0.0")
                            where v.News.Category.MagazineId == id
                            where v.NewsId == item.NewsId
                            where v.UserId != null
                            group v by new { UserMail = v.Users.Email, UserId = v.Users.UserId, Ip = v.Ip }
                                into x
                            select x)
                            .Select(x => new TopInfluencers
                            {
                                User = x.Key.UserMail,
                                Count = x.Where(v => !v.Ip.Contains("66.220.158.")).GroupBy(v => v.Ip).Count(),
                                UserId = x.Key.UserId
                            }).ToList();
                counts.AddRange(otra);
            }

            var count = (from v in counts
                         group v by new { user = v.User, userId = v.UserId } into x
                         select x).AsEnumerable()
                        .Select(x => new TopInfluencers
                        {
                            User = x.Key.user,
                            UserId = x.Key.userId,
                            Count = x.Where(v => v.User.Equals(x.Key.user)).Sum(v => v.Count)
                        }).OrderByDescending(x => x.Count).ToList();

            return count;
        }
        public List<TopInfluencers> GetTop100Influencer()
        {
            var items = (from v in Repository.Visits()
                         where !v.Ip.Contains("66.220.")
                         where !v.Ip.Contains("66.249.")
                         where !v.Ip.Contains("208.115.")
                         where !v.Ip.Contains("199.59.")
                         where !v.Ip.Contains("127.0.0")
                         where v.UserId != null
                         group v by new { UserMail = v.Users.Email }
                             into x
                         select x)
                                .Select(x => new TopInfluencers
                                {
                                    User = x.Key.UserMail,
                                    Count = x.Where(v => !v.Ip.Contains("66.220.158.")).GroupBy(v => v.Ip).Count()
                                })
                                .OrderByDescending(x => x.Count)
                                .Take(100)
                                .ToList();
            return items;
        }
        #endregion
        #region TagRelated
        public List<Dal.Models.News> GetNewsesByTag(string tag)
        {
            var item = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .Include(x => x.Visits)
                .Where(x => x.Keywords.Contains(tag))
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return item;
        }
        public List<Dal.Models.News> GetNewsByTag(string tag, int magazineId)
        {
            var items = Repository.Newses()
                .Where(x => x.Keywords.Contains(tag))
                .Where(x => x.Category.MagazineId == magazineId)
                .OrderByDescending(x => x.CreationDate)
                .Take(50)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> GetNewsByTagAndDateRange(string tag, DateTime from, DateTime to)
        {
            var items = Repository.Newses()
                .Where(x => x.Keywords.Contains(tag))
                .Where(x => x.CreationDate > from)
                .Where(x => x.CreationDate < to)
                .Take(50)
                .ToList();
            return items;
        }
        public List<Dal.Models.News> FindNewsByKeyword(string keyword, int magazineId)
        {
            var keys = keyword.Split(' ');

            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                  .Include(x => x.Visits)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Category.MagazineId == magazineId);

            items = keys.Aggregate(items, (current, key) => current.Where(x => x.Title.Contains(key) || x.Body.Contains(key) || x.Description.Contains(key) || x.Category.Name.Contains(keyword)));

            return items.ToList();
        }
        #endregion
        #region CommentRelated
        public List<Comment> GetComments(int id)
        {
            var items = Repository.Comments()
                .Where(x => x.NewsId == id)
                .Include(x => x.Users)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            List<Comment> comments = new List<Comment>();
            foreach (var item in items)
            {
                var a = new Comment
                {
                    CommentId = item.CommentId,
                    Content = item.Content,
                    CreationDate = item.CreationDate,
                    Users = item.Users,
                    NewsId = item.NewsId
                };

                comments.Add(a);
            }


            return comments;
        }
        public bool CreateComment(string texto, int newsId)
        {
            try
            {
                var user = Repository.Users()
                    .SingleOrDefault(x => x.Email.Equals(CurrentUserEmail, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    return false;
                }

                var noticia = Repository.Newses()
                    .SingleOrDefault(x => x.NewsId == newsId);

                if (noticia == null)
                {
                    return false;
                }

                var model = new Comment
                {
                    Content = texto,
                    CreationDate = DateTime.Now,
                    NewsId = newsId,
                    UserId = user.UserId,
                };
                Repository.Add(model);
                Repository.Save();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region SliderRelated
        public List<Slide> GetsSliders(string sguid)
        {
            var slider = Repository.Slides()
                .Include(x => x.News)
                .Include(x => x.Slider)
                .Where(x => x.Slider.sGuid.Equals(sguid))
                .OrderBy(x => x.Order)
                .ToList();

            return slider;
        }
        #endregion
        #region NewsLetterRelated
        public NewsLetter Create(string suscripcion)
        {
            try
            {
                var item = Repository.NewsLetters()
                    .SingleOrDefault(x => x.Email.Equals(suscripcion, StringComparison.OrdinalIgnoreCase));
                if (item != null)
                {
                    return item;
                }
                var model = new NewsLetter
                {
                    Email = suscripcion
                };
                Repository.Add(model);
                Repository.Save();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        #region ReportRelated
        public Report GetReportById(int id)
        {
            return Repository.Reports()
                .Where(x => x.ReportId == id)
                .Include(x => x.Title)
                .SingleOrDefault();
        }

        public List<Report> GetMyReports(int id)
        {

            var items = Repository.Reports()
                    .Where(x => x.MagazineId == id)
                    .DistinctBy(x => x.MediaId)


               // .AsEnumerable()
               //.Select(x => new ShowReports
               //{
               //    MediaId = 1 //x.Key.MediaId,
               //    ,Promedio =1// getProm(x.Key.MediaId),
               //    ,Medio = "1" //getMedioName(x.Key.MediaId)
               //})

               .ToList();


            return items;
        }

        public string getMedioName(int id)
        {
            return Repository.Reports()
                    .Where(x => x.MediaId == id)
                    .Select(x => x.Media.Name).FirstOrDefault();
        }

        public int getProm(int id)
        {
            var model = Repository.Reports()
                    .Where(x => x.MediaId == id)
                    .Average(x => x.Impact);
            return Convert.ToInt32(model);
        }


        #endregion
        #region VotingSystemRelated
        public bool VoteUp(int Id, int userId, int type)
        {
            try
            {
                var status = false;

                if (type == 0)
                {
                    var singleNew = Repository.Newses().SingleOrDefault(x => x.NewsId == Id);

                    if (singleNew == null)
                    {
                        SetMessage("Lo sentimos, no se encontró la noticia. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                        return false;
                    }

                    status = ChangeVote(1, Id, userId, type);
                }
                if (type == 1)
                {
                    var comment = Repository.Comments().SingleOrDefault(x => x.CommentId == Id);

                    if (comment == null)
                    {
                        SetMessage("Lo sentimos, no se encontró la noticia. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                        return false;
                    }

                    status = ChangeVote(1, Id, userId, type);
                }


                if (!status)
                    return status;

                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public bool ChangeVote(int newStatus, int id, int userId, int type)
        {
            var currentStatus = 0;

            var vote = new Vote();

            if (type == 0)
            {

                vote = Repository.Votes().Where(x => x.UserId == userId).Where(x => x.NewId == id).SingleOrDefault();

                if (vote == null)
                {
                    var model = new Vote
                    {
                        NewId = id,
                        UpVoted = newStatus,
                        UserId = userId,
                    };

                    Repository.Add(model);
                    Repository.Save();
                }
            }

            if (type == 1)
            {

                vote = Repository.Votes().Where(x => x.UserId == userId).Where(x => x.CommentId == id).SingleOrDefault();

                if (vote == null)
                {
                    var model = new Vote
                    {
                        CommentId = id,
                        UpVoted = newStatus,
                        UserId = userId,
                    };

                    Repository.Add(model);
                    Repository.Save();
                }
            }

            if (vote.UpVoted == 1)
                currentStatus = 1;
            if (vote.UpVoted == 2)
                currentStatus = 2;

            if (currentStatus == 0 || currentStatus != newStatus)
            {
                vote.UpVoted = newStatus;
                Repository.Save();
                return true;
            }
            if (currentStatus == newStatus)
            {
                vote.UpVoted = 0;
                Repository.Save();
                return true;
            }

            if (type == 0)
                CalculateNewTotalVotes(id);
            if (type == 1)
                CalculateCommentTotalVotes(id);

            return false;
        }
        public bool VoteDown(int Id, int userId, int type)
        {
            try
            {
                var status = false;

                if (type == 0)
                {
                    var singleNew = Repository.Newses().SingleOrDefault(x => x.NewsId == Id);

                    if (singleNew == null)
                    {
                        SetMessage("Lo sentimos, no se encontró la noticia. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                        return false;
                    }

                    status = ChangeVote(2, Id, userId, type);
                }
                if (type == 1)
                {
                    var comment = Repository.Comments().SingleOrDefault(x => x.CommentId == Id);

                    if (comment == null)
                    {
                        SetMessage("Lo sentimos, no se encontró la noticia. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                        return false;
                    }

                    status = ChangeVote(2, Id, userId, type);
                }


                if (!status)
                    return status;

                return true;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public List<int> GetSplittedNewsVotes(int id)
        {
            try
            {
                var news = Repository.Newses()
                    .Where(x => !x.Keywords.Contains("MarcoPaz"))
                    .Include(x => x.Votes)
                    .SingleOrDefault(x => x.NewsId == id);

                if (news == null)
                {
                    SetMessage("Lo sentimos, no se encontró la noticia. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                    return null;
                }

                var newsVotes = Repository.Votes()
                    .Where(x => x.UpVoted != 0)
                    .Where(x => x.NewId == id).ToList();

                List<int> votes = new List<int>();

                var ups = newsVotes.Where(x => x.UpVoted == 1).ToList().Count();
                var downs = newsVotes.Where(x => x.UpVoted == 2).ToList().Count();

                votes.Add(ups);
                votes.Add(downs);

                return votes;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }
        public List<int> GetSplittedCommentVotes(int id)
        {
            try
            {
                var comment = Repository.Comments()
                    .SingleOrDefault(x => x.CommentId == id);

                if (comment == null)
                {
                    SetMessage("Lo sentimos, no se encontró la noticia. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                    return null;
                }

                var commentVotes = Repository.Votes()
                    .Where(x => x.CommentId == id)
                    .Where(x => x.UpVoted != 0)
                    .ToList();

                List<int> votes = new List<int>();

                var ups = commentVotes.Where(x => x.UpVoted == 1).ToList().Count();
                var downs = commentVotes.Where(x => x.UpVoted == 2).ToList().Count();

                votes.Add(ups);
                votes.Add(downs);

                return votes;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }
        public List<Vote> GetNewsVotes(int id)
        {
            try
            {
                var votes = Repository.Votes()
                    .Where(x => x.NewId == id)
                    .ToList();

                if (votes == null)
                {
                    SetMessage("Lo sentimos, no encontramos la votación. Inténtelo de nuevo.", BootstrapAlertTypes.Danger);
                    return null;
                }

                return votes;
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return null;
            }
        }
        public Vote GetVoteByUserId(int userId)
        {
            return Repository.Votes()
                .Where(x => x.UserId == userId)
                .SingleOrDefault();
        }
        public int AlreadyVoted(int newId, int userId)
        {
            var votes = GetNewsVotes(newId);
            var vote = GetVoteByUserId(userId);

            if (vote == null) { return 4; }

            if (vote.UpVoted == 1)
                return 1;
            if (vote.UpVoted == 2)
                return 2;

            return 0;
        }
        #endregion
        #region KeyPointsRelated
        public KeyPointsContainer GetsKeyPointsByGuid(string kguid)
        {
            var datas = Repository.KeyPointsContainers()
                .Include(x => x.KeyPoints)
                .SingleOrDefault(x => x.Kguid.Equals(kguid));

            List<KeyPoint> keypoints = datas.KeyPoints.Where(x => !x.IsDeleted).ToList();

            datas.KeyPoints = keypoints;

            return datas;
        }

        public List<KeyPointsContainer> GetKeyPointsContainersById(int id)
        {
            var items = Repository.KeyPointsContainers().Include(x => x.KeyPoints).Where(x => x.MagazineId == id).ToList();
            return items;
        }

        public KeyPointsContainer GetsKeyPointsById(int id)
        {
            var datas = Repository.KeyPointsContainers()
                .Include(x => x.KeyPoints)
                .SingleOrDefault(x => x.KeyPointsContainerId == id);

            List<KeyPoint> keypoints = datas.KeyPoints.Where(item => !item.IsDeleted).ToList();

            datas.KeyPoints = keypoints;

            return datas;
        }
        public bool CreateKeyPoint(KeyPoint model)
        {
            try
            {

                var newModel = new KeyPoint
                {

                    CreationDate = DateTime.Now,
                    Description = model.Description,
                    SecondaryImage = model.SecondaryImage,
                    MainImage = model.MainImage,
                    Order = model.Order,
                    Title = model.Title,
                    Url = model.Url,
                    KeyPointsContainerId = model.KeyPointsContainerId
                };

                Repository.Add(newModel);
                Repository.Save();

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public KeyPointsContainer CreateKeyPointContainer(KeyPointsContainer model)
        {
            try
            {
                Repository.Add(model);
                Repository.Save();

                return model;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public KeyPointsContainer EditKeyPointsContainer(KeyPointsContainer model)
        {
            var item =
                Repository.KeyPointsContainers()
                    .SingleOrDefault(x => x.KeyPointsContainerId == model.KeyPointsContainerId);
            try
            {
                item.Name = model.Name;
                item.Description = model.Description;

                Repository.Save();
                return item;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public KeyPoint EditKeyPoint(KeyPoint model)
        {
            try
            {
                var currentData = Repository.KeyPoints()
          .SingleOrDefault(x => x.KeyPointId == model.KeyPointId);


                currentData.Description = model.Description;
                currentData.SecondaryImage = model.SecondaryImage;
                currentData.MainImage = model.MainImage;
                currentData.Order = model.Order;
                currentData.Title = model.Title;
                currentData.Url = model.Url;

                Repository.Save();

                return currentData;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public bool DeleteKeyPointContainer(int id)
        {
            try
            {
                var currentItem = Repository.KeyPointsContainers().SingleOrDefault(x => x.KeyPointsContainerId == id);

                currentItem.IsDeleted = true;
                Repository.Save();

                return true;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        #endregion
        #region Advertise
        public bool CreateAd(AdvertiseViewModel model, string imageCode, int? adId)
        {
            try
            {
                var category = Repository.AdCategories()
                    .SingleOrDefault(x => x.AdCategoryId == model.AdCategoryId);

                if (category == null)
                {
                    SetMessage("No se encontró la categoría.", BootstrapAlertTypes.Danger);
                    return false;
                }

                if (!adId.HasValue)
                {
                    var ad = new Advertise
                    {
                        Name = model.Name,
                        Content = model.Content,
                        Image = imageCode,
                        AdCategoryId = model.AdCategoryId,
                        CreationDate = DateTime.Now,
                        Url = model.Url,
                        IFrame = model.IFrame,
                        Campaign = model.Campaign,
                        Horizontal = model.Horizontal,
                        Medium = model.Medium,
                        Source = model.Source,
                        Term = model.Term
                    };

                    Repository.Add(ad);
                    Repository.Save();

                    return true;
                }
                else
                {
                    var currentAd = Repository.Advertises().SingleOrDefault(x => x.AdvertiseId == adId.Value);

                    currentAd.AdCategoryId = model.AdCategoryId;
                    currentAd.Content = model.Content;
                    currentAd.IFrame = model.IFrame;
                    currentAd.Image = imageCode;
                    currentAd.Name = model.Name;
                    currentAd.Url = model.Url;
                    currentAd.Campaign = model.Campaign;
                    currentAd.Horizontal = model.Horizontal;
                    currentAd.Medium = model.Medium;
                    currentAd.Source = model.Source;
                    currentAd.Term = model.Term;

                    Repository.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ServiceModelState.AddModelError("", ex.Message);
                return false;
            }
        }
        public List<Advertise> GetsAdvertises(int id)
        {
            var advertises = Repository.Advertises()
                .Include(x => x.AdCategory)
                .Where(x => x.AdCategoryId == id)
                .OrderBy(x => x.AdCategory)
                .ToList();

            return advertises;
        }
        public List<Advertise> Api_Ads_GetByTags(string tags, int magId)
        {
            try
            {
                var matches = new List<AdCategory>();

                var adGroups = Repository.AdCategories()
                        .Where(x => x.MagazineId == magId)
                        .Where(x => x.Ads.Any())
                        .Where(x => x.Active)
                        .Where(x => x.EndDate > DateTime.Now)
                        .Where(x => !string.IsNullOrEmpty(x.Tags))
                        .Include(x => x.Ads)
                        .ToList();

                if (!adGroups.Any())
                    return null;

                foreach (var item in tags.Split(',').ToList())
                {
                    matches.AddRange(adGroups.Where(x => x.Tags.Contains(item)).ToList());
                }

                var r = new Random();

                if (!matches.Any())
                {
                    adGroups = Repository.AdCategories()
                        .Where(x => x.MagazineId == magId)
                        .Include(x => x.Ads)
                        .ToList();

                    matches = matches.DistinctBy(x => x.AdCategoryId).OrderBy(x => r.Next()).ToList();
                }
                else
                    matches = matches.DistinctBy(x => x.AdCategoryId).OrderBy(x => r.Next()).ToList();

                return matches.First().Ads;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region Quiz
        public List<Quiz> GetQuizs(int id)
        {
            var quizs = Repository.Quizes()
                .Include(x => x.Magazine)
                .Where(x => x.MagazineId == id)
                .OrderBy(x => x.QuizId)
                .ToList();

            return quizs;

        }
        #endregion

        //UNNECESARY CODE
        //public List<Magazine> GetAllMagazinesByUserId(int id)
        //{
        //    var items = Repository.Magazines()
        //        .Where(x => x.UserId == id)
        //        .ToList();
        //    return items;
        //}
        //public int CountNews(int id)
        //{
        //    var news = GetNewsesByMagazineId(id);
        //    return news.Count();
        //}
        //public List<Visit> GetNewsByMagazineId(int id)
        //{
        //    var visits = Repository.Visits()
        //        .Where(x => x.News.Category.MagazineId == id)
        //        .Include(x => x.News)
        //        .ToList();

        //    return visits;
        //}
        //public List<Visit> GetVisits(int id)
        //{
        //    var visits = Repository.Visits()
        //        .Where(x => x.News.Category.MagazineId == id)
        //        .ToList();
        //    return visits;
        //}

        #region API
        public Magazine ApiGetMagazineByGuid(string mGuid)
        {
            return Repository.Magazines()
                .SingleOrDefault(x => x.Guid.Equals(mGuid, StringComparison.OrdinalIgnoreCase));
        }
        public List<Dal.Models.News> ApiGetNewsByMagazine(string mGuid)
        {
            var items = Repository.Newses()
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .Where(x => x.Keywords != "MarcoPaz")
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .OrderByDescending(x => x.CreationDate)
                .ToList();
            return items;
        }
        public Dal.Models.News ApiGetNewById(string mGuid, int nId)
        {
            var item = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.NewsId == nId)
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .SingleOrDefault(x => x.Keywords != "MarcoPaz");

            return item;
        }
        public List<Dal.Models.News> ApiGetLastNewsByCatId(string mGuid, int nId)
        {
            var item = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .SingleOrDefault(x => x.NewsId == nId);

            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .Where(x => x.CategoryId == item.CategoryId)
                .Where(x => x.NewsId != nId)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreationDate)
                .Take(4)
                .ToList();

            return items;
        }
        public List<Dal.Models.News> ApiGetNewsByCatId(string mGuid, int nId)
        {
            var items = Repository.Newses()
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .Where(x => x.CategoryId == nId)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return items;
        }
        public List<Category> ApiGetCategoriesByMagazine(string mGuid)
        {
            var items = Repository.Categories()
                .Where(x => x.Magazine.Guid.Equals(mGuid))
                .ToList();
            return items;
        }
        public Magazine ApiGetMagazine(string mGuid)
        {
            var mag = Repository.Magazines()
                .SingleOrDefault(x => x.Guid.Equals(mGuid));

            return mag;
        }
        public List<Slide> ApiGetMagSlider(string mGuid)
        {
            var slider = Repository.Slides()
                .Include(x => x.News)
                .Include(x => x.News.Category)
                .Where(x => x.News.Category.Magazine.Guid.Equals(mGuid))
                .OrderBy(x => x.Order)
                .ToList();

            return slider;
        }

        public List<KeyPointsContainer> ApiGetDatasByMagazine(string mGuid)
        {
            var data = Repository.KeyPointsContainers()
                .Include(x => x.Magazine)
                .Where(x => x.Magazine.Guid.Equals(mGuid))
                .ToList();

            return data;
        }
        public List<Comment> ApiGetComments(string mGuid, int id)
        {
            var items = Repository.Comments()
                .Where(x => x.NewsId == id)
                .Include(x => x.Users)
                .OrderByDescending(x => x.CreationDate)
                .Where(x => !x.IsDeleted)
                .ToList();

            List<Comment> comments = new List<Comment>();
            foreach (var item in items)
            {
                var a = new Comment
                {
                    CommentId = item.CommentId,
                    Content = item.Content,
                    CreationDate = item.CreationDate,
                    Users = item.Users,
                };
                comments.Add(a);
            }

            return comments;
        }
        public List<Dal.Models.News> ApiGetNewsByDateRange(string mGuid, string fromDate, string toDate)
        {
            DateTime? ultiFromDate = DateTime.Now;
            DateTime? ultiToDate = DateTime.Now;

            if (!string.IsNullOrEmpty(fromDate))
            {
                var tempFromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture)
                    .ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                ultiFromDate = DateTime.ParseExact(tempFromDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            else
            {
                ultiFromDate = null;
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                var tempOutDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    .ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                ultiToDate = DateTime.ParseExact(tempOutDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            else
            {
                ultiToDate = null;
            }

            var tempItems = Repository.Newses()
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .Where(x => !x.Keywords.Contains("MarcoPaz"))
                .Where(x => x.Keywords != "MarcoPaz")
                .ToList();

            var items = tempItems;

            if (ultiFromDate != null && ultiToDate != null)
                items = tempItems.Where(x => x.CreationDate >= ultiFromDate).Where(x => x.CreationDate <= ultiToDate).ToList();

            if (ultiFromDate != null && ultiToDate == null)
                items = tempItems.Where(x => x.CreationDate >= ultiFromDate).ToList();

            if (ultiToDate != null && ultiFromDate == null)
                items = tempItems.Where(x => x.CreationDate <= ultiToDate).ToList();

            return items;
        }
        public List<Dal.Models.News> ApiGetNewsByDateRangeAndTag(string mGuid, string fromDate, string toDate, string tag)
        {
            DateTime? ultiFromDate = DateTime.Now;
            DateTime? ultiToDate = DateTime.Now;

            if (!string.IsNullOrEmpty(fromDate))
            {
                var tempFromDate = DateTime.ParseExact(fromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    .ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                ultiFromDate = DateTime.ParseExact(tempFromDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            else
                ultiFromDate = null;

            if (!string.IsNullOrEmpty(toDate))
            {
                var tempOutDate = DateTime.ParseExact(toDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    .ToString("dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                ultiToDate = DateTime.ParseExact(tempOutDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }
            else
                ultiToDate = null;

            var tempItems = Repository.Newses()
                .Where(x => x.Category.Magazine.Guid.Equals(mGuid))
                .Where(x => x.Keywords != "MarcoPaz")
                .ToList();

            var items = tempItems;

            if (ultiFromDate != null && ultiToDate != null)
                items = tempItems.Where(x => x.CreationDate >= ultiFromDate).Where(x => x.CreationDate <= ultiToDate).ToList();

            if (ultiFromDate != null && ultiToDate == null)
                items = tempItems.Where(x => x.CreationDate >= ultiFromDate).ToList();

            if (ultiToDate != null && ultiFromDate == null)
                items = tempItems.Where(x => x.CreationDate <= ultiToDate).ToList();

            if (!string.IsNullOrEmpty(tag) && !string.IsNullOrWhiteSpace(tag))
                items = tempItems.Where(x => x.Keywords.Contains(tag)).ToList();

            return items;
        }
        public NewsLetter ApiSubscribeEmail(string mGuid, string email)
        {
            try
            {
                var mag = ApiGetMagazineByGuid(mGuid);

                if (mag == null)
                    return null;

                var item = Repository.NewsLetters()
                    .Where(x => x.Magazine.Guid.Equals(mGuid))
                    .SingleOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                    return item;

                var model = new NewsLetter
                {
                    Email = email,
                    MagazineId = mag.MagazineId,
                    CreationDate = DateTime.Now
                };

                Repository.Add(model);
                Repository.Save();
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<AdCategory> ApiGetAdCategories(string mGuid)
        {
            return Repository.AdCategories()
                .Where(x => x.Magazine.Guid.Equals(mGuid))
                .ToList();
        }
        public List<Advertise> ApiGetAdsByMagGuid(string mGuid)
        {
            var item = Repository.Advertises()
                .Include(x => x.AdCategory)
                .Include(x => x.AdCategory.Magazine)
                .Where(x => x.AdCategory.Magazine.Guid.Equals(mGuid))
                .ToList();

            return item;
        }

        public List<Quiz> ApiGetQuizByMagazineId(string mGuid)
        {
            var item = Repository.Quizes()
                .Include(x => x.Magazine)
                .Where(x => x.Magazine.Guid.Equals(mGuid))
                .ToList();

            return item;
        }

        public List<Galery> ApiGetGaleryByMagazineId(string mGuid)
        {
            var item = Repository.Galeries()
                .Include(x => x.Magazine)
                .Include(x => x.GaleryImages)
                .Include(x => x.GaleryImages.Select(z => z.Image))
                .Where(x => x.Magazine.Guid.Equals(mGuid))
                .ToList();

            return item;
        }

        public Answer CreateAnswer(int QuestionId, string Description, int? id)
        {
            var optionId = Repository.Options().SingleOrDefault(x => x.OptionId == id);

            var answer = new Answer()
            {
                QuestionId = QuestionId,
                Description = (!string.IsNullOrEmpty(Description) ? Description : null),
                Value = (optionId != null ? optionId.Value : null),
                CreationDate = DateTime.Now
            };

            Repository.Add(answer);
            Repository.Save();

            return answer;
        }

        public List<Advertise> ApiGetAdsByCatId(string mGuid, int catId)
        {
            var item = Repository.Advertises()
                .Include(x => x.AdCategory)
                .Include(x => x.AdCategory.Magazine)
                .Where(x => x.AdCategoryId == catId)
                //.Where(x => x.AdCategory.Magazine.Guid.Equals(mGuid))
                .ToList();

            return item;
        }

        public List<Dal.Models.News> ApiGetMarcoPazNews()
        {
            var item = Repository.Newses()
                .Where(x => x.Keywords.Contains("MarcoPaz"))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .Include(x => x.Visits)
                .OrderByDescending(x => x.CreationDate)
                .ToList();

            return item;
        }
        public List<Dal.Models.News> ApiGetNewsByTagAndQty(int qty, string tag)
        {
            var item = Repository.Newses()
                .Where(x => x.Keywords == tag)
                .Where(x => x.Keywords.Contains(tag))
                .Include(x => x.Category)
                .Include(x => x.Category.Magazine)
                .Include(x => x.Visits)
                .OrderByDescending(x => x.CreationDate)
                .Take(qty)
                .ToList();

            return item;
        }
        public List<Dal.Models.News> ApiGetNewsByTag(string tag, int magazineId)
        {
            var items = Repository.Newses()
                .Where(x => x.Keywords != "MarcoPaz")
                .Where(x => x.Keywords.Contains(tag))
                .Where(x => x.Category.MagazineId == magazineId)
                .OrderByDescending(x => x.CreationDate)
                .Take(50)
                .ToList();

            return items;
        }
        #endregion

        public static int LevensheinDistance_Get(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedstring = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedstring)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public bool ChangeMagzGuid()
        {
            var magz = Repository.Magazines().SingleOrDefault(x => x.MagazineId == 31);

            var guid = Guid.NewGuid().ToString();

            magz.Guid = guid;

            Repository.Save();

            return true;
        }

        public bool ChangeMagGuid(int id)
        {
            var mag = Repository.Magazines().SingleOrDefault(x => x.MagazineId == id);

            mag.Guid = Guid.NewGuid().ToString();

            Repository.Save();

            return true;
        }

        public bool CalculateTotalVisits()
        {
            var news = Repository.Newses().ToList();

            foreach (var item in news)
            {
                var visits = Repository.Visits().Where(x => x.NewsId == item.NewsId).GroupBy(x => x.Ip).Select(x => x.Key).ToList();

                if (visits.Count == 0 || !visits.Any())
                    item.VisitsCount = 0;
                else
                    item.VisitsCount = visits.Count;

            }

            Repository.Save();

            return true;
        }

        public bool CalculateTotalComments()
        {
            var news = Repository.Newses().ToList();

            foreach (var item in news)
            {
                var comments = Repository.Comments().Where(x => x.NewsId == item.NewsId).ToList();

                if (comments.Count == 0 || !comments.Any())
                    item.CommentsCount = 0;
                else
                    item.CommentsCount = comments.Count;
            }

            Repository.Save();

            return true;
        }

        public bool CalculateNewTotalUpVotes()
        {
            var news = Repository.Newses().ToList();

            foreach (var item in news)
            {
                var votes = Repository.Votes().Where(x => x.NewId == item.NewsId).Where(x => x.UpVoted == 1).ToList();

                if (votes.Count == 0 || !votes.Any())
                    item.UpVotes = 0;
                else
                    item.UpVotes = votes.Count;
            }

            Repository.Save();

            return true;
        }

        public bool CalculateNewTotalVotes(int id)
        {
            var news = Repository.Newses().SingleOrDefault(x => x.NewsId == id);

            var upVotes = Repository.Votes().Where(x => x.NewId == news.NewsId).Where(x => x.UpVoted == 1).ToList();
            var downVotes = Repository.Votes().Where(x => x.NewId == news.NewsId).Where(x => x.UpVoted == 2).ToList();

            if (upVotes.Count == 0 || !upVotes.Any())
                news.UpVotes = 0;
            else
                news.UpVotes = upVotes.Count;

            if (downVotes.Count == 0 || !downVotes.Any())
                news.DownVotes = 0;
            else
                news.DownVotes = downVotes.Count;

            Repository.Save();

            return true;
        }

        public bool CalculateCommentTotalVotes(int id)
        {
            var comment = Repository.Comments().SingleOrDefault(x => x.CommentId == id);

            var upVotes = Repository.Votes().Where(x => x.NewId == comment.NewsId).Where(x => x.UpVoted == 1).ToList();
            var downVotes = Repository.Votes().Where(x => x.NewId == comment.NewsId).Where(x => x.UpVoted == 2).ToList();

            if (upVotes.Count == 0 || !upVotes.Any())
                comment.UpVotes = 0;
            else
                comment.UpVotes = upVotes.Count;

            if (downVotes.Count == 0 || !downVotes.Any())
                comment.DownVotes = 0;
            else
                comment.DownVotes = downVotes.Count;

            Repository.Save();

            return true;
        }

        public bool CalculateNewTotalDownVotes()
        {
            var news = Repository.Newses().ToList();

            foreach (var item in news)
            {
                var votes = Repository.Votes().Where(x => x.NewId == item.NewsId).Where(x => x.UpVoted == 2).ToList();

                if (votes.Count == 0 || !votes.Any())
                    item.DownVotes = 0;
                else
                    item.DownVotes = votes.Count;
            }

            Repository.Save();

            return true;
        }

        public bool CalculateCommentTotalUpVotes()
        {
            var comments = Repository.Comments().ToList();

            foreach (var item in comments)
            {
                var votes = Repository.Votes().Where(x => x.CommentId == item.CommentId).Where(x => x.UpVoted == 1).ToList();

                if (votes.Count == 0 || !votes.Any())
                    item.UpVotes = 0;
                else
                    item.UpVotes = votes.Count;
            }

            Repository.Save();

            return true;
        }

        public bool CalculateCommentTotalDownVotes()
        {
            var comments = Repository.Comments().ToList();

            foreach (var item in comments)
            {
                var votes = Repository.Votes().Where(x => x.CommentId == item.CommentId).Where(x => x.UpVoted == 2).ToList();

                if (votes.Count == 0 || !votes.Any())
                    item.DownVotes = 0;
                else
                    item.DownVotes = votes.Count;
            }

            Repository.Save();

            return true;
        }

        public bool SetCategoryToNews()
        {
            var news = Repository.Newses()
                .Where(x => x.NewsId > 5769)
                .ToList();

            foreach (var item in news)
            {
                item.CategoryId = 150;
                item.IsDeleted = false;
            }

            Repository.Save();
            return true;
        }

        public List<Models.News> GetAllNews()
        {
            var items = Repository.Newses().OrderByDescending(x => x.NewsId).ToList();

            return items;
        }

        public List<Slide> GetSlider(string code)
        {
            var items = Repository.Slides().Include(x => x.Slider).Where(x => x.Slider.sGuid.Equals(code)).Include(x => x.News).Include(x => x.News.Category).ToList();

            return items;
        }

        public string CreateSlug(string phrase)
        {
            int maxLength = 100;

            string str = phrase.ToLower().Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n").Replace("?", "").Replace("¿", "");
            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            // cut and trim it
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();
            // hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }
    }


}