using Report.Domain.Enums;

namespace Report.Application.Dtos
{
    public class UpdateReportDto
    {
        public string? ReportDetail { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}
