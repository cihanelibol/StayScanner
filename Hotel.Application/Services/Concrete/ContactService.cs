using CosmosBase.Entites;
using CosmosBase.Repository.Abstract;
using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        public async Task<ApiResponse> CreateContactAsync(ContactDto contact)
        {
            var response = new ApiResponse();
            var contactData = contact.Adapt<Contact>();

            var data = await unitOfWork.Context.Contacts.AddAsync(contactData);
            await unitOfWork.SaveChangesAsync();

            response.Data = data.Entity.Id; 
            response.StatusCode = (int)HttpStatusCode.Created;
            response.IsSuccessful = true;
            return response;
        }

        public async Task<ApiResponse> DeleteContactAsync(Guid contactId)
        {
            var response = new ApiResponse();
            var data = unitOfWork.Context.Contacts.Where(c => c.Id.Equals(contactId)).SingleOrDefault();
            if (data == null)
                throw new ArgumentNullException();

            data.SetIsDeleted(true);

            await unitOfWork.SaveChangesAsync();
            response.Data = true;
            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }

        public async Task<ApiResponse> GetAllContactsByHotelIdAsync(Guid hotelId)
        {
            var response = new ApiResponse();   
            var  data = await unitOfWork.Context.Contacts.Where(x => x.HotelId.Equals(hotelId) && x.IsDeleted.Equals(false)).Select(v =>
                new ContactDto
                {
                    Email = v.Email,
                    HotelId = v.HotelId,
                    Location = v.Location,
                    PhoneNumber = v.PhoneNumber,
                    Details = v.Details,
                }
            ).ToListAsync();

            if (data.Count.Equals(0))
            {
                response.Error = "Data not found";
                response.IsSuccessful = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }
            response.Data = data;
            response.IsSuccessful = true;
            response.StatusCode= (int)HttpStatusCode.OK;
            return response;
        }
    }
}
