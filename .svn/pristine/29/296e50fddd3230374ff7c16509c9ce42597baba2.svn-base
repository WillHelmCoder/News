namespace Dal.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int Impact { get; set; }

        public string Title { get; set; }
        public string Comments { get; set; }

        public bool IsPositive { get; set; }
        public bool IsPaid { get; set; }
        public bool IsDeleted { get; set; }

        public int MediaId { get; set; }
        public virtual Media Media { get; set; }

        public int? MagazineId { get; set; }
        public virtual Magazine Magazine { get; set; }

        public int? NewsId { get; set; }
        public virtual News News { get; set; }
    }
}