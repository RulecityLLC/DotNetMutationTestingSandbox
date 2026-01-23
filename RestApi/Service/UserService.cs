using DotNetMutationTestingSandbox.Data;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<User> GetAllUsers()
    {
        // Add any business logic here
        return _repository.GetAll();
    }

    public User GetUser(int id)
    {
        try
        {
            var user = _repository.GetById(id);
            return user;
        }
        catch
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
    }
    public User CreateUser(string name, string email)
    {
        // Business logic: validate input
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required");
        }

        // Delegate to repository
        return _repository.Create(name, email);
    }

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
}
