using Report.Application.Dtos;

namespace Report.Application.Services.Abstract
{
    public interface IReportService
    {
        public Task<List<HotelsInfoByLocationDto>> GetHotelsInfoByLocationAsync(string location);
    }
}
