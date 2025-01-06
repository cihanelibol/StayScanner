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
        public async Task<IActionResult> Create(ContactDto contact)
        {
            var response = await _contactService.CreateContactAsync(contact);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid contactId)
        {
            var response = await _contactService.DeleteContactAsync(contactId);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("GetAllContactsByHotelId")]
        public async Task<IActionResult> GetAllContactsByHotelIdAsync(Guid hotelId)
        {
            var response = await _contactService.GetAllContactsByHotelIdAsync(hotelId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
