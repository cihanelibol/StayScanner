using Report.Domain.Enums;

namespace Report.Application.Dtos
{
    public record CreateReportDto
    {
        public string? RequestedBody { get; set; }
        public string? ReportDetail { get; set; }
        public required ReportType ReportType { get; init; }
        
    }
}
