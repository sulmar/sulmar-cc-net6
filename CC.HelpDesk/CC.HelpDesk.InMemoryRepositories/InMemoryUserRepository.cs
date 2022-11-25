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
