namespace Dal.Models
{
    public class City
    {
        public int CityId { set; get; }

        public string Name { set; get; }

        public int StateId { set; get; }
        public virtual State State { set; get; }
    }
}