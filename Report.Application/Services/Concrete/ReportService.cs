using CosmosBase.Entites;
using CosmosBase.Repository.Abstract;
using Mapster;
using MapsterMapper;
using Report.Application.Dtos;
using Report.Application.Services.Abstract;
using Report.Domain.Entities;
using Report.Infrastructure.Context;
using System.Net;

namespace Report.Application.Services.Concrete
{
    public class ReportService : IReportService
    {
        private readonly IRabbitMqService rabbitMqService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;

        public ReportService(IRabbitMqService rabbitMqService, IMapper mapper, IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.rabbitMqService = rabbitMqService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> GetHotelsInfoByLocationAsync(string location)
        {
            var response = new ApiResponse();

            if (string.IsNullOrWhiteSpace(location))
            {
                response.IsSuccessful = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Error = "Please provide a valid location";
                return response;
            }

            await rabbitMqService.SendAsync("location", "hotels_by_location", "report.create", location);

            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Data = "Report queued.";

            return response;

        }

        public async Task<Guid> CreateReportAsync(HotelsInfoByLocationDto hotelsInfoByLocation)
        {
            var report = hotelsInfoByLocation.Adapt<HotelsByLocation>();
            report.Status =Domain.Enums.ReportStatus.GettingReady;

            var data = await unitOfWork.Context.HotelsByLocations.AddAsync(report);
            await unitOfWork.Context.SaveChangesAsync();

            report.Status = Domain.Enums.ReportStatus.Completed;
            await unitOfWork.Context.SaveChangesAsync();

            return data.Entity.Id;
        }
    }
}
