using CosmosBase.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using Moq;
using Report.Application.Dtos;
using Report.Application.Services.Abstract;
using Report.Application.Services.Concrete;
using Report.Domain.Enums;
using Report.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Report.UnitTest.Application.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IUnitOfWork<ReportDbContext>> unitOfWorkMock;
        private readonly ReportDbContext dbContext;
        private readonly ReportService reportService;

        public ReportServiceTests()
        {
            var options = new DbContextOptionsBuilder<ReportDbContext>()
              .UseInMemoryDatabase(databaseName: "ReportTestDb")
              .Options;

            dbContext = new ReportDbContext(options);
            unitOfWorkMock = new Mock<IUnitOfWork<ReportDbContext>>();
            unitOfWorkMock.Setup(u => u.Context).Returns(dbContext);

            reportService = new ReportService(null, unitOfWorkMock.Object);

        }
        [Fact]
        public async Task CreateReportAsync_Should_Create_Report_And_Return_Id()
        {
            var createReportDto = new CreateReportDto
            {
                ReportDetail = "Test Report",
                ReportType = ReportType.GetHotelsByLocation,
                RequestedBody = ""
            };

            var response = await reportService.CreateReportAsync(createReportDto);

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Data);

        }
        [Fact]
        public async Task GetReportByIdAsync_Should_Return_Report()
        {
            var report = new Report.Domain.Entities.Report
            {
                ReportDetail = "Test Report",
                ReportType = ReportType.GetHotelsByLocation
            };

            dbContext.Reports.Add(report);
            await dbContext.SaveChangesAsync();

            var response = await reportService.GetReportByIdAsync(report.Id);

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

            var returnedReport = response.Data as Report.Domain.Entities.Report;
            Assert.NotNull(returnedReport);
            Assert.Equal(report.Id, returnedReport.Id);
            Assert.Equal("Test Report", returnedReport.ReportDetail);
        }

        [Fact]
        public async Task GetReportsAsync_Should_Return_Filtered_Reports()
        {
            var reports = new List<Report.Domain.Entities.Report>
            {
                new Report.Domain.Entities.Report
                {
                    ReportDetail = "Report 1",
                    ReportStatus = ReportStatus.GettingReady,
                    ReportType = ReportType.GetHotelsByLocation,
                },
                new Report.Domain.Entities.Report
                {
                    ReportDetail = "Report 2",
                    ReportStatus = ReportStatus.Completed,
                    ReportType = ReportType.GetHotelsByLocation,
                }
            };

            dbContext.Reports.AddRange(reports);
            await dbContext.SaveChangesAsync();

            var response = await reportService.GetReportsAsync(ReportStatus.GettingReady, ReportType.GetHotelsByLocation);

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);


        }
    }
}
