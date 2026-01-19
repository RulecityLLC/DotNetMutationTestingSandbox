using Microsoft.AspNetCore.Mvc;
using Moq;

namespace RestApi.Unit.Test
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IUserService> _mockService;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<IUserService>(MockBehavior.Strict);
            _controller = new UserController(_mockService.Object);
        }

        [Test]
        public void GetUsers_ReturnsOkResult()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "Alice", Email = "alice@example.com" }
            };
            _mockService.Setup(s => s.GetAllUsers()).Returns(users);

            // Act
            var result = _controller.GetUsers();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void GetUsers_Exception()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllUsers()).Throws(new Exception("unknown error"));

            // Act
            var result = _controller.GetUsers();

            // Assert
            var resultInner = result.Result as ObjectResult;
            Assert.That(resultInner!.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public void GetUser_UserExists_ReturnsOkResult()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
            _mockService.Setup(s => s.GetUser(1)).Returns(user);

            // Act
            var result = _controller.GetUser(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void GetUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetUser(999))
                .Throws(new KeyNotFoundException("User not found"));

            // Act
            var result = _controller.GetUser(999);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult!.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void CreateUser_ValidUser_ReturnsCreatedResult()
        {
            // Arrange
            string name = "Charlie";
            string email = "charlie@example.com";
            var userToCreate = new UserWithoutId { Name = name, Email = email };
            var createdUser = new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" };
            _mockService.Setup(s => s.CreateUser(name, email)).Returns(createdUser);

            // Act
            var result = _controller.CreateUser(userToCreate);

            // Assert
            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult!.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public void CreateUser_InvalidUser_ReturnsBadRequest()
        {
            // Arrange
            var userToCreate = new UserWithoutId { Name = "", Email = "test@example.com" };
            var user = new User { Email = "test@example.com" };
            _mockService.Setup(s => s.CreateUser(userToCreate.Name, userToCreate.Email))
                .Throws(new ArgumentException("Name is required"));

            // Act
            var result = _controller.CreateUser(userToCreate);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult!.StatusCode, Is.EqualTo(400));
        }
    }
}