using CosmosBase.Entites;
using Hotel.Application.Dto;

namespace Hotel.Application.Services.Abstract
{
    public interface IContactService
    {
        public Task<ApiResponse> CreateContactAsync(ContactDto contact);
        public Task<ApiResponse> DeleteContactAsync(Guid contactId);
        public Task<ApiResponse> GetAllContactsByHotelIdAsync(Guid hotelId);
    }
}
