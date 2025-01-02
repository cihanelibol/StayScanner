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
        public async Task<Guid> Create(HotelDto hotel)
        {
            return await _hotelService.CreateHotelAsync(hotel);
        }
        [HttpDelete]
        public async Task<bool> Delete(Guid hotelId)
        {
            return await _hotelService.DeleteHotelAsync(hotelId);
        }
        [HttpGet("GetAuthorizedByHotelId")]
        public async Task<AuthorizedPerson> GetAuthorizedByHotelId(Guid hotelId)
        {
            return await _hotelService.GetAuthorizedByHotelIdAsync(hotelId);
        }
        [HttpGet("GetAuthorizedListAsync")]
        public async Task<List<AuthorizedPerson>> GetAuthorizedList()
        {
            return await _hotelService.GetAuthorizedListAsync();
        }
    }
}
