using CosmosBase.Entites;

namespace Hotel.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        public string AuthorizedFirstName { get; set; }
        public string AuthorizedLastName { get; set; }
        public string CompanyName { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
