using System.Collections.Generic;

namespace Dal.Models
{
    public class KeyPointsContainer
    {
        public int KeyPointsContainerId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Kguid { get; set; }

        public bool IsDeleted { get; set; }

        public int MagazineId { get; set; }
        public virtual Magazine Magazine { get; set; }
        
        public virtual List<KeyPoint> KeyPoints { get; set; }
    }
}