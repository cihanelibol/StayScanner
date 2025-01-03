namespace Hotel.Application.Dto
{
    public record GetHotelsInfoByLocationResponse
    {
        public required string Location {  get; init; }
        public int HotelCount { get; init; }
        public int PhoneNumberCount { get; init; }
    }
}
