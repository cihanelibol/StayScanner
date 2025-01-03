using CosmosBase.Entites;
using Microsoft.AspNetCore.Mvc;
using Report.Application.Dtos;
using Report.Application.Services.Abstract;
using Report.Domain.Enums;

namespace StayScanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IRabbitMqService rabbitMqService;
        private readonly IReportService reportService;
        public ReportController(IRabbitMqService rabbitMqService, IReportService reportService)
        {
            this.rabbitMqService = rabbitMqService;
            this.reportService = reportService;
        }
        [HttpGet("GetHotelInfoByLocationReport")]
        public async Task<IActionResult> GetHotelsInfoByLocation(string location)
        {
            var result = await reportService.GetHotelsInfoByLocationAsync(location);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ApiResponse> CreateReport(CreateReportDto report)
        {
            return await reportService.CreateReportAsync(report);

        }

        [HttpGet]
        public async Task<IActionResult> GetReportList(ReportStatus? reportStatus, ReportType? reportType)
        {
            var result = await reportService.GetReportsAsync(reportStatus, reportType);
            return StatusCode(result.StatusCode, result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var result = await reportService.GetReportByIdAsync(id);

            return StatusCode(result.StatusCode, result);
        }


    }
}
