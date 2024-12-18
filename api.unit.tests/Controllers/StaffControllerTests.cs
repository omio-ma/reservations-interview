using Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace api.unit.tests.Controllers
{
    public class StaffControllerTests
    {
        private readonly IConfiguration _config;
        private readonly StaffController _controller;
        private const string ValidCode = "valid-code";

        public StaffControllerTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "staffAccessCode", ValidCode }
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            _controller = new StaffController(_config);

            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Theory]
        [InlineData("invalid-code")]
        [InlineData("")]
        public async Task CheckCode_InvalidCode_ReturnsNoContent(string accessCode)
        {
            // Act
            var result = await _controller.CheckCode(accessCode);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task CheckCode_ValidCode_ReturnsOk()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Staff")
            };

            var mockAuthService = new Mock<IAuthenticationService>();
            _controller.HttpContext.RequestServices = Mock.Of<IServiceProvider>(sp =>
                sp.GetService(typeof(IAuthenticationService)) == mockAuthService.Object
            );

            // Act
            var result = await _controller.CheckCode(ValidCode);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
