namespace DotNetMutationTestingSandbox.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(string name, string email);
    }

}