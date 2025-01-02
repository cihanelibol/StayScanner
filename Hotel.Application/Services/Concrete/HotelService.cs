using CosmosBase.Entites;
using CosmosBase.Repository.Abstract;
using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Hotel.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Hotel.Application.Services.Concrete
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork<ApplicationDbContext> unitOfWork;
        private readonly IMapper mapper;

        public HotelService(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> CreateHotelAsync(HotelDto hotelDto)
        {
            var response = new ApiResponse();
            var hotel = hotelDto.Adapt<Hotel.Domain.Entities.Hotel>();

            var data = await unitOfWork.Context.Hotels.AddAsync(hotel);
            await unitOfWork.SaveChangesAsync();

            response.Data = data.Entity.Id;
            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.Created;

            return response;

        }

        public async Task<ApiResponse> DeleteHotelAsync(Guid hotelId)
        {
            var response = new ApiResponse();
            var data = unitOfWork.Context.Hotels.Where(c => c.Id.Equals(hotelId) && c.IsDeleted.Equals(false)).SingleOrDefault();
            if (data == null)
                throw new ArgumentNullException();

            data.SetIsDeleted(true);
            unitOfWork.Context.Hotels.Update(data);
            await unitOfWork.SaveChangesAsync();

            response.IsSuccessful = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Data = true;
            return response;
        }

        public async Task<ApiResponse> GetAuthorizedByHotelIdAsync(Guid id)
        {
            var response = new ApiResponse();
            response.Data = await unitOfWork.Context.Hotels.Where(c => c.Id.Equals(id) && c.IsDeleted.Equals(false)).Select(x =>
           new AuthorizedPerson
           {
               AuthorizedFirstName = x.AuthorizedFirstName,
               AuthorizedLastName = x.AuthorizedLastName,

           }).SingleOrDefaultAsync();

            response.StatusCode = (int)HttpStatusCode.OK;
            response.IsSuccessful = true;
            return response;
        }

        public async Task<ApiResponse> GetAuthorizedListAsync()
        {
            var response = new ApiResponse();
            response.Data = await unitOfWork.Context.Hotels.Where(x => x.IsDeleted.Equals(false)).Select(x =>
           new AuthorizedPerson
           {
               AuthorizedFirstName = x.AuthorizedFirstName,
               AuthorizedLastName = x.AuthorizedLastName,

           }).ToListAsync();

            response.StatusCode = (int)HttpStatusCode.OK;
            response.IsSuccessful = true;
            return response;
        }


    }
}
