using Hotel.Domain.Entities;

namespace Hotel.Application.Dto
{
    public record HotelDto()
    {
        public string AuthorizedFirstName { get; init; }
        public string AuthorizedLastName { get; init; }
        public string CompanyName { get; init; }
        public List<Contact> Contacts { get; init; }
    }
}
