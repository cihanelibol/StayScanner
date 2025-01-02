using CosmosBase.Entites;
using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class HotelsByLocation : BaseEntity
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int ContactCount { get; set; }
        public ReportStatus Status { get; set; }
    }
}
