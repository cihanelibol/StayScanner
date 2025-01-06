using CosmosBase.Repository.Abstract;
using Hotel.Application.Dto;
using Hotel.Application.Services.Concrete;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.UnitTest.Application.Services
{
    public class HotelServiceTests
    {
        private readonly Mock<IUnitOfWork<HotelDbContext>> unitOfWorkMock;
        private readonly HotelDbContext dbContext;
        private readonly Hotel.Application.Services.Concrete.HotelService hotelService;


        public HotelServiceTests()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: "HotelTestDb")
                .Options;

            dbContext = new HotelDbContext(options);
            unitOfWorkMock = new Mock<IUnitOfWork<HotelDbContext>>();
            unitOfWorkMock.Setup(u => u.Context).Returns(dbContext);

            hotelService = new HotelService(unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateHotelAsync_Should_Create_Hotel_And_Return_Id()
        {
            var hotelDto = new HotelDto
            {
                CompanyName = "Test Hotel",
                AuthorizedFirstName = "John",
                AuthorizedLastName = "Doe"
            };

            var response = await hotelService.CreateHotelAsync(hotelDto);

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task DeleteHotelAsync_Should_Set_IsDeleted_And_Return_True()
        {
            var hotel = new Hotel.Domain.Entities.Hotel
            {

                CompanyName = "Test Hotel",
                AuthorizedFirstName = "Test Person",
                AuthorizedLastName = "Test Person LastName",

            };

            dbContext.Hotels.Add(hotel);
            await dbContext.SaveChangesAsync();

            var response = await hotelService.DeleteHotelAsync(hotel.Id);

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.True((bool)response.Data);

            var deletedHotel = dbContext.Hotels.SingleOrDefault(h => h.Id == hotel.Id);
            Assert.NotNull(deletedHotel);
            Assert.True(deletedHotel.IsDeleted);
        }

        [Fact]
        public async Task GetAuthorizedByHotelIdAsync_Should_Return_AuthorizedPerson()
        {
            var hotel = new Hotel.Domain.Entities.Hotel
            {
                AuthorizedFirstName = "John",
                AuthorizedLastName = "Doe",
                CompanyName = "John Doe Hotel"
            };

            dbContext.Hotels.Add(hotel);
            await dbContext.SaveChangesAsync();

            var response = await hotelService.GetAuthorizedByHotelIdAsync(hotel.Id);

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

            var authorizedPerson = response.Data as AuthorizedPerson;
            Assert.NotNull(authorizedPerson);
            Assert.Equal("John", authorizedPerson.AuthorizedFirstName);
            Assert.Equal("Doe", authorizedPerson.AuthorizedLastName);
        }
        [Fact]
        public async Task GetHotelsInfoByLocation_Should_Return_Hotel_Info_By_Location()
        {
            var hotel = new Hotel.Domain.Entities.Hotel
            {
                CompanyName = "John Doe Apart",
                AuthorizedFirstName ="John",
                AuthorizedLastName = "Doe",
                Contacts = new List<Contact>
                {
                    new Contact { Location = "Yenişarbademli", PhoneNumber = "1234567890", Details = "5 Oda",Email = "crazyjohn@yahoo.com" }
                }
            };

            dbContext.Hotels.Add(hotel);
            await dbContext.SaveChangesAsync();

            var response = await hotelService.GetHotelsInfoByLocation("Test Location");

            Assert.True(response.IsSuccessful);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

            var locationInfo = response.Data as GetHotelsInfoByLocationResponse;
            Assert.NotNull(locationInfo);
            Assert.Equal("Test Location", locationInfo.Location);
            Assert.Equal(0, locationInfo.HotelCount);
            Assert.Equal(0, locationInfo.PhoneNumberCount);
        }


    }
}
