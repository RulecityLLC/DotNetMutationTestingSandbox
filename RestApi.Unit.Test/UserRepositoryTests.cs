using DotNetMutationTestingSandbox.Data;

namespace RestApi.Unit.Test
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new UserRepository();
        }

        [Test]
        public void GetAll_ReturnsListOfUsers()
        {
            // Act
            var users = _repository.GetAll();

            // Assert
            Assert.That(users, Is.Not.Null);
            Assert.That(users.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetById_ExistingUser_ReturnsUser()
        {
            // Act
            var user = _repository.GetById(1);

            // Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user!.Id, Is.EqualTo(1));
            Assert.That(user.Name, Is.EqualTo("Alice"));
        }

        [Test]
        public void GetById_NonexistentUser_ReturnsNull()
        {
            // Act/Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.GetById(999));
        }

        [Test]
        public void Create_ValidUser_ReturnsCreatedUser()
        {
            // Arrange
            var newUser = new User { Name = "Charlie", Email = "charlie@example.com" };

            // Act
            var createdUser = _repository.Create(newUser.Name, newUser.Email);

            // Assert
            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.Id, Is.GreaterThan(0));
            Assert.That(createdUser.Name, Is.EqualTo("Charlie"));
            Assert.That(createdUser.Email, Is.EqualTo("charlie@example.com"));
        }
    }
}
