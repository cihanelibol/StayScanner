using CosmosBase.Repository.Abstract;
using Hotel.Application.Dto;
using Hotel.Application.Services.Abstract;
using Hotel.Infrastructure.Context;
using Mapster;
using MapsterMapper;

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

        public async Task<Guid> CreateHotel(HotelDto hotelDto)
        {
            var hotel = hotelDto.Adapt<Hotel.Domain.Entities.Hotel>();

            var data = await unitOfWork.Context.Hotels.AddAsync(hotel);
            await unitOfWork.SaveChangesAsync();

            return data.Entity.Id;

        }
    }
}
