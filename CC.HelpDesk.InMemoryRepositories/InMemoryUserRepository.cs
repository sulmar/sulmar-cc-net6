using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Domain;
using System.Linq;

namespace CC.HelpDesk.InMemoryRepositories;

public class InMemoryUserRepository : IUserRepository
{
    private List<User> users;

    public InMemoryUserRepository()
    {
        users = new List<User> 
        {
            new User(1, "John", "Smith") { Email = "john.smith@domain.com" },
            new User(2, "Kate", "Smith") { Email = "kate.smith@domain.com" },
            new User(3, "Mark", "Spider") { Email = "mark.spider@domain.com" },
        };
    }

    public List<User> GetAll()
    {
        return users;
    }

    public User Get(int id)
    {
        return users.SingleOrDefault(user => user.Id == id);
    }

    public void Add(User user)
    {
        throw new NotImplementedException();
    }

    public void Update(User user) 
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }
}
