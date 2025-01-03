using CosmosBase.Repository.Abstract;
using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Services.Concrete
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork<HotelDbContext> unitOfWork;
        private readonly IMapper mapper;

        public ContactService(IUnitOfWork<HotelDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateContactAsync(ContactDto contact)
        {
            var contactData = contact.Adapt<Contact>();

            var data = await unitOfWork.Context.Contacts.AddAsync(contactData);
            await unitOfWork.SaveChangesAsync();

            return data.Entity.Id;
        }

        public async Task<bool> DeleteContactAsync(Guid contactId)
        {
            var data = unitOfWork.Context.Contacts.Where(c => c.Id.Equals(contactId)).SingleOrDefault();
            if (data == null)
                throw new ArgumentNullException();

            data.SetIsDeleted(true);

            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<ContactDto>> GetAllContactsByHotelIdAsync(Guid hotelId)
        {
            return await unitOfWork.Context.Contacts.Where(x => x.HotelId.Equals(hotelId) && x.IsDeleted.Equals(false)).Select(v =>
                new ContactDto
                {
                    Email = v.Email,
                    HotelId = v.HotelId,
                    Location = v.Location,
                    PhoneNumber = v.PhoneNumber,
                    Details = v.Details,
                }
            ).ToListAsync();
        }
    }
}
