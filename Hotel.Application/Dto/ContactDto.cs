namespace Hotel.Application.Dto
{
    public record ContactDto
    {
        public string PhoneNumber { get; init; }
        public string Email { get; init; }
        public string Location { get; init; }
    }
}
