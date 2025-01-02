using Microsoft.AspNetCore.Mvc;
using Report.Application.Services.Abstract;

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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await rabbitMqService.SendAsync("location", "hotels_by_location", "report.create", "New York");


            var data = await rabbitMqService.ConsumeAsync("location", "hotels_by_location", "report.create");
            return Ok(data);
        }

        [HttpGet("GetHotelsInfoByLocation")]
        public async Task<IActionResult> GetHotelsInfoByLocation(string location)
        {
            var result = await reportService.GetHotelsInfoByLocationAsync(location);
            return StatusCode(result.StatusCode, result);

        }

        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            return Ok();
        }

    }
}
