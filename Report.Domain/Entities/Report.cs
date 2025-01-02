using CosmosBase.Entites;
using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string? ReportDetail { get; set; }
        public string? ReportObjectType { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}
