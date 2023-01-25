using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Application.Exceptions;
using OfficeMicroService.Application.Services;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Presentation.Controllers;

namespace OfficeMicroService.Test.Controllers
{
    public class OfficesControllerTest
    {
        private readonly Mock<IOfficeServices> _serviceMock;
        private readonly OfficesController _controller;

        public OfficesControllerTest()
        {
            _serviceMock = new Mock<IOfficeServices>();
            _controller = new OfficesController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_WithCorrectData_ReturnsStatusOkAndData()
        {
            //Arrange
         
            var officeItemsMapped = new Fixture().CreateMany<OfficeDto>(5).ToList();

            _serviceMock.Setup(r => r.GetAllAsync()).ReturnsAsync(officeItemsMapped);
            //Act
            var actual = await _controller.GetAll();
            //Assert
            actual.Should().BeOfType<OkObjectResult>();
            var result = (actual as OkObjectResult).Value;
            result.Should().NotBeNull();
            result.Should().BeSameAs(officeItemsMapped);
        }

        [Fact]
        public async Task GetAll_GetAllAsyncThrowException_ExceptionIsThrown()
        {
            //Arrange
            _serviceMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception(""));
            //Act
            Func<Task> actual = () => _controller.GetAll();
            //Assert
            await actual.Should().ThrowAsync<Exception>();
        }


        [Fact]
        public async Task GetById_WithInValidId_ReturnsStatusNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            OfficeDto officeDto = null;

            _serviceMock.Setup(r => r.GetAsync(id)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.GetById(id);
            //Assert
            actual.Should().BeOfType<NotFoundObjectResult>();
            string result = (actual as NotFoundObjectResult).Value.ToString();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo($"Can't find office by {id}");
        }

        [Fact]
        public async Task GetById_GetAsyncThrowException_ExceptionIsThrown()
        {
            //Arrange
            var id = Guid.NewGuid();

            _serviceMock.Setup(r => r.GetAsync(id)).ThrowsAsync(new Exception(""));
            //Act
            Func<Task> actual = () => _controller.GetById(id);
            //Assert
            await actual.Should().ThrowAsync<Exception>();
        }


        [Fact]
        public async Task GetById_WithValidData_ReturnsStatusOk()
        {
            //Arrange
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
            _serviceMock.Setup(r => r.GetAsync(id)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.GetById(id);
            //Assert
            actual.Should().BeOfType<OkObjectResult>();
            var result = (actual as OkObjectResult).Value;
            result.Should().NotBeNull();
            result.Should().BeSameAs(officeDto);
        }

        [Fact]
        public async Task Create_WithInvalidModel_ReturnsStatusUnprocessableEntity()
        {
            //Arrange
            OfficeForCreateDto model = new OfficeForCreateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            _controller.ModelState.AddModelError("Status", "Can't be empty.");
            //Act
            var actual = await _controller.Create(model);
            //Assert
            actual.Should().BeOfType<UnprocessableEntityObjectResult>();
        }

        [Fact]
        public async Task Create_WhenCreateAsyncReturnNull_ReturnsStatusBadRequest()
        {
            //Arrange
            OfficeForCreateDto model = new OfficeForCreateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = null;
            _serviceMock.Setup(r => r.CreateAsync(model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Create(model);
            //Assert
            actual.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_WithCorrectData_ReturnsStatusCreated()
        {
            //Arrange
            OfficeForCreateDto model = new OfficeForCreateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = new OfficeDto
            {
                PhotoId = model.PhotoId,
                Street = model.Street,
                City = model.City,
                HouseNumber = model.HouseNumber,
                OfficeNumber = model.OfficeNumber,
                RegistryPhoneNumber = model.RegistryPhoneNumber
            };

            _serviceMock.Setup(r => r.CreateAsync(model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Create(model);
            //Assert
            actual.Should().BeOfType<CreatedResult>();
            var result = (actual as CreatedResult).Value;
            result.Should().NotBeNull();
            result.Should().BeSameAs(officeDto);
        }


        [Fact]
        public async Task Update_WithInValidModel_ReturnsStatusUnprocessableEntity()
        {
            //Arrange
            var id = Guid.NewGuid();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            _controller.ModelState.AddModelError("Status", "Can't be empty.");
            //Act
            var actual = await _controller.Update(id, model);
            //Assert
            actual.Should().BeOfType<UnprocessableEntityObjectResult>();
        }

        [Fact]
        public async Task Update_WhenUpdateAsyncThrowException_ReturnsStatusBadRequest()
        {
            //Arrange
            var id = Guid.NewGuid();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            _serviceMock.Setup(r => r.UpdateAsync(id, model)).ThrowsAsync(new Exception(""));
            //Act
            Func<Task> actual = () => _controller.Update(id, model);
            //Assert
            await actual.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Update_WithInCorrectId_ReturnsStatusNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = null;
            _serviceMock.Setup(r => r.UpdateAsync(id, model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Update(id, model);
            //Assert
            actual.Should().BeOfType<NotFoundObjectResult>();
            string result = (actual as NotFoundObjectResult).Value.ToString();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo($"Can't find office by {id}");
        }

        [Fact]
        public async Task Update_WithCorrectData_ReturnsStatusNoContent()
        {
            //Arrange
            var id = Guid.NewGuid();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = Guid.NewGuid(),
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = new OfficeDto
            {
                PhotoId = model.PhotoId,
                Street = model.Street,
                City = model.City,
                HouseNumber = model.HouseNumber,
                OfficeNumber = model.OfficeNumber,
                RegistryPhoneNumber = model.RegistryPhoneNumber
            };

            _serviceMock.Setup(r => r.UpdateAsync(id, model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Update(id, model);
            //Assert
            actual.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task ChangeStatus_WithCorrectData_ReturnsStatusNoContent()
        {
            //Arrange
            var id = Guid.NewGuid();
            _serviceMock.Setup(r => r.ChangeStatusAsync(id));
            //Act
            var actual = await _controller.ChangeStatus(id);
            //Assert
            actual.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ChangeStatus_WhenChangeStatusAsyncThrowNotFoundException_ReturnsStatusNotFound()
        {
            //Arrange
            var id = Guid.NewGuid();
            _serviceMock.Setup(r => r.ChangeStatusAsync(id)).Throws(new NotFoundException("Office not found."));
            //Act
            var actual = () => _controller.ChangeStatus(id);
            //Assert
           
            await actual.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ChangeStatus_WhenChangeStatusAsyncThrowBadHttpRequestException_ReturnsStatusBadRequest()
        {
            //Arrange
            var id = Guid.NewGuid();
            _serviceMock.Setup(r => r.ChangeStatusAsync(id)).Throws(new Exception(""));
            //Act
            var actual = () => _controller.ChangeStatus(id);
            //Assert
            await actual.Should().ThrowAsync<Exception>();
        }
    }
}
