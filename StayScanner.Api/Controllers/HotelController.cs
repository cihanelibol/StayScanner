using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace StayScanner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost]
        public async Task<Guid> Create(HotelDto hotel)
        {
            return await _hotelService.CreateHotel(hotel);
        }
    }
}
