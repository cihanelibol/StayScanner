using CosmosBase.Entites;
using Report.Application.Dtos;
using Report.Domain.Enums;

namespace Report.Application.Services.Abstract
{
    public interface IReportService
    {
        public Task<ApiResponse> GetReportsAsync(ReportStatus? report, ReportType? reportType);
        public Task<ApiResponse> CreateReportAsync(CreateReportDto report);
        public Task<ApiResponse> GetReportByIdAsync(Guid id);
        public Task<ApiResponse> GetHotelsInfoByLocationAsync(string location);
        public Task<ApiResponse> UpdateReportAsync(UpdateReportDto location);


    }
}
