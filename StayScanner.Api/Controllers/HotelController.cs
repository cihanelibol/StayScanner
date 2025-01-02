using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace StayScanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(HotelDto hotel)
        {
            var result = await _hotelService.CreateHotelAsync(hotel);
            return StatusCode(result.StatusCode,result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid hotelId)
        {
            var result = await _hotelService.DeleteHotelAsync(hotelId);
            return StatusCode(result.StatusCode, result);   
        }
        [HttpGet("GetAuthorizedByHotelId")]
        public async Task<IActionResult> GetAuthorizedByHotelId(Guid hotelId)
        {
            var result = await _hotelService.GetAuthorizedByHotelIdAsync(hotelId);
            return StatusCode(result.StatusCode,result);
        }
        [HttpGet("GetAuthorizedListAsync")]
        public async Task<IActionResult> GetAuthorizedList()
        {
            var result = await _hotelService.GetAuthorizedListAsync();
            return StatusCode(result.StatusCode,result);
        }
    }
}
