﻿using CosmosBase.Entites;
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
        private readonly IUnitOfWork<ReportDbContext> unitOfWork;

        public ReportService(IRabbitMqService rabbitMqService, IUnitOfWork<ReportDbContext> unitOfWork)
        {
            this.rabbitMqService = rabbitMqService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> GetHotelsInfoByLocationAsync(string location)
        {
            ApiResponse response = new ApiResponse();

             rabbitMqService.SendAsync("report", "create_report", "report.location", location);

            response.Data = "Report queued for preparation";
            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }


        public async Task<ApiResponse> CreateReportAsync(CreateReportDto report)
        {
            ApiResponse response = new ApiResponse();

            var data = report.Adapt<Report.Domain.Entities.Report>();

            data.SetCreatedAt(DateTime.UtcNow);
            data.ReportStatus = ReportStatus.GettingReady;

            var createdData = await unitOfWork.Context.Reports.AddAsync(data);
            await unitOfWork.SaveChangesAsync();
            response.Data = createdData.Entity.Id;
            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
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

        public async Task<ApiResponse> GetReportsAsync(ReportStatus? reportStatus, ReportType? reportType)
        {

            ApiResponse response = new ApiResponse();

            var predicate = PredicateBuilder.True<Report.Domain.Entities.Report>();

            predicate = predicate.And(c => c.IsDeleted.Equals(false));

            if (!reportStatus.Equals(null))
            {
                predicate.And(c => c.ReportStatus.Equals(reportStatus));
            }
            if (!reportType.Equals(null))
            {
                predicate.And(c => c.ReportType.Equals(reportStatus));
            }

            var data = await unitOfWork.Context.Reports.Where(predicate).ToListAsync();

            response.Data = data.Adapt<List<ReportDto>>();
            response.StatusCode = (int)HttpStatusCode.OK;
            response.IsSuccessful = true;

            return response;
        }

        public Task<ApiResponse> UpdateReportAsync(UpdateReportDto location)
        {
            throw new NotImplementedException();
        }
    }
}
