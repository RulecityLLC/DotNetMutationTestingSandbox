using DotNetMutationTestingSandbox.Data;
using Moq;

namespace RestApi.Unit.Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockRepository;
        private UserService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IUserRepository>();
            _service = new UserService(_mockRepository.Object);
        }

        [Test]
        public void GetAllUsers_ReturnsUsers()
        {
            // Arrange
            var users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" }
        };
            _mockRepository.Setup(r => r.GetAll()).Returns(users);

            // Act
            var result = _service.GetAllUsers();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            _mockRepository.Verify(r => r.GetAll(), Times.Once);
        }

        [Test]
        public void GetUser_UserExists_ReturnsUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
            _mockRepository.Setup(r => r.GetById(1)).Returns(user);

            // Act
            var result = _service.GetUser(1);

            // Assert
            Assert.That(result.Name, Is.EqualTo("Alice"));
        }

        [Test]
        public void GetUser_UserNotFound_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetById(999)).Throws(new Exception("user not found"));

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _service.GetUser(999));
        }

        [Test]
        public void CreateUser_MissingName_ThrowsException()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.CreateUser(user.Name, user.Email));
            Assert.That(exception!.Message, Does.Contain("Name is required"));
        }

        [Test]
        public void CreateUser_MissingEmail_ThrowsException()
        {
            // Arrange
            var user = new User { Name = "Test" };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _service.CreateUser(user.Name, user.Email));
            Assert.That(exception!.Message, Does.Contain("Email is required"));
        }

        [Test]
        public void CreateUser_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var user = new User { Name = "Charlie", Email = "charlie@example.com" };
            var createdUser = new User { Id = 3, Name = "Charlie", Email = "charlie@example.com" };
            _mockRepository.Setup(r => r.Create(user.Name, user.Email)).Returns(createdUser);

            // Act
            var result = _service.CreateUser(user.Name, user.Email);

            // Assert
            Assert.That(result.Name, Is.EqualTo("Charlie"));
            _mockRepository.Verify(r => r.Create(user.Name, user.Email), Times.Once);
        }
    }
}
