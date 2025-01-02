using CosmosBase.Entites;

namespace Hotel.Domain.Entities
{
    public class Contact : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
    }
}
