using AutoMapper;
using FluentAssertions;
using Moq;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Application.Exceptions;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Repository;

namespace OfficeMicroService.Test.Services
{
    public class OfficeServiceTest
    {
        private readonly Mock<IOfficeRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OfficeServices _officeService;

        public OfficeServiceTest()
        {
            _repositoryMock = new Mock<IOfficeRepository>();
            _mapperMock = new Mock<IMapper>();
            _officeService = new OfficeServices(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_WithValidData_ReturnsItems()
        {
            // Arrange
            var office = new Office
            {
                Id = Guid.NewGuid(),
                Status = OfficeStatus.Active,
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            var officeDto = new OfficeDto
            {
                Id = office.Id,
                Status = office.Status,
                PhotoId = office.PhotoId,
                Street = office.Street,
                City = office.City,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.HouseNumber,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };

            var officeList = new List<Office> { office };
            var officeDtoList = new List<OfficeDto> { officeDto };

            _repositoryMock.Setup(r => r.FindAllAsync()).ReturnsAsync(officeList);
            _mapperMock.Setup(r => r.Map<IEnumerable<OfficeDto>>(officeList)).Returns(officeDtoList);

            // Act
            var actual = await _officeService.GetAllAsync();

            // Assert
            officeDtoList.Should().BeSameAs(actual);
        }

        [Fact]
        public async Task GetAllAsync_WithoutData_ReturnsEmptyList()
        {
            // Arrange
            List<Office> officesItems = null;

            List<OfficeDto> officeItemsMapped = new List<OfficeDto>();

            _repositoryMock.Setup(r => r.FindAllAsync()).ReturnsAsync(officesItems);
            _mapperMock.Setup(r => r.Map<List<OfficeDto>>(officesItems)).Returns(officeItemsMapped);

            // Act
            var actual = await _officeService.GetAllAsync();

            // Assert
            actual.Should().BeSameAs(officeItemsMapped);
        }

        [Fact]
        public async Task GetAsync_WithValidData_ReturnsItem()
        {
            // Arrange
            var id = Guid.NewGuid();
            var officeDto = new OfficeDto
            {
                Id = id,
                Status = OfficeStatus.Active,
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            var offices = new List<Office>
            {
                new Office
                {
                    Id = id,
                    Status = officeDto.Status,
                    PhotoId = officeDto.PhotoId,
                    Street = officeDto.Street,
                    City = officeDto.City,
                    HouseNumber = officeDto.HouseNumber,
                    OfficeNumber = officeDto.OfficeNumber,
                    RegistryPhoneNumber = officeDto.RegistryPhoneNumber
                }
            };

            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDto);

            // Act
            var actual = await _officeService.GetAsync(id);

            // Assert
            actual.Should().BeSameAs(officeDto);
        }

        [Fact]
        public async Task GetAsync_WhenNoDataById_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var offices = new List<Office>();
            Office office = null;
            OfficeDto officeDto = null;


            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);

            // Act
            var actual = await _officeService.GetAsync(id);

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsNull_ReturnsNull()
        {
            // Arrange
            OfficeForCreateDto officeForCreateDto = null;

            // Act
            var actual = await _officeService.CreateAsync(officeForCreateDto);

            // Assert
            actual.Should().BeNull();
            _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Office>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsValid_ReturnsOfficeDtoModel()
        {
            // Arrange
            OfficeForCreateDto officeForCreateDto = new OfficeForCreateDto()
            {
                Status = OfficeStatus.Active,
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            Office office = new Office()
            {
                Status = officeForCreateDto.Status,
                PhotoId = officeForCreateDto.PhotoId,
                Street = officeForCreateDto.Street,
                City = officeForCreateDto.City,
                HouseNumber = officeForCreateDto.HouseNumber,
                OfficeNumber = officeForCreateDto.OfficeNumber,
                RegistryPhoneNumber = officeForCreateDto.RegistryPhoneNumber
            };

            OfficeDto officeDto = new OfficeDto()
            {
                Status = officeForCreateDto.Status,
                PhotoId = officeForCreateDto.PhotoId,
                Street = officeForCreateDto.Street,
                City = officeForCreateDto.City,
                HouseNumber = officeForCreateDto.HouseNumber,
                OfficeNumber = officeForCreateDto.OfficeNumber,
                RegistryPhoneNumber = officeForCreateDto.RegistryPhoneNumber
            };

            _mapperMock.Setup(r => r.Map<Office>(officeForCreateDto)).Returns(office);
            _repositoryMock.Setup(r => r.CreateAsync(office));
            _mapperMock.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);
            // Act
            var actual = await _officeService.CreateAsync(officeForCreateDto);

            // Assert
            actual.Should().BeSameAs(officeDto);
        }

        [Fact]
        public async Task UpdateAsync_WhenIdNotExist_ReturnsNull()
        {
            // Arrange
            OfficeForUpdateDto officeForUpdateDto = new OfficeForUpdateDto()
            {
                Status = OfficeStatus.Active,
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            var id = Guid.NewGuid();
            var offices = new List<Office>();
            Office office = null;
            OfficeDto officeDto = null;


            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);

            // Act
            var actual = await _officeService.UpdateAsync(id, officeForUpdateDto);

            // Assert
            actual.Should().BeNull();
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Office>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenModelIsNull_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var officeDto = new OfficeDto
            {
                Id = id,
                Status = OfficeStatus.Active,
                PhotoId = Guid.NewGuid(),

                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            var offices = new List<Office>
            {
                new Office
                {
                    Id = id,
                    Status = officeDto.Status,
                    PhotoId = officeDto.PhotoId,
                    Street = officeDto.Street,
                    City = officeDto.City,
                    HouseNumber = officeDto.HouseNumber,
                    OfficeNumber = officeDto.OfficeNumber,
                    RegistryPhoneNumber = officeDto.RegistryPhoneNumber
                }
            };

           
            OfficeForUpdateDto officeForUpdateDto = null;

            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDto);

            // Act
            var actual = await _officeService.UpdateAsync(id, officeForUpdateDto);

            // Assert
            actual.Should().BeNull();
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Office>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenIdAndModelValid_ReturnsOfficeDtoModel()
        {
            // Arrange
            var id = Guid.NewGuid();
            var office = new Office()
            {
                Status = OfficeStatus.Active,
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            var offices = new List<Office>
            {
                new Office
                {
                    Id = id,
                    Status = office.Status,
                    PhotoId = office.PhotoId,
                    Street = office.Street,
                    City = office.City,
                    HouseNumber = office.HouseNumber,
                    OfficeNumber = office.OfficeNumber,
                    RegistryPhoneNumber = office.RegistryPhoneNumber
                }
            };
            var officeDtoGet = new OfficeDto
            {
                Id = id,
                Status = office.Status,
                PhotoId = office.PhotoId,
                Street = office.Street,
                City = office.City,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };
            var officeForUpdateDto = new OfficeForUpdateDto()
            {
                Status = office.Status,
                PhotoId = office.PhotoId,
                Street = office.Street,
                City = office.City,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };

           
            var officeDtoUpdate = new OfficeDto
            {
                Id = id,
                Status = office.Status,
                PhotoId = office.PhotoId,
                Street = office.Street,
                City = office.City,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };

            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDtoGet);

            _mapperMock.Setup(r => r.Map<Office>(officeForUpdateDto)).Returns(office);
            _repositoryMock.Setup(r => r.UpdateAsync(office));
            _mapperMock.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDtoUpdate);

            // Act
            var actual = await _officeService.UpdateAsync(id, officeForUpdateDto);

            // Assert
            actual.Should().BeSameAs(officeDtoUpdate);
        }

        [Fact]
        public async Task ChangeStatusAsync_WhenIdNotExist_ThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var offices = new List<Office>();
            Office office = null;
            OfficeDto officeDto = null;


            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);

            // Act
            Func<Task> action = () => _officeService.ChangeStatusAsync(id);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ChangeStatusAsync_WhenDataValid_OnceInUpdate()
        {
            // Arrange
            var id = Guid.NewGuid();
            Office office = new Office()
            {
                Status = OfficeStatus.Inactive,
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            var offices = new List<Office>
            {
                new Office
                {
                    Id = id,
                    Status = office.Status,
                    PhotoId = office.PhotoId,
                    Street = office.Street,
                    City = office.City,
                    HouseNumber = office.HouseNumber,
                    OfficeNumber = office.OfficeNumber,
                    RegistryPhoneNumber = office.RegistryPhoneNumber
                }
            };
            var officeDtoGet = new OfficeDto
            {
                Id = id,
                Status = office.Status,
                PhotoId = office.PhotoId,
                Street = office.Street,
                City = office.City,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber,
                RegistryPhoneNumber = office.RegistryPhoneNumber
            };

            _repositoryMock.Setup(r => r.FindByConditionAsync(k => k.Id == id)).ReturnsAsync(offices);
            _mapperMock.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDtoGet);

            _mapperMock.Setup(r => r.Map<Office>(officeDtoGet)).Returns(office);
            _repositoryMock.Setup(r => r.UpdateAsync(office));

            // Act
            await _officeService.ChangeStatusAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(office), Times.Once);
        }
    }
}