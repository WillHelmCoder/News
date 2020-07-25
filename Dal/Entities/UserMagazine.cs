namespace Dal.Models
{
    public class UserMagazine
    {
        public int UserMagazineId { set; get; }

        public int UserId { set; get; }
        public virtual User User { set; get; }

        public int MagazineId { set; get; }
        public virtual Magazine Magazine { set; get; }
    }
}
