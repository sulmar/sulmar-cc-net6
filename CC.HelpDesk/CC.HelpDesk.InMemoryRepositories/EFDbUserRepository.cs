using CC.HelpDesk.Domain;
using CC.HelpDesk.IRepositories;

namespace CC.HelpDesk.Infrastructure
{
    public class EFDbUserRepository : IUserRepository
    {
        private readonly ApiDbContext context;

        public EFDbUserRepository(ApiDbContext context)
        {
            this.context = context;
        }

        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return context.Users.Any(u => u.Id == id);
        }

        public User Get(int id)
        {
            return context.Users.Find(id);
        }

        public List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public User GetByName(string name)
        {
            return context.Users.SingleOrDefault(u => u.FirstName == name);
        }

        public void Remove(int id)
        {
            context.Users.Remove(Get(id));
            context.SaveChanges();
        }

        public void Update(User user)
        {
            context.Update(user);
            context.SaveChanges();
        }
    }
}
