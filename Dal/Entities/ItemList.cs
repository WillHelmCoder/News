namespace Dal.Models
{
    public class ItemList
    {
        public int ItemListId { set; get; }

        public string Name { set; get; }
        public string Content { set; get; }

        public bool IsDeleted { set; get; }

        public int MagazineId { set; get; }
        public virtual Magazine Magazine { set; get; }
    }
}