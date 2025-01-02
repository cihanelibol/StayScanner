using CosmosBase.Entites;
using Report.Application.Dtos;

namespace Report.Application.Services.Abstract
{
    public interface IReportService
    {
        public Task<ApiResponse> GetHotelsInfoByLocationAsync(string location);
        public Task<Guid> CreateReportAsync(HotelsInfoByLocationDto hotelsInfoByLocation);


    }
}
