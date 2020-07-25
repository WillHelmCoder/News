using System;

namespace Dal.Models
{
    public class KeyPoint
    {
        public int KeyPointId { get; set; }
        public int Order { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string SecondaryImage { get; set; }
        public string MainImage { get; set; }
        public string Url { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsDeleted { get; set; }

        public int KeyPointsContainerId { get; set; }
        public virtual KeyPointsContainer KeyPointsContainer { get; set; }
    }
}