using CosmosBase.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Report.Application.Services.Abstract;
using Report.Domain.Enums;
using Report.Infrastructure.Context;

namespace Report.Application.BackgroundServices
{
    public class HotelsInfoByLocationReportCreator : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private IUnitOfWork<ReportDbContext> _unitOfWork;

        public HotelsInfoByLocationReportCreator(IServiceScopeFactory serviceScopeFactory, IHttpClientFactory httpClientFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();
                _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<ReportDbContext>>();

                var datas = await _unitOfWork.Context.Reports.Where(c =>
                    !c.IsDeleted &&
                    c.ReportStatus.Equals(ReportStatus.GettingReady) &&
                    c.ReportType.Equals(ReportType.GetHotelsByLocation))
                    .ToListAsync();

                foreach (var report in datas)
                {
                    var client = _httpClientFactory.CreateClient();
                    try
                    {
                        var response = await client.GetAsync($"http://localhost:7146/api/Hotel/GetHotelInfoByLocation?location={report.RequestedBody}", stoppingToken);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            report.ReportStatus = ReportStatus.Completed;
                            report.SetUpdatedAt(DateTime.UtcNow);
                            report.ReportDetail = result;
                            await _unitOfWork.Context.SaveChangesAsync();
                        }
                    }
                    catch (Exception r)
                    {
                        break;
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

    }
}
