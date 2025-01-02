namespace Report.Application.Dtos
{
    public record CreateReportDto
    {
        public required string ReportDetail { get; init; }
        public required string ReportObjectType { get; init; }
    }
}
