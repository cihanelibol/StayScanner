using Report.Domain.Enums;

namespace Report.Application.Dtos
{
    public record HotelsInfoByLocationDto
    {
        public string Location { get; init; }
        public int HotelCount { get; init; }
        public int ContactCount { get; init; }
        public ReportStatus Status { get; init; }
    }
}
