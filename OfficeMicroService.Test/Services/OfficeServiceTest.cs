using AutoFixture;
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
        private readonly Mock<IOfficeRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly OfficeServices _officeService;

        public OfficeServiceTest()
        {
            _repository = new Mock<IOfficeRepository>();
            _mapper = new Mock<IMapper>();
            _officeService = new OfficeServices(_repository.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetAllOfficesAsync_WithValidData_ReturnsItems()
        {
            // Arrange
            var office = new Office
            {
                Id = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482"),
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };


            var officeDto = new OfficeDto
            {
                Id = "ECA5E74C-3219-4004-B23D-3AFC2137B482",
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            var officesItems = new Fixture().CreateMany<Office>(5);
            officesItems = officesItems.Append(office);

            var officeItemsMapped = new Fixture().CreateMany<OfficeDto>(5);
            officeItemsMapped = officeItemsMapped.Append(officeDto);

            _repository.Setup(r => r.FindByCondition(r => true)).ReturnsAsync(officesItems.ToList());
            _mapper.Setup(r => r.Map<IEnumerable<OfficeDto>>(officesItems)).Returns(officeItemsMapped.ToList());

            // Act
            var actual = await _officeService.GetAllAsync();

            // Assert
            officeDto.Should().BeSameAs(actual.Last());
        }

        [Fact]
        public async Task GetAllOfficesAsync_WithoutData_ReturnsEmptyList()
        {
            // Arrange
            List<Office> officesItems = null;

            List<OfficeDto> officeItemsMapped = new List<OfficeDto>();

            _repository.Setup(r => r.FindByCondition(r => true)).ReturnsAsync(officesItems);
            _mapper.Setup(r => r.Map<List<OfficeDto>>(officesItems)).Returns(officeItemsMapped);

            // Act
            var actual = await _officeService.GetAllAsync();

            // Assert
            actual.Should().BeSameAs(officeItemsMapped);
        }

        [Fact]
        public async Task GetAsync_WithValidData_ReturnsItem()
        {
            // Arrange
            var guid = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482");
            var offices = new List<Office>
            {
                new Office
                {
                    Id = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482"),
                    Status = OfficeStatus.Active.ToString(),
                    PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                    Street = "Street 1",
                    City = "City 1",
                    HouseNumber = "1A",
                    OfficeNumber = "33",
                    RegistryPhoneNumber = "+375338926491"
                }
            };

            var officeDto = new OfficeDto
            {
                Id = "ECA5E74C-3219-4004-B23D-3AFC2137B482",
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };


            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDto);

            // Act
            var actual = await _officeService.GetAsync(guid.ToString());

            // Assert
            actual.Should().BeSameAs(officeDto);
        }

        [Fact]
        public async Task GetAsync_WhenNoDataById_ReturnsNull()
        {
            // Arrange
            var guid = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482");
            var offices = new List<Office>();
            Office office = null;
            OfficeDto officeDto = null;


            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);

            // Act
            var actual = await _officeService.GetAsync(guid.ToString());

            // Assert
            actual.Should().BeSameAs(officeDto);
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
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsGood_ReturnsOfficeDtoModel()
        {
            // Arrange
            OfficeForCreateDto officeForCreateDto = new OfficeForCreateDto()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            Office office = new Office()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = new OfficeDto()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            _mapper.Setup(r => r.Map<Office>(officeForCreateDto)).Returns(office);
            _repository.Setup(r => r.CreateAsync(office));
            _mapper.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);
            // Act
            var actual = await _officeService.CreateAsync(officeForCreateDto);

            // Assert
            actual.Should().BeSameAs(officeDto);
        }

        [Fact]
        public async Task UpdateAsync_WhenIdInCorrect_ReturnsNull()
        {
            // Arrange
            OfficeForUpdateDto officeForUpdateDto = new OfficeForUpdateDto()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            string id = "ECA5E74C-3219-4004-B23D-3AFC2137B482";
            var guid = Guid.Parse(id);
            var offices = new List<Office>();
            Office office = null;
            OfficeDto officeDto = null;


            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);

            // Act
            var actual = await _officeService.UpdateAsync(id, officeForUpdateDto);

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenModelIsNull_ReturnsNull()
        {
            // Arrange
            string id = "ECA5E74C-3219-4004-B23D-3AFC2137B482";
            var guid = Guid.Parse(id);
            var offices = new List<Office>
            {
                new Office
                {
                    Id = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482"),
                    Status = OfficeStatus.Active.ToString(),
                    PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                    Street = "Street 1",
                    City = "City 1",
                    HouseNumber = "1A",
                    OfficeNumber = "33",
                    RegistryPhoneNumber = "+375338926491"
                }
            };

            var officeDto = new OfficeDto
            {
                Id = "ECA5E74C-3219-4004-B23D-3AFC2137B482",
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            OfficeForUpdateDto officeForUpdateDto = null;

            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDto);

            // Act
            var actual = await _officeService.UpdateAsync(id, officeForUpdateDto);

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenIdAndModelCorrect_ReturnsOfficeDtoModel()
        { 
            // Arrange
            string id = "ECA5E74C-3219-4004-B23D-3AFC2137B482";
            var guid = Guid.Parse(id);
            var offices = new List<Office>
            {
                new Office
                {
                    Id = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482"),
                    Status = OfficeStatus.Active.ToString(),
                    PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                    Street = "Street 1",
                    City = "City 1",
                    HouseNumber = "1A",
                    OfficeNumber = "33",
                    RegistryPhoneNumber = "+375338926491"
                }
            };
            var officeDtoGet = new OfficeDto
            {
                Id = "ECA5E74C-3219-4004-B23D-3AFC2137B482",
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            OfficeForUpdateDto officeForUpdateDto = new OfficeForUpdateDto()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            Office office = new Office()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            OfficeDto officeDtoUpdate = new OfficeDto()
            {
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDtoGet);

            _mapper.Setup(r => r.Map<Office>(officeForUpdateDto)).Returns(office);
            _repository.Setup(r => r.UpdateAsync(office));
            _mapper.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDtoUpdate);

            // Act
            var actual = await _officeService.UpdateAsync(id, officeForUpdateDto);

            // Assert
            Assert.Equal(officeDtoUpdate, actual);
        }

        [Fact]
        public async Task ChangeStatusAsync_WhenIdInCorrect_ReturnsException()
        {
            // Arrange
            string id = "ECA5E74C-3219-4004-B23D-3AFC2137B482";
            var guid = Guid.Parse(id);
            var offices = new List<Office>();
            Office office = null;
            OfficeDto officeDto = null;


            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(office)).Returns(officeDto);

            // Act
            Func<Task> action = () => _officeService.ChangeStatusAsync(id);

            // Assert
            action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ChangeStatusAsync_WhenDataIsCorrect_OnceInUpdate()
        {
            // Arrange
            string id = "ECA5E74C-3219-4004-B23D-3AFC2137B482";
            var guid = Guid.Parse(id);
            var offices = new List<Office>
            {
                new Office
                {
                    Id = Guid.Parse("ECA5E74C-3219-4004-B23D-3AFC2137B482"),
                    Status = OfficeStatus.Active.ToString(),
                    PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                    Street = "Street 1",
                    City = "City 1",
                    HouseNumber = "1A",
                    OfficeNumber = "33",
                    RegistryPhoneNumber = "+375338926491"
                }
            };
            var officeDtoGet = new OfficeDto
            {
                Id = "ECA5E74C-3219-4004-B23D-3AFC2137B482",
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            Office office = new Office()
            {
                Status = OfficeStatus.Inactive.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            _repository.Setup(r => r.FindByCondition(r => r.Id == guid)).ReturnsAsync(offices);
            _mapper.Setup(r => r.Map<OfficeDto>(offices.First())).Returns(officeDtoGet);

            _mapper.Setup(r => r.Map<Office>(officeDtoGet)).Returns(office);
            _repository.Setup(r => r.UpdateAsync(office));

            // Act
            _officeService.ChangeStatusAsync(id);

            // Assert
            _repository.Verify(r => r.UpdateAsync(office), Times.Once);
        }
    }
}