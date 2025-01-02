using CosmosBase.Repository.Abstract;
using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Hotel.Infrastructure.Context;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Guid> CreateHotelAsync(HotelDto hotelDto)
        {
            var hotel = hotelDto.Adapt<Hotel.Domain.Entities.Hotel>();

            var data = await unitOfWork.Context.Hotels.AddAsync(hotel);
            await unitOfWork.SaveChangesAsync();

            return data.Entity.Id;

        }

        public async Task<bool> DeleteHotelAsync(Guid hotelId)
        {
            var data = unitOfWork.Context.Hotels.Where(c => c.Id.Equals(hotelId) && c.IsDeleted.Equals(false)).SingleOrDefault();
            if (data == null)
                throw new ArgumentNullException();

            data.SetIsDeleted(true);
            unitOfWork.Context.Hotels.Update(data);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<AuthorizedPerson> GetAuthorizedByHotelIdAsync(Guid id)
        {
            return await unitOfWork.Context.Hotels.Where(c => c.Id.Equals(id) && c.IsDeleted.Equals(false)).Select(x =>
           new AuthorizedPerson
           {
               AuthorizedFirstName = x.AuthorizedFirstName,
               AuthorizedLastName = x.AuthorizedLastName,

           }).SingleOrDefaultAsync();
        }

        public async Task<List<AuthorizedPerson>> GetAuthorizedListAsync()
        {
            return await unitOfWork.Context.Hotels.Where(x=>x.IsDeleted.Equals(false)).Select(x =>
           new AuthorizedPerson
           {
               AuthorizedFirstName = x.AuthorizedFirstName,
               AuthorizedLastName = x.AuthorizedLastName,

           }).ToListAsync();
        }

      
    }
}
