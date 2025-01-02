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
        [HttpPost]
        public async Task<ApiResponse> CreateReport(CreateReportDto report)
        {
            return await reportService.CreateReportAsync(report);

        }

        [HttpGet]
        public async Task<IActionResult> GetReportList(ReportStatus? reportStatus)
        {
            var result = await reportService.GetReportsAsync(reportStatus);
            return StatusCode(result.StatusCode, result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(Guid id)
        {
            var result = await reportService.GetReportByIdAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Deneme")]
        public async Task<IActionResult> Index()
        {
            await rabbitMqService.SendAsync("location", "hotels_by_location", "report.create", "New York");


            var data = await rabbitMqService.ConsumeAsync("location", "hotels_by_location", "report.create");
            return Ok(data);
        }

    }
}
