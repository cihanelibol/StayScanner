using CosmosBase.Entites;
using CosmosBase.Repository.Abstract;
using LinqKit;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Report.Application.Dtos;
using Report.Application.Services.Abstract;
using Report.Domain.Enums;
using Report.Infrastructure.Context;
using System.Net;

namespace Report.Application.Services.Concrete
{
    public class ReportService : IReportService
    {
        private readonly IRabbitMqService rabbitMqService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;

        public ReportService(IRabbitMqService rabbitMqService, IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.rabbitMqService = rabbitMqService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> CreateReportAsync(CreateReportDto report)
        {
            ApiResponse response = new ApiResponse();

            var reportEntity = report.Adapt<Report.Domain.Entities.Report>();
            reportEntity.ReportStatus = Domain.Enums.ReportStatus.GettingReady;

            var data = await unitOfWork.Context.Reports.AddAsync(reportEntity);
            await unitOfWork.Context.SaveChangesAsync();

            response.Data = data.Entity.Id;
            response.StatusCode = (int)HttpStatusCode.Created;
            response.IsSuccessful = true;

            return response;
        }

        public async Task<ApiResponse> GetReportByIdAsync(Guid id)
        {
            ApiResponse response = new ApiResponse();
            var data = await unitOfWork.Context.Reports.Where(c => c.IsDeleted.Equals(false) && c.Id.Equals(id)).SingleOrDefaultAsync();

            response.Data = data;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.IsSuccessful = true;

            return response;
        }

        public async Task<ApiResponse> GetReportsAsync(ReportStatus? reportStatus)
        {

            ApiResponse response = new ApiResponse();

            var predicate = PredicateBuilder.True<Report.Domain.Entities.Report>();

            predicate = predicate.And(c => c.IsDeleted.Equals(false));

            if (!reportStatus.Equals(null))
            {
                predicate.And(c => c.ReportStatus.Equals(reportStatus));
            }

            var data = await unitOfWork.Context.Reports.Where(predicate).ToListAsync();

            response.Data = data.Adapt<List<ReportDto>>();
            response.StatusCode = (int)HttpStatusCode.OK;
            response.IsSuccessful = true;

            return response;
        }


    }
}
