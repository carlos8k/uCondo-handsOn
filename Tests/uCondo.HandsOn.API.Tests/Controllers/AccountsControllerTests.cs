using Microsoft.AspNetCore.Mvc;
using Moq;
using uCondo.HandsOn.API.Controllers;
using uCondo.HandsOn.Domain.Dtos;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Interfaces.Services;
using uCondo.HandsOn.Domain.Validation;

namespace uCondo.HandsOn.API.Tests.Controllers
{
    public class AccountsControllerTests
	{
        AccountsController _controller;

        [Fact]
        public async Task Get_OkObjectResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => ValidationResult<IEnumerable<AccountDto>>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.GetAsync(null, null, null);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetNextCode_OkObjectResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.GetNextCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(() => ValidationResult<AccountChildSequenceDto>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.GetNextCodeAsync("1");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Create_CreatedResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.CreateAsync(It.IsAny<AccountCreateDto>()))
                .ReturnsAsync(() => ValidationResult<AccountDto>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.CreateAsync(new AccountCreateDto
            {
                Code = "1",
                Name = "Test"
            });

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Create_InvalidCodeDoubleDot_BadRequestObjectResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.CreateAsync(It.IsAny<AccountCreateDto>()))
                .ReturnsAsync(() => ValidationResult<AccountDto>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.CreateAsync(new AccountCreateDto
            {
                Code = "1..0",
                Name = "Test"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_InvalidCodeAlpha_BadRequestObjectResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.CreateAsync(It.IsAny<AccountCreateDto>()))
                .ReturnsAsync(() => ValidationResult<AccountDto>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.CreateAsync(new AccountCreateDto
            {
                Code = "a.b.c.d",
                Name = "Test"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_InvalidNameEmpty_BadRequestObjectResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.CreateAsync(It.IsAny<AccountCreateDto>()))
                .ReturnsAsync(() => ValidationResult<AccountDto>.Success(null));

            _controller = new AccountsController(mock.Object);

            var result = await _controller.CreateAsync(new AccountCreateDto
            {
                Code = "1",
                Name = string.Empty
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_NoContentResult()
        {
            var mock = new Mock<IAccountsService>();

            mock.Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(() => ValidationResult.Success());

            _controller = new AccountsController(mock.Object);

            var result = await _controller.DeleteAsync("1");

            Assert.IsType<NoContentResult>(result);
        }
    }
}