using CosmosBase.Entites;
using Report.Domain.Enums;

namespace Report.Domain.Entities
{
    public class Report : BaseEntity
    {
        public string? RequestedBody { get; set; }
        public string? ReportDetail { get; set; }
        public ReportType ReportType { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}
