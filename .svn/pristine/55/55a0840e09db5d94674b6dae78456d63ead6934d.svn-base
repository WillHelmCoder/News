using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.Models
{
    public class ModelFactory
    {
        public UserModel Create(User user)
        {
            return new UserModel
            {
                Id = user.UserId,
                Email = user.Email,
                Code = user.Code,
            };
        }

        public NewsModel Article_Response(News news, List<Advertise> ads)
        {
            try
            {
                var headerAd = new Advertise();
                var rightAd = new Advertise();
                var footerAd = new Advertise();

                if (ads != null)
                {
                    if (ads.Any())
                    {
                        var usableAds = new List<Advertise>();

                        var r = new Random();

                        rightAd = ads.Where(x => !x.Horizontal).OrderBy(x => r.Next()).ToList().FirstOrDefault();

                        try
                        {
                            headerAd = ads.Where(x => x.Horizontal).OrderBy(x => r.Next()).ToList().FirstOrDefault();
                            footerAd = ads.Where(x => x.Horizontal).Where(x => x.AdvertiseId != headerAd.AdvertiseId).OrderBy(x => r.Next()).ToList().FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            headerAd = null;
                            footerAd = null;
                        }
                    }
                }
                else
                {
                    headerAd = null;
                    footerAd = null;
                    rightAd = null;
                }

                var returnModel = new NewsModel
                {
                    NewsId = news.NewsId,
                    Title = news.Title,
                    Description = news.Description,
                    Body = news.Body,
                    Image = "http://thecontent.mx/content/data/" + news.Image,
                    CategoryId = news.CategoryId,
                    Category = new Category()
                    {
                        CategoryId = news.CategoryId,
                        Name = news.Category.Name,
                        Permalink = news.Category.Permalink
                    },
                    HeaderAd = headerAd != null ? new Advertise()
                    {
                        Content = headerAd.Content,
                        Horizontal = headerAd.Horizontal,
                        IFrame = headerAd.IFrame,
                        Image = "http://thecontent.mx/content/data/" + headerAd.Image,
                        Name = headerAd.Name,
                        Url = headerAd.Url + "?utm_source=" + headerAd.Source + "&utm_medium=" + headerAd.Medium + "&utm_campaign=" + headerAd.Campaign + "&utm_term=&utm_content=" + headerAd.Campaign,
                    } : null,
                    FooterAd = footerAd != null ? new Advertise()
                    {
                        Content = footerAd.Content,
                        Horizontal = footerAd.Horizontal,
                        IFrame = footerAd.IFrame,
                        Image = "http://thecontent.mx/content/data/" + footerAd.Image,
                        Name = footerAd.Name,
                        Url = footerAd.Url + "?utm_source=" + footerAd.Source + "&utm_medium=" + footerAd.Medium + "&utm_campaign=" + footerAd.Campaign + "&utm_term=&utm_content=" + footerAd.Campaign,
                    } : null,
                    RightAd = rightAd != null ? new Advertise()
                    {
                        Content = rightAd.Content,
                        Horizontal = rightAd.Horizontal,
                        IFrame = rightAd.IFrame,
                        Image = "http://thecontent.mx/content/data/" + rightAd.Image,
                        Name = rightAd.Name,
                        Url = rightAd.Url + "?utm_source=" + rightAd.Source + "&utm_medium=" + rightAd.Medium + "&utm_campaign=" + rightAd.Campaign + "&utm_term=&utm_content=" + rightAd.Campaign,
                    } : null,
                    Date = news.CreationDate.ToLongDateString(),
                    Alt = news.Alt,
                    Keywords = news.Keywords,
                    MetaDesc = news.MetaDesc,
                    Permalink = news.Permalink,
                    Visitas = news.VisitsCount,
                    DownVotes = news.DownVotes,
                    UpVotes = news.UpVotes,
                    Thumbnail = "http://thecontent.mx/content/thumbnails/" + news.Thumbnail
                };

                return returnModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CityModel> Create(List<City> cities)
        {
            var items = cities.Select(x => new CityModel
            {
                CityId = x.CityId,
                Name = x.Name,
            }).ToList();
            return items;
        }
        public List<NewsModel> Create(List<News> newses)
        {
            var items = newses.Select(x => new NewsModel
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Description = x.Description,
                Image = "http://thecontent.mx/content/data/" + x.Image,
                Body = x.Body,
                CategoryId = x.CategoryId,
                Category = new Category()
                {
                    Name = x.Category.Name,
                    ParentCategoryId = x.Category.ParentCategoryId,
                    CategoryId = x.CategoryId,
                    Image = x.Category.Image,
                    IsDeleted = false,
                    MagazineId = x.Category.MagazineId,
                    Permalink = x.Category.Permalink
                },
                Date = x.CreationDate.Day + " de " + x.CreationDate.ToString("MMMM") + " del " + x.CreationDate.Year,
                Alt = x.Alt,
                Keywords = x.Keywords,
                MetaDesc = x.MetaDesc,
                Permalink = x.Permalink,
                Visitas = x.VisitsCount,
                DownVotes = x.DownVotes,
                UpVotes = x.UpVotes,
                CreationDate = x.CreationDate,
                WithVideo = x.WithVideo,
                VideoEmbed = x.VideoEmbed,
                Thumbnail = "http://thecontent.mx/content/thumbnails/"+ x.Thumbnail
            }).ToList();

            return items;
        }

        public List<NewsModel> CreateModelUpgrade(List<News> newses)
        {
            var items = newses.Select(x => new NewsModel
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Description = x.Description,
                Image = "http://thecontent.mx/content/data/" + x.Image,
                Body = x.Body,
                CategoryId = x.CategoryId,
                Category = new Category()
                {
                    Name = x.Category.Name,
                    CategoryId = x.CategoryId,
                    Image = x.Category.Image,
                    IsDeleted = false,
                    MagazineId = x.Category.MagazineId,
                    Magazine = new Magazine()
                    {
                        Domain = x.Category.Magazine.Domain
                    },
                    Permalink = x.Category.Permalink
                },
                Date = x.CreationDate.Day + " de " + x.CreationDate.ToString("MMMM") + " del " + x.CreationDate.Year,
                Alt = x.Alt,
                Keywords = x.Keywords,
                MetaDesc = x.MetaDesc,
                Permalink = x.Permalink,
                Visitas = x.VisitsCount,
                DownVotes = x.DownVotes,
                UpVotes = x.UpVotes,
                CreationDate = x.CreationDate,
                WithVideo = x.WithVideo,
                VideoEmbed = x.VideoEmbed,
                Thumbnail = "http://thecontent.mx/content/thumbnails/" + x.Thumbnail
            }).ToList();

            return items;
        }

        public List<SliderModel> Create(List<Slide> sliders)
        {
            var items = sliders.Select(x => new SliderModel
            {
                SliderId = x.SliderId,
                NewsId = x.NewsId,
                News = new News()
                {
                    NewsId = x.NewsId,
                    Alt = x.News.Alt,
                    Description = x.News.Description,
                    Image = "http://thecontent.mx/content/data/" + x.News.Image,
                    Permalink = x.News.Permalink,
                    Title = x.News.Title,
                    WithVideo = x.News.WithVideo,
                    VideoEmbed = x.News.VideoEmbed,
                    CategoryId = x.News.CategoryId,
                    Category = new Category()
                    {
                        Name = x.News.Category.Name,
                        CategoryId = x.News.CategoryId,
                        Permalink = x.News.Category.Permalink
                    },
                },
                Order = x.Order
            }).ToList();

            return items;
        }

        public KeyPointsContainer Create(KeyPointsContainer datas)
        {
            var items = new KeyPointsContainer
            {
                KeyPointsContainerId = datas.KeyPointsContainerId,
                Name = datas.Name,
                Description = datas.Description,
                Kguid = datas.Kguid,
                IsDeleted = datas.IsDeleted,
                KeyPoints = datas.KeyPoints.OrderBy(z => z.Order).Select(z => new KeyPoint
                {
                    KeyPointId = z.KeyPointId,
                    Title = z.Title,
                    Description = z.Description,
                    Url = z.Url,
                    Order = z.Order,
                    MainImage = "http://TheContent.mx/Content/Data/" + z.MainImage,
                    SecondaryImage = "http://TheContent.mx/Content/Data/" + z.SecondaryImage,
                    CreationDate = z.CreationDate,
                    IsDeleted = z.IsDeleted,
                    KeyPointsContainerId = z.KeyPointsContainerId
                }).ToList()
            };

            return items;
        }

        public CategoryModel Create(Category model)
        {
            return new CategoryModel
            {
                CategoryId = model.CategoryId,
                Image = "http://thecontent.mx/content/data/" + model.Image,
                Name = model.Name,
                MagazineTitle = model.Magazine.Title,
                MagazineId = model.MagazineId,
                Permalink = model.Permalink
            };
        }
        public List<CategoryModel> Create(List<Category> categories)
        {
            var items = categories.Select(x => new CategoryModel
            {
                CategoryId = x.CategoryId,
                ParentCategoryId = x.ParentCategoryId,
                Image = "http://thecontent.mx/content/data/" + x.Image,
                Name = x.Name,
                MagazineTitle = x.Magazine.Title,
                MagazineId = x.MagazineId,
                Permalink = x.Permalink,
                Height = x.Height,
                Width = x.Width

            }).ToList();

            return items;
        }
        public MagazineModel Create(Magazine model)
        {
            return new MagazineModel
            {
                MagazineId = model.MagazineId,
                Title = model.Title,
                Code = model.Code,
                Description = model.Description,
                Logo = model.Logo,
                Address = model.Address,
                CityId = model.CityId,
                Email = model.Email,
                User = model.User.Email
            };
        }
        public List<MagazineModel> Create(List<Magazine> model)
        {
            var items = model.Select(x => new MagazineModel
            {
                MagazineId = x.MagazineId,
                Title = x.Title,
                Code = x.Code,
                Description = x.Description,
                Logo = x.Logo,
                Address = x.Address,
                CityId = x.CityId,
                Email = x.Email,
                User = x.User.Email
            }).ToList();

            return items;
        }
        public List<GroupNewsModel> Create(List<GroupNewsModel> model)
        {
            var items = model.Select(x => new GroupNewsModel
            {
                Count = x.Count,
                Title = x.Title,
                Facebook = x.Facebook,
                Twitter = x.Twitter,
                LinkedIn = x.LinkedIn,
                Google = x.Google,
                Public = x.Public
            }).ToList();

            return items;
        }
        public List<VisitsByMonth> Create(List<MonthCountModel> model)
        {
            var items = model.Select(x => new VisitsByMonth
            {
                CountFb = x.CountFb,
                CountTw = x.CountTw,
            }).ToList();

            return items;
        }
        public NewsWithVisitsModel Create(NewsWithVisitsModel model)
        {
            return new NewsWithVisitsModel
            {
                TotalNews = model.TotalNews,
                TotalVisits = model.TotalVisits,
                VisitCounts = model.VisitCounts
            };
        }
        public List<Comment> Create(List<Comment> comments)
        {
            var items = comments.Select(x => new Comment()
            {
                Content = x.Content,
                Users = x.Users,
                CreationDate = x.CreationDate,
                UpVotes = x.UpVotes,
                DownVotes = x.DownVotes
            }).ToList();

            return items;
        }

        public List<AdCategory> Create(List<AdCategory> adCategories)
        {
            var items = adCategories.Select(x => new AdCategory()
            {
                Name = x.Name,
                AdCategoryId = x.AdCategoryId
            }).ToList();

            return items;
        }

        public List<Advertise> Create(List<Advertise> advertises)
        {
            var items = advertises.Select(x => new Advertise()
            {
                AdvertiseId = x.AdvertiseId,
                Name = x.Name,
                Content = x.Content,
                CreationDate = x.CreationDate,
                Image = "http://www.thecontent.mx/content/data/" + x.Image,
                AdCategoryId = x.AdCategoryId,
                IsDeleted = x.IsDeleted,
                IFrame = x.IFrame,
                Url = x.Url
            }).ToList();

            return items;
        }

        public List<Quiz> Create(List<Quiz> Quizs)
        {
            var items = Quizs.Select(x => new Quiz()
            {
                QuizId = x.QuizId,
                Title = x.Title,
                Description = x.Description,
                CreationDate = x.CreationDate,
                MagazineId = x.MagazineId
            }).ToList();

            return items;
        }

        public NewsLetter CreateNewsletterSubscription(NewsLetter model)
        {
            return new NewsLetter
            {
                Email = model.Email
            };
        }
        public List<Response_GaleryModel> CreateGaleries_ResponseModel(List<Galery> galeries)
        {
            return galeries.Select(x => new Response_GaleryModel()
            {
                GaleryId = x.GaleryId,
                Title = x.Title,
                Description = x.Description,
                MetaDesc = x.Description,
                Alt = x.Alt,
                CreationDate = x.CreationDate.ToShortDateString(),
                CreationDateTime = x.CreationDate,
                Keywords = x.Keywords,
                Permalink = x.Keywords,
                Images = x.GaleryImages.OrderBy(z => z.Order).Select(z => new Response_GaleryImageModel
                {
                    GaleryImageId = z.GaleryImageId,
                    Title = z.Title,
                    Description = z.Description,
                    ImageCode = "http://www.thecontent.mx/content/data/" + z.Image.Code,
                }).ToList()
            }).ToList();
        }

        public Response_GaleryModel CreateGalery_ResponseModel(Galery galery)
        {
            return new Response_GaleryModel()
            {
                GaleryId = galery.GaleryId,
                Title = galery.Title,
                Description = galery.Description,
                MetaDesc = galery.Description,
                Alt = galery.Alt,
                CreationDate = galery.CreationDate.ToShortDateString(),
                Keywords = galery.Keywords,
                Permalink = galery.Keywords,
                Images = galery.GaleryImages.OrderBy(z => z.Order).Select(z => new Response_GaleryImageModel
                {
                    GaleryImageId = z.GaleryImageId,
                    Title = z.Title,
                    Description = z.Description,
                    ImageCode = z.Image.Code,
                }).ToList()
            };
        }

        public List<ItemList> CreateList(List<ItemList> lists)
        {
            var items = lists.Select(x => new ItemList()
            {
                ItemListId = x.ItemListId,
                MagazineId = x.MagazineId,
                Name = x.Name,
                Content = x.Content,
                IsDeleted = x.IsDeleted
            }).ToList();

            return items;
        }
    }
}