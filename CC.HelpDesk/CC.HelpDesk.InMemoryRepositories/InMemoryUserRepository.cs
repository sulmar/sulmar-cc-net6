using CC.HelpDesk.IRepositories;
using CC.HelpDesk.Domain;
using System.Linq;

namespace CC.HelpDesk.Infrastructure;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> users;

    public InMemoryUserRepository(List<User> users)
    {
        this.users = users;
    }

    public List<User> GetAll()
    {
        return users;
    }

    public User Get(int id)
    {
        return users.SingleOrDefault(user => user.Id == id);
    }

    public User GetByName(string name)
    {
        return GetByFirstName(name);
    }

    public User GetByFirstName(string firstName)
    {
        return users.SingleOrDefault(user => user.FirstName == firstName); // Linq
    }

    
    public User GetByLastName(string lastName)
    {
        return users.SingleOrDefault(user => user.LastName == lastName);
    }

    public void Add(User user)
    {
        var id = users.Max(user => user.Id);
        user.Id = ++id;

        users.Add(user);
    }

    public void Update(User user) 
    {
        var modifiedUser = Get(user.Id);

        modifiedUser.FirstName = user.FirstName;
        modifiedUser.LastName = user.LastName;
        modifiedUser.HashedPassword = user.HashedPassword;
        modifiedUser.Email = user.Email;
        modifiedUser.Salary = user.Salary;

        // Remove(user.Id);
        // users.Add(user);
    }

    public void Remove(int id)
    {
        users.Remove(Get(id));
    }
    
    public bool Exists(int id) {
        return users.Any(user => user.Id == id); // Linq 
    }

}
