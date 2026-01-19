# DotNetMutationTestingSandbox
Sample repo showing how to enforce mutation testing threshold gates on pull requests

## Building

```bash
dotnet build
```

## Running unit tests

```bash
# just running the unit tests
dotnet test

# running the tests with mutations
dotnet tool restore
dotnet stryker
```
## Running embedded web server

```bash
dotnet run --project RestApi
```

## Testing the API

With web server running, navigate to
```
http://localhost:5041/Swagger/
```

## Self-guided tutorial to observe how the mutation testing gate works

Let's say we want to enhance our project by adding the ability to find a user by their name.  While it would be more efficient to add functionality like this to our repository class, for educational purposes, we will add it to our service class instead.

Inside IUserService, add a new method:

```
  User GetUser(string name);
```

Inside UserService, implement this method:

```
    public User GetUser(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name is required");
        }
        IEnumerable<User> all = _repository.GetAll();
        User user = all.First(m => m.Name == name);
        return user;
    }
```

Inside UserServiceTests.cs, add a new test method that does not properly assert on the result:

```
        [Test]
        public void GetUserByName_UserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
            var users = new List<User>
            {
                user
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(users);

            // Act
            User result = _service.GetUser(user.Name);
        }

```

Create a pull request.  The mutation testing gate should fail with an error about the mutation score being below the threshold.

Download and view the mutation report from the pull request.  It will be in the artifacts section of the pull request run.
Take note of where mutations (defects) are being added and how the test we added isn't catching this defect.

Improve this test by adding an assert at the end of the test so it looks like this:

```
        [Test]
        public void GetUserByName_UserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
            var users = new List<User>
            {
                user
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(users);

            // Act
            User result = _service.GetUser(user.Name);

            // Assert
            Assert.That(result, Is.EqualTo(user));
        }
```

Add another test to catch a mutation (defect) that will be injected by the mutation testing tool.

```
        [Test]
        public void GetUserByName_UserDoesNotExist()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
            var users = new List<User>
            {
                user
            };
            _mockRepository.Setup(r => r.GetAll()).Returns(users);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _service.GetUser("a username that does not exist"));
        }

```

Download and view the new mutation report and note how our improved test is now catching at least one more of the defects that the mutation testing framework is injecting.
