using Hotel.Application.Dto;

namespace Hotel.Application.Services.Abstract
{
    public interface IContactService
    {
        public Task<Guid> CreateContactAsync(ContactDto contact);
        public Task<bool> DeleteContactAsync(Guid contactId);
        public Task<List<ContactDto>> GetAllContactsByHotelIdAsync(Guid hotelId);
    }
}
