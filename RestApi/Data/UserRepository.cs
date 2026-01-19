//using DotNetMutationTestingSandbox.Data.Model;

namespace DotNetMutationTestingSandbox.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users;
        private int _nextId;

        public UserRepository()
        {
            _users = new Dictionary<int, User>
        {
            { 1, new User { Id = 1, Name = "Alice", Email = "alice@example.com" } },
            { 2, new User { Id = 2, Name = "Bob", Email = "bob@example.com" } }
        };
            _nextId = 3;
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Values.ToList();
        }

        public User GetById(int id)
        {
            return _users[id];
        }

        public User Create(string name, string email)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Id = _nextId++
            };
            _users[user.Id] = user;
            return user;
        }
    }
}
