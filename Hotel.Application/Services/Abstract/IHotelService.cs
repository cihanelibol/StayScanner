using Hotel.Application.Dto;

namespace Hotel.Application.Services.Abstract
{
    public interface IHotelService
    {
        public Task<Guid> CreateHotelAsync(HotelDto hotelDto);
        public Task<bool> DeleteHotelAsync(Guid hotelId);
        public Task<List<AuthorizedPerson>> GetAuthorizedListAsync();
        public Task<AuthorizedPerson> GetAuthorizedByHotelIdAsync(Guid id);
    }
}
