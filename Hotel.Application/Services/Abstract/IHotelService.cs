using CosmosBase.Entites;
using Hotel.Application.Dto;

namespace Hotel.Application.Services.Abstract
{
    public interface IHotelService
    {
        public Task<ApiResponse> CreateHotelAsync(HotelDto hotelDto);
        public Task<ApiResponse> DeleteHotelAsync(Guid hotelId);
        public Task<ApiResponse> GetAuthorizedListAsync();
        public Task<ApiResponse> GetAuthorizedByHotelIdAsync(Guid id);
        public Task<ApiResponse> GetHotelsInfoByLocation(string location);
    }
}
