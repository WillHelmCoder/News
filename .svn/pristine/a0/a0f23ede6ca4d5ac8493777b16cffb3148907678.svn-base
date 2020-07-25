using Dal.Interfaces;
using Dal.Models;
using Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.Services
{
    public class ListService : BaseService
    {
        public ListService(Repository repository, ILog logger) : base(repository, logger) { }

        public List<ItemList> GetListOfItemsByMagazineId(int id)
        {
            var item = Repository.ListsOfItems()
                .Where(x => x.MagazineId == id)
                .ToList();

            return item;
        }

        public ItemList GetListOfItemByListId(int id)
        {
            var item = Repository.ListsOfItems().SingleOrDefault(x => x.ItemListId.Equals(id));

            return item;
        }

        public ItemList CreateListOfItems(ItemList model)
        {
            var item = new ItemList()
            {
                MagazineId = model.MagazineId,
                Name = model.Name,
                Content = model.Content,
                IsDeleted = false
            };

            Repository.Add(item);
            Repository.Save();

            return item;
        }

        public bool EditListOfItems(ItemList model)
        {
            try
            {
                var list = Repository.ListsOfItems().SingleOrDefault(x => x.ItemListId.Equals(model.ItemListId));

                if (list == null)
                {
                    return false;
                }

                list.Name = model.Name;
                list.Content = model.Content;
                list.MagazineId = model.MagazineId;

                Repository.Save();

                return true;
            }
            catch (Exception e)
            {
                ServiceModelState.AddModelError("", e.Message);
                return false;
            }
        }

        public bool DeleteListOfItem(int id)
        {
            try
            {
                var list = Repository.ListsOfItems().SingleOrDefault(x => x.ItemListId.Equals(id));

                if (list == null)
                {
                    return false;
                }

                list.IsDeleted = true;

                Repository.Save();

                return true;
            }
            catch (Exception e)
            {
                ServiceModelState.AddModelError("", e.Message);
                return false;
            }
        }
    }
}