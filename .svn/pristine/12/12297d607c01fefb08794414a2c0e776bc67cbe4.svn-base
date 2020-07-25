using System;

namespace Dal.Models
{
    public class SmartLink
    {
        public int SmartLinkId { set; get; }

        public string Code { set; get; }
        public string Email { set; get; }

        public DateTime CreationDate { set; get; }
        public DateTime? ActivatonDate { set; get; }
        public DateTime ExpirationDate { set; get; }

        public int SmartLinkTypeId { set; get; }
        public SmartLinkTypes SmartLinkType { get { return (SmartLinkTypes)SmartLinkTypeId; } set { SmartLinkTypeId = (int)value; } }
    }
}