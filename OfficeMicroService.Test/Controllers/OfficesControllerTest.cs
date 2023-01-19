using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
        private readonly Mock<IOfficeServices> _service;
        private readonly OfficesController _controller;

        public OfficesControllerTest()
        {
            _service = new Mock<IOfficeServices>();
            _controller = new OfficesController(_service.Object);
        }

        [Fact]
        public async Task GetAll_WithCorrectData_ReturnsStatusOk()
        {
            //Arrange
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
            var officeItemsMapped = new Fixture().CreateMany<OfficeDto>(5);
            officeItemsMapped = officeItemsMapped.Append(officeDto);

            _service.Setup(r => r.GetAllAsync()).ReturnsAsync(officeItemsMapped.ToList());
            //Act
            var actual = await _controller.GetAll();
            //Assert
            actual.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAll_GetAllAsyncThrowException_ReturnsStatusBadRequest()
        {
            //Arrange
            _service.Setup(r => r.GetAllAsync()).ThrowsAsync(new BadHttpRequestException("", StatusCodes.Status400BadRequest));
            //Act
            Func<Task> actual = () => _controller.GetAll();
            //Assert
            actual.Should().ThrowAsync<BadHttpRequestException>();
        }


        [Fact]
        public async Task GetById_WithInValidId_ReturnsStatusNotFound()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            OfficeDto officeDto = null;

            _service.Setup(r => r.GetAsync(id)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.GetById(id);
            //Assert
            actual.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_GetAsyncThrowException_ReturnsStatusBadRequest()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();

            _service.Setup(r => r.GetAsync(id)).ThrowsAsync(new BadHttpRequestException("", StatusCodes.Status400BadRequest));
            //Act
            Func<Task> actual = () => _controller.GetById(id);
            //Assert
            actual.Should().ThrowAsync<BadHttpRequestException>();
        }


        [Fact]
        public async Task GetById_WithCorrectData_ReturnsStatusOk()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();

            var officeDto = new OfficeDto
            {
                Id = id,
                Status = OfficeStatus.Active.ToString(),
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            _service.Setup(r => r.GetAsync(id)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.GetById(id);
            //Assert
            actual.Should().BeOfType<OkObjectResult>();
            (actual as OkObjectResult).Value.Should().BeSameAs(officeDto);
        }

        [Fact]
        public async Task Create_WithInValidModel_ReturnsStatusUnprocessableEntity()
        {
            //Arrange
            OfficeForCreateDto model = new OfficeForCreateDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
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
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = null;
            _service.Setup(r => r.CreateAsync(model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Create(model);
            //Assert
            actual.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Create_WithCorrectData_ReturnsStatusCreated()
        {
            //Arrange
            OfficeForCreateDto model = new OfficeForCreateDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = new OfficeDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            _service.Setup(r => r.CreateAsync(model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Create(model);
            //Assert
            actual.Should().BeOfType<CreatedResult>();
        }


        [Fact]
        public async Task Update_WithInValidModel_ReturnsStatusUnprocessableEntity()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
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
            string id = Guid.NewGuid().ToString();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };
            _service.Setup(r => r.UpdateAsync(id, model)).ThrowsAsync(new BadHttpRequestException("", StatusCodes.Status400BadRequest));
            //Act
            Func<Task> actual = () => _controller.Update(id, model);
            //Assert
            actual.Should().ThrowAsync<BadHttpRequestException>();
        }

        [Fact]
        public async Task Update_WithInCorrectId_ReturnsStatusNotFound()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = null;
            _service.Setup(r => r.UpdateAsync(id, model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Update(id, model);
            //Assert
            actual.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_WithCorrectData_ReturnsStatusNoContent()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            OfficeForUpdateDto model = new OfficeForUpdateDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            OfficeDto officeDto = new OfficeDto
            {
                PhotoId = "6E0122D6-98D5-49D4-AAB3-9700851630F9",
                Street = "Street 1",
                City = "City 1",
                HouseNumber = "1A",
                OfficeNumber = "33",
                RegistryPhoneNumber = "+375338926491"
            };

            _service.Setup(r => r.UpdateAsync(id, model)).ReturnsAsync(officeDto);
            //Act
            var actual = await _controller.Update(id, model);
            //Assert
            actual.Should().BeOfType<NoContentResult>();
        }


        [Fact]
        public async Task ChangeStatus_WithCorrectData_ReturnsStatusNoContent()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            _service.Setup(r => r.ChangeStatusAsync(id));
            //Act
            var actual = await _controller.ChangeStatus(id);
            //Assert
            actual.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task ChangeStatus_WhenChangeStatusAsyncThrowNotFoundException_ReturnsStatusNotFound()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            _service.Setup(r => r.ChangeStatusAsync(id)).Throws(new NotFoundException(""));
            //Act
            var actual = () => _controller.ChangeStatus(id);
            //Assert
            actual.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ChangeStatus_WhenChangeStatusAsyncThrowBadHttpRequestException_ReturnsStatusBadRequest()
        {
            //Arrange
            string id = Guid.NewGuid().ToString();
            _service.Setup(r => r.ChangeStatusAsync(id)).Throws(new BadHttpRequestException("", StatusCodes.Status400BadRequest));
            //Act
            var actual = () => _controller.ChangeStatus(id);
            //Assert
            actual.Should().ThrowAsync<BadHttpRequestException>();
        }
    }
}
