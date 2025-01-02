namespace Hotel.Application.Dto
{
    public record ContactDto
    {
        public required Guid HotelId { get; set; }
        public required string PhoneNumber { get; init; }
        public required string Email { get; init; }
        public required string Location { get; init; }
        public string Details { get; init; }
    }
}
