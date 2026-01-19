public interface IUserService
{
    IEnumerable<User> GetAllUsers();
    User GetUser(int id);
    User CreateUser(string name, string email);
}
