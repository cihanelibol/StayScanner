using Hotel.Application.Dto;

namespace Hotel.Application.Services.Abstract
{
    public interface IHotelService
    {
        public Task<Guid> CreateHotel(HotelDto hotelDto);
    }
}
