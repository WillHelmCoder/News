namespace Dal.Models
{
    public class Slide
    {
        public int SlideId { set; get; }
        public int Order { set; get; }

        public bool IsDeleted { get; set; }

        public int NewsId { set; get; }
        public News News { set; get; }

        public int SliderId { get; set; }
        public virtual Slider Slider { get; set; }
    }
}
