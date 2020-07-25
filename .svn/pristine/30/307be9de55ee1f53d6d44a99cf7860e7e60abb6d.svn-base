using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Threading;
using System.Web;
using Dal.Models;
using Dal.Repositories;
using Dal.Interfaces;
using Facebook;
using Microsoft.Ajax.Utilities;
using News = Dal.Models.News;

namespace Dal.Services
{
    public class InfluencerService : BaseService
    {
        public InfluencerService(Repository repository, ILog logger)
            : base(repository, logger)
        {
        }

        public Visit CreateVisit(Int32 id, int? idInfluencer, String iP, DateTime date, String social, String cookie)
        {
            var existingVisit = Repository.Visits()
                .Where(x => x.NewsId == id)
                .Where(x => x.Ip == iP)
                .SingleOrDefault(x => x.cookievalue == cookie);

            if (existingVisit != null)
                return null;

            var tempUsrId = (idInfluencer == 0 ? null : idInfluencer);

            var visit = new Visit
            {
                UserId = tempUsrId,
                NewsId = id,
                Ip = iP,
                Date = date,
                cookievalue = cookie,
                social = social
            };

            var tempNew = Repository.Newses().SingleOrDefault(x => x.NewsId == id);

            if (tempNew != null)
                tempNew.VisitsCount = tempNew.VisitsCount + 1;

            Repository.Add(visit);
            Repository.Save();

            return visit;
        }

        public List<GroupBySocialModel> GetFbCount(Int32 id, Int32 magId)
        {
            var items = (from v in Repository.Visits()
                         where !v.News.IsDeleted
                         where !v.social.Equals("Publico", StringComparison.OrdinalIgnoreCase)
                         where v.News.Category.MagazineId == magId
                         where v.UserId == id
                         where v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)
                         group v by new { NewId = v.NewsId, Social = v.social }
                             into x
                             select x)
                .AsEnumerable()
               .Select(x => new GroupBySocialModel
               {
                   Id = x.Key.NewId,
                   Count = x.Where(v => !v.Ip.Contains("66.220.158.")).GroupBy(v => v.Ip).Count(),
                   Social = x.Key.Social,
               })
               .OrderBy(x => x.Id)
               .ToList();
            return items;
        }


        public List<GroupBySocialModel> GetTwCount(Int32 id, Int32 magId)
        {
            var items = (from v in Repository.Visits()
                         where !v.News.IsDeleted
                         where !v.social.Equals("Publico", StringComparison.OrdinalIgnoreCase)
                         where v.News.Category.MagazineId == magId
                         where v.UserId == id
                         where v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)
                         group v by new { NewId = v.NewsId, Social = v.social }
                             into x
                             select x)
                        .AsEnumerable()
                        .Select(x => new GroupBySocialModel
                        {
                            Id = x.Key.NewId,
                            Count = x.Where(v => !v.Ip.Contains("66.220.158.")).GroupBy(v => v.Ip).Count(),
                            Social = x.Key.Social,
                        })
                        .OrderBy(v => v.Id)
                        .ToList();
            return items;


        }

        public List<GroupBySocialModel> GetGpCount(Int32 id, Int32 magId)
        {
            var items = (from v in Repository.Visits()
                         where !v.News.IsDeleted
                         where !v.social.Equals("Publico", StringComparison.OrdinalIgnoreCase)
                         where v.News.Category.MagazineId == magId
                         where v.UserId == id
                         where v.social.Equals("gp", StringComparison.OrdinalIgnoreCase)
                         group v by new { NewId = v.NewsId, Social = v.social }
                             into x
                             select x)
                        .AsEnumerable()
                        .Select(x => new GroupBySocialModel
                        {
                            Id = x.Key.NewId,
                            Count = x.Where(v => !v.Ip.Contains("66.220.158.")).GroupBy(v => v.Ip).Count(),
                            Social = x.Key.Social,
                        })
                        .OrderBy(v => v.Id)
                        .ToList();
            return items;
        }

        public List<GroupBySocialModel> GetLiCount(Int32 id, Int32 magId)
        {
            var items = (from v in Repository.Visits()
                         where !v.News.IsDeleted
                         where !v.social.Equals("Publico", StringComparison.OrdinalIgnoreCase)
                         where v.News.Category.MagazineId == magId
                         where v.UserId == id
                         where v.social.Equals("li", StringComparison.OrdinalIgnoreCase)
                         group v by new { NewId = v.NewsId, Social = v.social }
                             into x
                             select x)
                    .AsEnumerable()
                    .Select(x => new GroupBySocialModel
                    {
                        Id = x.Key.NewId,
                        Count = x.Where(v => !v.Ip.Contains("66.220.158.")).GroupBy(v => v.Ip).Count(),
                        Social = x.Key.Social,
                    })
                    .OrderBy(v => v.Id)
                    .ToList();
            return items;

        }

        public List<User> GetInfluencers()
        {
            var influencers = Repository.Users()
                .ToList();

            return influencers;
        }

        public List<GroupNewsModel> GetVisitsByInfluencerId(Int32 id)
        {
            var items = (from v in Repository.Visits()
                         where v.UserId == id
                         join n in Repository.Newses() on v.NewsId equals n.NewsId
                         group v by new { NewId = v.NewsId, Title = v.News.Title }
                             into x
                             select x)
                .AsEnumerable()

                .Select(x => new GroupNewsModel
                {
                    Id = x.Key.NewId,
                    Count = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(),
                    Title = x.Key.Title,
                    Facebook = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                    Twitter = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                    LinkedIn = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("li", StringComparison.OrdinalIgnoreCase)),
                    Google = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("gp", StringComparison.OrdinalIgnoreCase))
                })
                .OrderBy(v => v.Id)
                .ToList();

            return items;
        }

        public List<GroupNewsModel> GetVisitsByMagazineIdAndInfluencerId(Int32 id, Int32 magId)
        {
            var items = (from v in Repository.Visits()
                         where v.UserId == id
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
                    Count = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(),
                    Title = x.Key.Title,
                    Facebook = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                    Twitter = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                    LinkedIn = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("li", StringComparison.OrdinalIgnoreCase)),
                    Google = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("gp", StringComparison.OrdinalIgnoreCase))
                })
                .OrderBy(v => v.Id)
                .ToList();

            return items;
        }

        public GroupNewsModel GetCounter(Int32 id)
        {

            var noticia = Repository.Newses()
                .SingleOrDefault(x => x.NewsId == id);

            if (noticia == null)
            {
                return null;
            }

            var items = Repository.Visits()
                .Where(x => x.NewsId == id)
                .ToList();

            var model = new GroupNewsModel
            {
                Id = id,
                Title = noticia.Title,
                Count = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(),
                Facebook = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                Twitter = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                LinkedIn = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("li", StringComparison.OrdinalIgnoreCase)),
                Google = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("gp", StringComparison.OrdinalIgnoreCase)),
                Public = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("Público", StringComparison.OrdinalIgnoreCase))
            };

            return model;

        }

        public List<GroupNewsModel> GetCounterByInfluencer(Int32 id)
        {
            var items = (from v in Repository.Visits()
                         where v.NewsId == id
                         where v.UserId != null
                         join n in Repository.Users() on v.UserId equals n.UserId
                         group v by new { NewId = v.NewsId, UserId = v.UserId, Email = n.Email }
                             into x
                             select x)
                .AsEnumerable()

                .Select(x => new GroupNewsModel
                {
                    Id = x.Key.NewId,
                    Count = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.UserId == x.Key.UserId.Value),
                    Title = x.Key.Email,
                    Facebook = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Where(v => v.UserId == x.Key.UserId.Value).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                    Twitter = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Where(v => v.UserId == x.Key.UserId.Value).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                    LinkedIn = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Where(v => v.UserId == x.Key.UserId.Value).Count(v => v.social.Equals("li", StringComparison.OrdinalIgnoreCase)),
                    Google = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Where(v => v.UserId == x.Key.UserId.Value).Count(v => v.social.Equals("gp", StringComparison.OrdinalIgnoreCase))
                })
                .OrderByDescending(v => v.Id)
                .ToList();



            return items;
        }

        public List<MonthCountModel> GetCounterByInfluencerAndMonth(Int32 id)
        {
            var items = (from v in Repository.Visits()
                         where v.UserId == id
                         where v.social.Contains("fb")
                         join n in Repository.Users() on v.UserId equals n.UserId
                         group v by new { month = v.Date.Month }
                             into x
                             select x)
                .AsEnumerable()

                .Select(x => new MonthCountModel
                {
                    Month = x.Key.month,
                    CountFb = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                    CountTw = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                })
                .OrderByDescending(v => v.Month)
                .ToList();

            return items;
        }

        //public List<MonthCountModel> GetCounterByInfluencerAndTw(Int32 id)
        //{
        //    var items = (from v in Repository.Visits()
        //                 where v.UserId == id
        //                 where v.social.Contains("Tw")
        //                 join n in Repository.Users() on v.UserId equals n.UserId
        //                 group v by new { month = v.Date.Month }
        //                     into x
        //                     select x)
        //        .AsEnumerable()

        //        .Select(x => new MonthCountModel
        //        {
        //            Month = x.Key.month,
        //            Count = x.Where(v => !v.Ip.Contains("66.220.158.")).DistinctBy(v => v.Ip).Count()
        //        })
        //        .OrderByDescending(v => v.Month)
        //        .ToList();

        //    return items;
        //}

        public User GetUser(Int32 id)
        {
            var item = Repository.Users()
                .SingleOrDefault(x => x.UserId == id);
            return item;
        }

        public Int32 GetVisitsByMagazine(Int32 id)
        {
            var item = Repository.Visits()
                       .Include(x => x.News)
                       .Include(x => x.News.Category)
                       .Include(x => x.News.Category.Magazine)
                       .Count(x => x.News.Category.MagazineId == id);
            return item;
        }

        public List<GroupNewsModel> GetMagazineNewsStatistics(Int32 id)
        {
            var noticias = Repository.Newses()
                .Where(x => x.Keywords != "MarcoPaz")
                .Where(x => x.Category.MagazineId == id)
                .Where(x => !x.IsDeleted)
                .ToList();

            if (noticias.Count == 0)
            {
                return null;
            }

            var model = new List<GroupNewsModel>();
            foreach (var item in noticias)
            {
                var items = Repository.Visits()
                            .Where(x => x.NewsId == item.NewsId)
                            .ToList();

                var modelItem = new GroupNewsModel
                {
                    Id = id,
                    Title = item.Title,
                    Count = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(),
                    Facebook = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                    Twitter = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                    LinkedIn = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("li", StringComparison.OrdinalIgnoreCase)),
                    Google = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("gp", StringComparison.OrdinalIgnoreCase)),
                    Public = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("Público", StringComparison.OrdinalIgnoreCase))
                };

                model.Add(modelItem);
            }

            return model;

        }

        public GroupNewsModel GetNewsStatistics(Int32 id)
        {
            var noticia = Repository.Newses()
                .Where(x => x.Keywords != "MarcoPaz")
                .SingleOrDefault(x => x.NewsId == id);

            if (noticia == null)
            {
                return null;
            }

            var items = Repository.Visits()
                        .Where(x => x.NewsId == noticia.NewsId)
                        .ToList();

            var model = new GroupNewsModel
            {
                Id = id,
                Title = noticia.Title,
                Count = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(),
                Facebook = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("fb", StringComparison.OrdinalIgnoreCase)),
                Twitter = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("tw", StringComparison.OrdinalIgnoreCase)),
                LinkedIn = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("li", StringComparison.OrdinalIgnoreCase)),
                Google = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("gp", StringComparison.OrdinalIgnoreCase)),
                Public = items.Where(x => !x.Ip.Contains("66.220.158.")).DistinctBy(x => x.Ip).Count(v => v.social.Equals("Público", StringComparison.OrdinalIgnoreCase))
            };

            return model;

        }
    }
}