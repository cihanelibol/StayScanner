using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace StayScanner.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpPost]
        public async Task<Guid> Create(ContactDto contact)
        {
            return await _contactService.CreateContactAsync(contact);
        }
        [HttpDelete]
        public async Task<bool> Delete(Guid contactId)
        {
            return await _contactService.DeleteContactAsync(contactId);
        }
        [HttpGet("GetAllContactsByHotelId")]
        public async Task<List<ContactDto>> GetAllContactsByHotelIdAsync(Guid hotelId)
        {
            return await _contactService.GetAllContactsByHotelIdAsync(hotelId);
        }
    }
}
