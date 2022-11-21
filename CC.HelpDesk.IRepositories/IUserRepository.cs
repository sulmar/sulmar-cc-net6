using CC.HelpDesk.Domain;

namespace CC.HelpDesk.IRepositories;

public interface IUserRepository
{
    List<User> GetAll();
    User Get(int id);
    void Add(User user);
    void Update(User user);
    void Remove(int id);
}
